using DataBaseAccess;
using NarodnaSkupstina.Entiteti;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina
{


    public class DTOManager
    {
        #region NarodniPoslanik

        public async static Task<Result<List<NarodniPoslanikView>, ErrorMessage>> vratiSveNarodnePoslanikeAsync()
        {
            ISession? s = null;
            List<NarodniPoslanikView> np = new ();
            try
            {
                 s = DataLayer.GetSession();

                if(!(s?.IsConnected ?? false))
                {
                    return "Sesija se ne moze otvoriti".ToError(403);
                }

                IEnumerable<NarodniPoslanik> sviPoslanici = from o in await s.Query<NarodniPoslanik>().ToListAsync()
                                                                                      select o;
                foreach(NarodniPoslanik p in sviPoslanici)
                {
                    var posla = new NarodniPoslanikView(p)
                    {
                        //
                    };
                    np.Add(posla);
                }
  
            }
            catch (Exception )
            {
                return "Nemoguce vratiti sve Narodne Poslanike".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return np;
        }

        public static List<NarodniPoslanikPregled> vratiSveNarodnePoslanikePgrupe(string naziv)
        {
            List<NarodniPoslanikPregled> poslanici = new List<NarodniPoslanikPregled>();
            try
            {
                ISession s = DataLayer.GetSession();
                IEnumerable<NarodnaSkupstina.Entiteti.NarodniPoslanik> sviPoslanici = from o in s.Query<NarodnaSkupstina.Entiteti.NarodniPoslanik>()
                                                                                      where o.PGrupa.Naziv== naziv
                                                                                      select o;
                foreach (NarodnaSkupstina.Entiteti.NarodniPoslanik r in sviPoslanici)
                {
                    poslanici.Add(new NarodniPoslanikPregled(r.Id, r.JMBG, r.IdentifikacioniBroj, r.Ime, r.ImeJednogRoditelja, r.Prezime, r.DatumRodjenja, r.MestoRodjenja, r.Grad, r.Adresa, r.BrojTelefona, r.BrojMobilnog, r.StalniRadniOdnos));
                }
                s.Close();
            }
            catch (Exception e)
            { Console.WriteLine(e.ToString()); } return poslanici;
        }

        public static List<NarodniPoslanikBasic> vratiNPAnketni(string tip)
        {
            List<NarodniPoslanikBasic> poslanici = new List<NarodniPoslanikBasic>();
            try
            {
                ISession s = DataLayer.GetSession();
                IEnumerable<NarodnaSkupstina.Entiteti.NarodniPoslanik> sviPoslanici = from o in s.Query<NarodnaSkupstina.Entiteti.NarodniPoslanik>()
                                                                                      where o.RadnoT.Tip == tip
                                                                                      select o;
                foreach (NarodnaSkupstina.Entiteti.NarodniPoslanik r in sviPoslanici)
                {
                    RadnoTeloBasic rt = DTOManager.vratiRadnoTelo(r.RadnoT.Tip);
                    PoslanickaBasic pgrupica = DTOManager.vratiPGrupu(r.PGrupa.Naziv);
                    poslanici.Add(new NarodniPoslanikBasic(r.Id, r.JMBG, r.IdentifikacioniBroj, r.Ime, r.ImeJednogRoditelja, r.Prezime, r.DatumRodjenja, r.MestoRodjenja, r.Grad, r.Adresa, r.BrojTelefona, r.BrojMobilnog, r.StalniRadniOdnos, pgrupica,rt));
                }
                s.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString()); 
            }
            return poslanici;
        }

        public static void obrisiPoslanika(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                NarodnaSkupstina.Entiteti.NarodniPoslanik n = s.Load<NarodnaSkupstina.Entiteti.NarodniPoslanik>(id);
                s.Delete(n);
                s.Flush();

                s.Close();
            }
            catch(Exception ex) { Console.WriteLine(ex.ToString());
        }    
         }

        public static void dodajPoslanika(NarodniPoslanikBasic nb)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.NarodniPoslanik n = new NarodnaSkupstina.Entiteti.NarodniPoslanik();
                n.JMBG=nb.JMBG;
                n.IdentifikacioniBroj=nb.Identif; //mzd nije dobro mzd treba ide id
                n.Ime = nb.Ime;
                n.ImeJednogRoditelja = nb.ImeRod;
                n.Prezime = nb.Prezime;
                n.DatumRodjenja = nb.DatumR;
                n.MestoRodjenja = nb.MestoR;
                n.Grad=nb.Grad;
                n.Adresa = nb.Adresa;
                n.BrojTelefona = nb.BrojT;
                n.BrojMobilnog = nb.BrojM;
                n.StalniRadniOdnos = nb.StalniR; //ovo sad dodato
                //protectrd je set pa zato nemoze vidi za to
                s.SaveOrUpdate(n);

                s.Flush();
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine (ec.ToString());
            }
        }

        public static NarodniPoslanikBasic azurirajPoslanika(NarodniPoslanikBasic r)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                NarodnaSkupstina.Entiteti.NarodniPoslanik n = s.Load<NarodnaSkupstina.Entiteti.NarodniPoslanik>(r.Id
                    /*mzd ide identif*/);
                n.JMBG = r.JMBG;
                n.IdentifikacioniBroj = r.Identif; //mzd nije dobro mzd treba ide id
                n.Ime = r.Ime;
                n.ImeJednogRoditelja = r.ImeRod;
                n.Prezime = r.Prezime;
                n.DatumRodjenja = r.DatumR;
                n.MestoRodjenja = r.MestoR;
                n.Grad = r.Grad;
                n.Adresa = r.Adresa;
                n.BrojTelefona = r.BrojT;
                n.BrojMobilnog = r.BrojM;

                s.Update(n);
                s.Flush();
                s.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return r; //msm da ovde ide o al ono
        }

        public static NarodniPoslanikBasic vratiNarodnog(int id)
        {
            NarodniPoslanikBasic nb = new NarodniPoslanikBasic();
            try
            {
                ISession s = DataLayer.GetSession();
                NarodnaSkupstina.Entiteti.NarodniPoslanik n = s.Load<NarodnaSkupstina.Entiteti.NarodniPoslanik>(id);
                RadnoTeloBasic rt = DTOManager.vratiRadnoTelo(n.RadnoT.Tip);
                PoslanickaBasic pgrupica = DTOManager.vratiPGrupu(n.PGrupa.Naziv);
                nb = new NarodniPoslanikBasic(n.Id, n.JMBG, n.IdentifikacioniBroj, n.Ime, n.ImeJednogRoditelja, n.Prezime, n.DatumRodjenja, n.MestoRodjenja, n.Grad, n.Adresa, n.BrojTelefona, n.BrojMobilnog, n.StalniRadniOdnos, pgrupica, rt);
                s.Close();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
                return nb;
        }

        

        #endregion

            #region StalniRadnik

        public static List<StalniRadnikBasic> vratiSveStalneRadnike()
        {
            List<StalniRadnikBasic> sr = new List<StalniRadnikBasic>();
            try
            {
                ISession s = DataLayer.GetSession();

                IEnumerable<NarodnaSkupstina.Entiteti.StalniRadnik> sviStalniRadnici = from o in s.Query<NarodnaSkupstina.Entiteti.StalniRadnik>()
                                                                                       
                                                                                       select o;
                foreach (NarodnaSkupstina.Entiteti.StalniRadnik p in sviStalniRadnici)
                {
                    sr.Add(new StalniRadnikBasic(p.Id, p.JMBG, p.IdentifikacioniBroj, p.Ime, p.ImeJednogRoditelja, p.Prezime, p.DatumRodjenja, p.MestoRodjenja, p.Grad, p.Adresa, p.BrojTelefona, p.BrojMobilnog, p.StalniRadniOdnos));
                }
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine("Problem", ec.Message);
            }
            return sr;
        }

        public static StalniRadnikBasic vratiStalnog(int id)
        {
            StalniRadnikBasic srb = new StalniRadnikBasic();
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.StalniRadnik sr = s.Load<NarodnaSkupstina.Entiteti.StalniRadnik>(id);
                srb = new StalniRadnikBasic(sr.Id, sr.JMBG, sr.IdentifikacioniBroj, sr.Ime, sr.ImeJednogRoditelja, sr.Prezime, sr.DatumRodjenja, sr.MestoRodjenja, sr.Grad, sr.Adresa, sr.BrojTelefona, sr.BrojMobilnog, sr.StalniRadniOdnos);
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine("Doslo je do greske kod vratiStalnog", ec.Message);
            }
            return srb;
        }

        public static List<StalniRadniOdnosBasic> vratiSveStalneOdnosRadnike(/*int id*/)
        {
            List<StalniRadniOdnosBasic> stalniO = new List<StalniRadniOdnosBasic>();
            try
            {
                ISession s = DataLayer.GetSession();
                IEnumerable<NarodnaSkupstina.Entiteti.StalniRadniOdnos> sviOdnos = from o in s.Query<NarodnaSkupstina.Entiteti.StalniRadniOdnos>()
                                                                                   /*where o.StalniRadnik.StalniRadniOdnos==true */                                                                            
                                                                                   select o;
                foreach(NarodnaSkupstina.Entiteti.StalniRadniOdnos sviO in sviOdnos)
                {
                    StalniRadnikBasic stalniRadnik = DTOManager.vratiStalnog(sviO.StalniRadnik.Id);
                    stalniO.Add(new StalniRadniOdnosBasic(sviO.Id, sviO.BrojRadneKnjizice, sviO.PrethodanRadniStazGod, sviO.PrethodanRadniStazMesec, sviO.PrethodanRadniStazDan,sviO.ImeFirme,stalniRadnik));
                }
                s.Close();
            }
            catch(Exception ex)
            {
                //
            }
            return stalniO;
        }

        public static void obrisiStalnog(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.StalniRadniOdnos r = s.Load<NarodnaSkupstina.Entiteti.StalniRadniOdnos>(id);

                s.Delete(r);
                s.Flush();

                s.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #region StalniRadniOdnos



        #endregion

        #region SluzbenaProstorija

        public static void obrisiSluzbenaProstorija(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                SluzbenaProstorija prost = s.Load<SluzbenaProstorija>(id);
                //mzd nije samo obicna prostortija nego basic il tk nes


                s.Delete(prost);
                s.Flush();



                s.Close();

            }
            catch (Exception ec)
            {
                //handle exceptions
            }


        }
        public static List<SluzbenaProstorijaPregled> vratiPGrupaProstorije(int id)
        {
            List<SluzbenaProstorijaPregled> prost = new List<SluzbenaProstorijaPregled>();
            try
            {
                ISession s = DataLayer.GetSession();
                IEnumerable<NarodnaSkupstina.Entiteti.SluzbenaProstorija> svePros = from o in s.Query<NarodnaSkupstina.Entiteti.SluzbenaProstorija>()
                                                                                    where o.ProstorijaPoslanickeGrupe.Id == id
                                                                                    select o;
                foreach(NarodnaSkupstina.Entiteti.SluzbenaProstorija r in svePros)
                {
                    prost.Add(new SluzbenaProstorijaPregled(r.Id, r.BrojProstorije, r.Sprat));
                }
                s.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return prost;
        }
        public static SluzbenaProstorijaBasic vratiSluzbenaProstorija(int id)
        {
            SluzbenaProstorijaBasic o = new SluzbenaProstorijaBasic();
            try
            {
                ISession s = DataLayer.GetSession();

                SluzbenaProstorija prostorija = s.Load<SluzbenaProstorija>(id);

                o = new SluzbenaProstorijaBasic(prostorija.Id, prostorija.BrojProstorije, prostorija.Sprat);





                s.Close();

            }
            catch (Exception ec)
            {
                //handle exceptions
            }

            return o;

        }
        public static List<SluzbenaProstorijaPregled> vratiSveSluzbeneProstorije()
        {
            List<SluzbenaProstorijaPregled> prostorije = new List<SluzbenaProstorijaPregled>();
            try
            {
                ISession s = DataLayer.GetSession();
                IEnumerable<NarodnaSkupstina.Entiteti.SluzbenaProstorija> sveProstorije = from o in s.Query<NarodnaSkupstina.Entiteti.SluzbenaProstorija>()
                                                                                          select o;
                foreach (NarodnaSkupstina.Entiteti.SluzbenaProstorija p in sveProstorije)
                {
                    prostorije.Add(new SluzbenaProstorijaPregled(p.Id, p.BrojProstorije, p.Sprat));
                }
                s.Close();
            }
            catch(Exception ec)
            {
                Console.WriteLine(ec.ToString());
            }
            return prostorije;
        }
        public static SluzbenaProstorijaBasic azurirajSluzbenuProstoriju(SluzbenaProstorijaBasic r)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                NarodnaSkupstina.Entiteti.SluzbenaProstorija o = s.Load<NarodnaSkupstina.Entiteti.SluzbenaProstorija>(r.Id);
                o.BrojProstorije = r.BrojProstorije;
                o.Sprat = r.Sprat;
                s.Update(o);
                s.Flush();
                s.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return r;
        }

        public static List<SluzbenaProstorijaPregled> vratiSluzbenaProstorijaPGrupa(int PGrupaID)
        {
            List<SluzbenaProstorijaPregled> odInfos = new List<SluzbenaProstorijaPregled>();
            try
            {
                ISession s = DataLayer.GetSession();

                IEnumerable<SluzbenaProstorija> prosto = from o in s.Query<SluzbenaProstorija>()
                                                          /*where o.ProstorijaPoslanickeGrupe.Id == PGrupaID*/
                                                         select o;

                foreach (SluzbenaProstorija o in prosto)
                {
                    odInfos.Add(new SluzbenaProstorijaPregled(o.Id, o.BrojProstorije, o.Sprat));
                }

                s.Close();

            }
            catch (Exception ec)
            {
                //handle exceptions
            }

            return odInfos;
        }
        public static void izmeniSluzbenaProstorija(SluzbenaProstorijaBasic sprost)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.SluzbenaProstorija o = s.Load<SluzbenaProstorija>(sprost.Id);

                o.BrojProstorije = sprost.BrojProstorije;
                o.Sprat = sprost.Sprat;
                



                s.SaveOrUpdate(o);

                s.Flush();

                s.Close();
            }
            catch (Exception ec)
            {
                //handle exceptions
            }
        }

        public static void dodajSluzbenuProstoriju(SluzbenaProstorijaBasic sluzb)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.SluzbenaProstorija sp = new NarodnaSkupstina.Entiteti.SluzbenaProstorija();
                sp.BrojProstorije = sluzb.BrojProstorije;
                sp.Sprat = sluzb.Sprat;

                s.SaveOrUpdate(sp);

                s.Flush();

                s.Close();

            }
            catch (Exception ec)
            {
                Console.WriteLine(ec.Message);
            }
        }

        #endregion

        #region PoslanickaGrupa

        public static List<PoslanickaPregled> vratiSvePGrupa()
        {
            List<PoslanickaPregled> grupe = new List<PoslanickaPregled>();
            try
            {
                ISession s = DataLayer.GetSession();

                IEnumerable<NarodnaSkupstina.Entiteti.PoslanickaGrupa> svePGrupe = from o in s.Query<NarodnaSkupstina.Entiteti.PoslanickaGrupa>()
                                                                                   select o;
                foreach (NarodnaSkupstina.Entiteti.PoslanickaGrupa p in svePGrupe)
                {
                    grupe.Add(new PoslanickaPregled
                        (p.Id, p.Naziv));
                }
                s.Close();
            }
            catch(Exception ec)
            {
                Console.WriteLine(ec.Message);
            }
            return grupe;
        }

        public static void dodajPoslanickuGrupu(PoslanickaBasic p)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.PoslanickaGrupa o = new NarodnaSkupstina.Entiteti.PoslanickaGrupa();
                
                o.Id = p.Id;

                s.SaveOrUpdate(o);
                s.Flush();
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec.Message);
            }
        }

        public static PoslanickaBasic azurirajPGrupu(PoslanickaBasic p)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.PoslanickaGrupa o = s.Load<NarodnaSkupstina.Entiteti.PoslanickaGrupa>(p.Naziv);
                o.Id = p.Id;

                s.Update(o);
                s.Flush();

                s.Close();
            }
            catch (Exception ec )
            {
                Console.WriteLine(ec.Message);
            }
            return p; //msm da ovde ide o
        }

        public static PoslanickaBasic vratiPGrupu(string id)
        {
            PoslanickaBasic pb = new PoslanickaBasic();
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.PoslanickaGrupa o = s.Load<NarodnaSkupstina.Entiteti.PoslanickaGrupa>(id);
                pb = new PoslanickaBasic(o.Id, o.Naziv);
                s.Close();
            }
            catch(Exception ec )
            {
                Console.WriteLine(ec.Message);
            }
            return pb;
        }
        public static PoslanickaBasic vratiNazivPGrupu(string naziv)
        {
            PoslanickaBasic pb = new PoslanickaBasic();
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.PoslanickaGrupa o = s.Load<NarodnaSkupstina.Entiteti.PoslanickaGrupa>(naziv);
                pb = new PoslanickaBasic(o.Id, o.Naziv);
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec.Message);
            }
            return pb;
        }
        public static PoslanickaBasic vratiPGrupuNaziv(string naziv)
        {
            PoslanickaBasic pb = new PoslanickaBasic();
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.PoslanickaGrupa o = s.Load<NarodnaSkupstina.Entiteti.PoslanickaGrupa>(naziv);
                pb = new PoslanickaBasic(o.Id, o.Naziv);
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec.Message);
            }
            return pb;
        }

        public static void obrisiPgrupu(string id)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.PoslanickaGrupa o = s.Load<NarodnaSkupstina.Entiteti.PoslanickaGrupa>(id);
                s.Delete(o);
                s.Flush();
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec.Message);
            }
        }


        #endregion

        #region RadnoTelo

        public static RadnoTeloBasic vratiRadnoTelo(string gg)
        {
            RadnoTeloBasic rd = new RadnoTeloBasic();
            try
            {
                ISession s = DataLayer.GetSession();
                NarodnaSkupstina.Entiteti.RadnoTelo r = s.Load<NarodnaSkupstina.Entiteti.RadnoTelo>(gg);
                rd = new RadnoTeloBasic(r.Tip,r.Id);

                s.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine (e.Message);
            }
            return rd;
        }

        #region AnketniOdbor

        //treba setup da odradimo tako da kad
        //se prikazuju narodniposlanici koji su u tom
        //radnom telu

        /* public static List<AnketniOdboriPregled> vratiSvePoslanikeAO()
     {
         List<AnketniOdboriPregled> ao = new List<AnketniOdboriPregled>();
         try
         {
             ISession s = DataLayer.GetSession();
             IEnumerable<NarodnaSkupstina.Entiteti.AnketniOdbori> sve = from o in s.Query<NarodnaSkupstina.Entiteti.AnketniOdbori>()
                                                                        where o.Clanovi.tip
                                                                        select o;
         }
     }*/

        #endregion

        #endregion

        #region BioPrisutan

        public static List<BioPrisutanBasic> vratiPrisutnost(int identif, int idSed)
        {
            List<BioPrisutanBasic> prisustvuje = new List<BioPrisutanBasic>(); 
            try {
                ISession s = DataLayer.GetSession();
                IEnumerable<BioPrisutan> b = from o in s.Query<BioPrisutan>()
                                             where o.Id.NPPrisutan.Id == identif
                                             //ovde moze bude identif umesto id vidi
                                             where o.Id.SednicaZasedanja.Id == idSed
                                             select o;
                foreach(BioPrisutan r in b)
                {
                    BioPrisutanIdBasic id = new BioPrisutanIdBasic();
                    id.NPBP = DTOManager.vratiNarodnog(r.Id.NPPrisutan.Id);
                    id.PrisutanSednica = DTOManager.vratiSednicu(r.Id.SednicaZasedanja.Id);
                    prisustvuje.Add(new BioPrisutanBasic(id, r.DatumOd, r.DatumDo));
                }
                s.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine (e.Message);
            }
            return prisustvuje;
        }

        public static void izmeniBioPrisutan(BioPrisutanBasic prisut)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                BioPrisutanId id = new BioPrisutanId();
                id.NPPrisutan = s.Load<NarodniPoslanik>(prisut.Id.NPBP.Identif);//ista prica za identif/id
                id.SednicaZasedanja = s.Load<NarodnaSkupstina.Entiteti.Sednica>(prisut.Id.PrisutanSednica.Id);
                NarodnaSkupstina.Entiteti.BioPrisutan o = s.Load<BioPrisutan>(id);

                o.DatumDo = prisut.Ddo;
                o.DatumOd = prisut.Dod;

                s.SaveOrUpdate(o);
                s.Flush();
                s.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion

        #region Sednica

        public static SednicaBasic vratiSednicu(int id)
        {
            SednicaBasic sb = new SednicaBasic();
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.Sednica o = s.Load<NarodnaSkupstina.Entiteti.Sednica>(id);
                sb = new SednicaBasic(o.Id, o.BrojZasedanja, o.BrojSaziva, o.DatumEND, o.DatumStart, o.TipFlag);
                s.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            return sb;
        }
        public static void obrisiSednicu(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                NarodnaSkupstina.Entiteti.Sednica o = s.Load<NarodnaSkupstina.Entiteti.Sednica>(id);
                s.Delete(o);
                s.Flush();
                s.Close();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }
        public static void dodajSednicu(SednicaBasic sb)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                NarodnaSkupstina.Entiteti.Sednica o = new NarodnaSkupstina.Entiteti.Sednica();
                o.BrojZasedanja = sb.BrZ;
                o.BrojSaziva = sb.BrS;
                o.DatumEND = sb.DEND;
                o.DatumStart = sb.DSTR;
                o.TipFlag = sb.TF;

                s.SaveOrUpdate(o);
                s.Flush(); s.Close();
            }
            catch(Exception e) { Console.WriteLine(e.Message); }
        }
        public static List<SednicaPregled> vratiSveSednice()
        {
            List<SednicaPregled> sednice = new List<SednicaPregled>();
            try
            {
                ISession s = DataLayer.GetSession();
                IEnumerable<NarodnaSkupstina.Entiteti.Sednica> sveSednice = from o in s.Query<NarodnaSkupstina.Entiteti.Sednica>()
                                                                            select o;
                foreach (NarodnaSkupstina.Entiteti.Sednica p in sveSednice)
                {
                    sednice.Add(new SednicaPregled
                        (p.Id, p.BrojZasedanja, p.BrojSaziva, p.DatumEND, p.DatumStart, p.TipFlag));
                }
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec.Message);
            }
            return sednice;
        }

        public static SednicaBasic azurirajSednicu(SednicaBasic sb)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                NarodnaSkupstina.Entiteti.Sednica o = s.Load<NarodnaSkupstina.Entiteti.Sednica>(sb.Id);
                o.BrojZasedanja = sb.BrZ;
                o.BrojSaziva = sb.BrS;
                o.DatumEND = sb.DEND;
                o.DatumStart = sb.DSTR;
                o.TipFlag = sb.TF;

                s.Update(o);
                s.Flush();
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine (ec.Message);
            }
            return sb;
        }
        #endregion

        #region RadniDan

        public static List<RadniDanBasic> vratiSveRadneDane()
        {
            List<RadniDanBasic> dani = new List<RadniDanBasic> ();
            try
            {
                ISession s = DataLayer.GetSession();
                IEnumerable<NarodnaSkupstina.Entiteti.RadniDan> sviDani = from o in s.Query<NarodnaSkupstina.Entiteti.RadniDan>()
                                                                          select o;
                foreach(NarodnaSkupstina.Entiteti.RadniDan rd in sviDani)
                {
                    dani.Add(new RadniDanBasic(rd.Id, rd.BrojP
                        , rd.VremeP, rd.VremeK));
                }
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine (ec.Message);
            }
            return dani;
        }

        public static List<RadniDanPregled> vratiSveRDaneSednice(int id)
        {
            List<RadniDanPregled> dani = new List<RadniDanPregled>();
            try
            {
                ISession s = DataLayer.GetSession();
                IEnumerable<NarodnaSkupstina.Entiteti.RadniDan> sviDani = from o in s.Query<NarodnaSkupstina.Entiteti.RadniDan>()
                                                                          where o.Sedni.Id == id
                                                                          select o;
                foreach (NarodnaSkupstina.Entiteti.RadniDan r in sviDani)
                {
                    dani.Add(new RadniDanPregled(r.Id, r.BrojP, r.VremeP, r.VremeK));
                }
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine (ec.Message);
            }
            return dani;
        }

        public static void dodajRadniDan(RadniDanBasic r)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.RadniDan o = new NarodnaSkupstina.Entiteti.RadniDan();
                o.BrojP = r.BrP;
                o.VremeP = r.Poc;
                o.VremeK = r.Kraj;

                s.SaveOrUpdate(o);

                s.Flush();
                s.Close();
            }
            catch(Exception ec)
            {
                Console.WriteLine (ec.Message);
            }
        }
        public static RadniDanBasic azurirajRadniDan(RadniDanBasic r)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                NarodnaSkupstina.Entiteti.RadniDan o = s.Load<NarodnaSkupstina.Entiteti.RadniDan>(r.Id);

                o.BrojP = r.BrP;
                o.VremeP = r.Poc;
                o.VremeK = r.Kraj;
                s.Update(o);
                s.Flush();
                s.Close();
            }
            catch (Exception ec)
            {
                Console.WriteLine (ec.Message);
            }
            return r;
        }
        public static RadniDanBasic vratiRadniDan(int id)
        {
            RadniDanBasic rdb=new RadniDanBasic();
            try
            {
                ISession s = DataLayer.GetSession();
                NarodnaSkupstina.Entiteti.RadniDan rd = s.Load<NarodnaSkupstina.Entiteti.RadniDan>(id);
                rdb = new RadniDanBasic(rd.Id, rd.BrojP, rd.VremeP, rd.VremeK);
                s.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine (e.Message);
            }
            return rdb;
        }

        public static void obrisiRadniDan(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.RadniDan r = s.Load<NarodnaSkupstina.Entiteti.RadniDan>(id);
                s.Delete(r);
                s.Flush();
                s.Close();
            }
            catch (Exception e) { Console.WriteLine ( e.Message); }
        }

        #endregion

        #region PravniAkt


        #region PredlogBiraca
        public static void obrisiPravniAktBiraca(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                PredlozioBiraci akt = s.Load<PredlozioBiraci>(id);

                s.Delete(akt);
                s.Flush();



                s.Close();

            }
            catch (Exception ec)
            {
                //handle exceptions
            }
        }

        public static List<PravniAktPregled> vratiPAkt()
        {
            List<PravniAktPregled> ins = new List<PravniAktPregled>();
            try
            {
                ISession s = DataLayer.GetSession();

                IEnumerable<PravniAkt> akt = from o in s.Query<PravniAkt>()
                                                    select o;
                foreach(PravniAkt p in akt)
                {
                    ins.Add(new PravniAktPregled(p.Id, p.Tip,p.Naziv));
                }
                s.Close();
            }
            catch(Exception ec)
            {
                Console.WriteLine(ec.Message);
            }
            return ins;
        }
        public static void obrisiPAkt(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                PravniAkt odeljenje = s.Load<PravniAkt>(id);

                s.Delete(odeljenje);
                s.Flush();



                s.Close();

            }
            catch (Exception ec)
            {
            }
        }
        
            public static void sacuvajPredlogBiraca(PredlozioBiraciBasic bir)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.PredlozioBiraci o = new NarodnaSkupstina.Entiteti.PredlozioBiraci();

                o.Naziv = bir.Naziv;
                o.Tip = bir.Tip;
                o.brojBiraca = bir.broj;
                /*NarodnaSkupstina.Entiteti.Prodavnica p = s.Load<Prodavnica.Entiteti.Prodavnica>(odeljenje.Prodavnica.Id);
                o.PripadaProdavnici = p;*/


                s.Save(o);

                s.Flush();

                s.Close();
            }
            catch (Exception ec)
            {
                //handle exceptions
            }
        }

            public static void sacuvajPredlogPoslanika(PredlozioNarodniPoslanikBasic bir)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.PredlozioNarodniPoslanik o = new NarodnaSkupstina.Entiteti.PredlozioNarodniPoslanik();

                o.Naziv = bir.Naziv;
                o.Tip = bir.Tip;
                /*NarodnaSkupstina.Entiteti.Prodavnica p = s.Load<Prodavnica.Entiteti.Prodavnica>(odeljenje.Prodavnica.Id);
                o.PripadaProdavnici = p;*/


                s.Save(o);

                s.Flush();

                s.Close();
            }
            catch (Exception ec)
            {
                //handle exceptions
            }
        }

        public static PredlozioBiraciBasic vratiPB(int id)
        {
            PredlozioBiraciBasic o = new PredlozioBiraciBasic();
            try
            {
                ISession s = DataLayer.GetSession();

                PredlozioBiraci predlog = s.Load<PredlozioBiraci>(id);

                o.AktID = predlog.AktID;
                o.Naziv = predlog.Naziv;
                o.Tip = predlog.Tip;
                //o.broj=predlog.




                s.Close();

            }
            catch (Exception ec)
            {
                //handle exceptions
            }

            return o;

        }

        public static void izmenPredlogBiraca(PredlozioBiraciBasic pred)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.PredlozioBiraci o = s.Load<NarodnaSkupstina.Entiteti.PredlozioBiraci>(pred.AktID);

                o.Naziv = pred.Naziv;
                o.Tip = pred.Tip;
                o.brojBiraca = pred.broj;




                s.SaveOrUpdate(o);

                s.Flush();

                s.Close();
            }
            catch (Exception ec)
            {
                //handle exceptions
            }
        }

        /*public static void sacuvajPredlogBiraca(PredlozioBiraciBasic pred)
        {
            try
            {
                ISession s = DataLayer.GetSession();

                NarodnaSkupstina.Entiteti.PredlozioBiraci o = new NarodnaSkupstina.Entiteti.PredlozioBiraci();

                o.Naziv = pred.Naziv;
                o.Tip = pred.Tip;
                o.brojBiraca = pred.broj;
                NarodnaSkupstina.Entiteti.PredlozioBiraci p = s.Load<NarodnaSkupstina.Entiteti.PredlozioBiraci>(*//*pred.Prodavnica.Id*//*);
                o.PripadaProdavnici = p;


                s.Save(o);

                s.Flush();

                s.Close();
            }
            catch (Exception ec)
            {
                //handle exceptions
            }
        }*/
        #endregion

        #endregion
    }
}
