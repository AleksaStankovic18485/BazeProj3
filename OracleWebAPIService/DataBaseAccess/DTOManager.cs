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

        public async static Task<Result<List<NarodniPoslanikView>, ErrorMessage>> VratiSveNarodnePoslanikeAsync()
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

        public static Result<List<NarodniPoslanikView>, ErrorMessage> VratiSveNarodnePoslanikePgrupe(string naziv)
        {
            ISession? s = null;
            List<NarodniPoslanikView> poslanici = new();
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                IEnumerable<NarodniPoslanik> sviPoslanici = from o in s.Query<NarodniPoslanik>()
                                                                                      where o.PGrupa.Naziv== naziv
                                                                                      select o;
                foreach (NarodniPoslanik r in sviPoslanici)
                {
                    poslanici.Add(new NarodniPoslanikView(r));
                }
                
            }
            catch (Exception)
            { return "Nemoguće vratiti sve narodne poslanike koji su u PoslanickojGrupi sa zadatim ID-jem.".ToError(400); }
            finally{
                s?.Close();
                s?.Dispose();
            }
            return poslanici;
        }

        public static Result<List<NarodniPoslanikView>,ErrorMessage> VratiNPAnketni(string tip)
        {
            List<NarodniPoslanikView> poslanici = new();
            try
            {
                ISession s = DataLayer.GetSession();
                IEnumerable<NarodnaSkupstina.Entiteti.NarodniPoslanik> sviPoslanici = from o in s.Query<NarodnaSkupstina.Entiteti.NarodniPoslanik>()
                                                                                      where o.RadnoT.Tip == tip
                                                                                      select o;
                foreach (NarodnaSkupstina.Entiteti.NarodniPoslanik r in sviPoslanici)
                {
                    var radnoTel = VratiRadnoTelo(r.RadnoT.Tip);
                    //RadnoTeloBasic rt = DTOManager.vratiRadnoTelo(r.RadnoT.Tip);
                    var posG = VratiPGrupu(r.PGrupa.Naziv);
                    //PoslanickaBasic pgrupica = DTOManager.vratiPGrupu(r.PGrupa.Naziv);
                    poslanici.Add(new NarodniPoslanikView(r));
                }
                s.Close();
            }
            catch (Exception)
            {
                return "Nemoguće vratiti sve NarodnePoslanike Anketnog Odbora (svih odbora) sa zadatim ID-jem.".ToError(400);
            }
            return poslanici;
        }

        public static Result<bool, ErrorMessage> ObrisiPoslanika(int id)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                NarodniPoslanik n = s.Load<NarodniPoslanik>(id);
                s.Delete(n);
                s.Flush();

                s.Close();
            }
            catch(Exception )
            {
                return "Nemoguće obrisati radnika sa zadatim ID-jem.".ToError(400);
        }
            return true;
         }

        public async static Task<Result<bool, ErrorMessage>> DodajPoslanikaAsync(NarodniPoslanikView nb)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();

                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                NarodniPoslanik n = new()
                {
                    JMBG = nb.JMBG,
                    IdentifikacioniBroj = nb.IdentifikacioniBroj,
                    Ime = nb.Ime,
                    ImeJednogRoditelja = nb.ImeJednogRoditelja,
                    Prezime = nb.Prezime,
                    DatumRodjenja = nb.DatumRodjenja,
                    MestoRodjenja = nb.MestoRodjenja,
                    Grad = nb.Grad,
                    Adresa = nb.Adresa,
                    BrojTelefona = nb.BrojTelefona,
                    BrojMobilnog = nb.BrojMobilnog,
                    StalniRadniOdnos = nb.StalniRadniOdnos //ovo sad dodato
                                                           //protectrd je set pa zato nemoze vidi za to
                };
                await s.SaveOrUpdateAsync(n);

                await s.FlushAsync();
                
            }
            catch (Exception ec)
            {
                return "Nemoguće dodati NarodnogPoslanika.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return true;
        }

        public static Result<NarodniPoslanikView, ErrorMessage> AzurirajPoslanika(NarodniPoslanikView r)
        {
            try
            {
                ISession s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                NarodniPoslanik n = s.Load<NarodniPoslanik>(r.Id);
                n.JMBG = r.JMBG;
                n.IdentifikacioniBroj = r.IdentifikacioniBroj; //
                n.Ime = r.Ime;
                n.ImeJednogRoditelja = r.ImeJednogRoditelja;
                n.Prezime = r.Prezime;
                n.DatumRodjenja = r.DatumRodjenja;
                n.MestoRodjenja = r.MestoRodjenja;
                n.Grad = r.Grad;
                n.Adresa = r.Adresa;
                n.BrojTelefona = r.BrojTelefona;
                n.BrojMobilnog = r.BrojMobilnog;

                s.Update(n);
                s.Flush();
                s.Close();
            }
            catch(Exception e)
            {
                return "Nemoguće ažurirati NarodnogPoslanika.".ToError(400);
            }
            return r; //msm da ovde ide o al ono
        }

        public async static Task<Result<NarodniPoslanikView, ErrorMessage>> VratiNarodnog(int id)
        {
            ISession? s = null;
            NarodniPoslanikView nb = default!; //msm da ide mzd new ();
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                NarodniPoslanik n = await s.LoadAsync<NarodniPoslanik>(id);
                //RadnoTeloBasic rt = DTOManager.vratiRadnoTelo(n.RadnoT.Tip);
                //PoslanickaBasic pgrupica = DTOManager.vratiPGrupu(n.PGrupa.Naziv);
                nb = new NarodniPoslanikView(n /*pgrupica, rt*/);
                
            }
            catch (Exception) { return "Nemoguće vratiti radnika sa zadatim ID-jem.".ToError(400); }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
                return nb;
        }

        

        #endregion

            #region StalniRadnik

        public static Result<List<StalniRadnikView>,ErrorMessage> VratiSveStalneRadnike()
        {
            List<StalniRadnikView> sr = new ();
            try
            {
                ISession s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                IEnumerable<StalniRadnik> sviStalniRadnici = from o in s.Query<StalniRadnik>()
                                                                                       
                                                                                       select o;
                foreach (StalniRadnik p in sviStalniRadnici)
                {
                    sr.Add(new StalniRadnikView(p));
                }
                s.Close();
            }
            catch (Exception)
            {
                return "Nemoguće vratiti sve Stalne Radnike.".ToError(400);
            }
            return sr;
        }

        public static Result<StalniRadnikView, ErrorMessage> VratiStalnog(int id)
        {
            StalniRadnikView srb;
            try
            {
                ISession? s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                StalniRadnik sr = s.Load<StalniRadnik>(id);
                srb = new StalniRadnikView(sr);
                s.Close();
            }
            catch (Exception)
            {
                return "Nemoguće vratiti šefa.".ToError(400);
            }
            return srb;
        }

        public async static Task<Result<List<StalniRadniOdnosView>, ErrorMessage>> VratiSveStalneOdnosRadnike(/*int id*/) //mozda bude async mora se proba
        {
            //da msm da cu da promenim da ne bude asinhrona al aj ga ostavimo
            //dok ne moz se testira cu ga samo ostavimo u txt 
            ISession? s = null;

            List<StalniRadniOdnosView> stalniO = new ();
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                IEnumerable<StalniRadniOdnos> sviOdnos = from o in s.Query<StalniRadniOdnos>()
                                                                                   /*where o.StalniRadnik.StalniRadniOdnos==true */                                                                            
                                                                                   select o;
                

                foreach (StalniRadniOdnos sviO in sviOdnos)
                {
                    var st = VratiStalnog(sviO.StalniRadnik?.Id ?? -1); //proveriti ovo
                   // StalniRadnikBasic stalniRadnik = DTOManager.vratiStalnog(sviO.StalniRadnik.Id);
                   if(st.IsError) //ovo mzd sklonimo al mora testiramo prvo
                    {
                        continue;
                    }
                    stalniO.Add(new StalniRadniOdnosView(sviO));
                }
                s.Close();
            }
            catch(Exception)
            {
                return "Nemoguće vratiti sve StalneRadnike sa zadatim ID-jem.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return stalniO;
        }

        public async static Task<Result<bool,ErrorMessage>>ObrisiStalnog(int id)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                StalniRadniOdnos r = await s.LoadAsync<StalniRadniOdnos>(id);

                await s.DeleteAsync(r);
                await s.FlushAsync();

            }
            catch (Exception)
            {
                return "Nemoguće obrisati Stalnog.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return true;
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
