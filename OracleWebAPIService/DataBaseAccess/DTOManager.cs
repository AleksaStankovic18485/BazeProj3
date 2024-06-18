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

namespace DataBaseAccess
{


    public static class DTOManager
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

                IEnumerable<NarodniPoslanik> sviPoslanici = from o in await s.QueryOver<NarodniPoslanik>().ListAsync()
                                                                                      select o;
                foreach(NarodniPoslanik p in sviPoslanici)
                {
                    var posla = new NarodniPoslanikView(p);
             
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
        public async static Task<Result<NarodniPoslanikView, ErrorMessage>> VratiNarodnogAsync(int id)
        {
            ISession? s = null;

            NarodniPoslanikView radnikView = default!;

            try
            {
                s = DataLayer.GetSession();

                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                NarodniPoslanik r = await s.LoadAsync<NarodniPoslanik>(id);
                radnikView = new NarodniPoslanikView(r);
            }
            catch (Exception)
            {
                return "Nemoguće vratiti NarodnogPO sa zadatim ID-jem.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }

            return radnikView;
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
                IEnumerable<NarodniPoslanik> sviPoslanici = from o in s.Query<NarodnaSkupstina.Entiteti.NarodniPoslanik>()
                                                                                      where o.RadnoT.Tip == tip
                                                                                      select o;
                foreach (NarodniPoslanik r in sviPoslanici)
                {
                    var radnoTel = VratiRadnoTelo(r.RadnoT.Tip);
                    //RadnoTeloBasic rt = DTOManager.vratiRadnoTelo(r.RadnoT.Tip);
                    var posG = VratiPGrupuAsync(r.PGrupa.Naziv);
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

        public async static Task<Result<bool, ErrorMessage>> ObrisiNarodnogAsync(int id)
        {
            ISession? s = null;

            try
            {
                s = DataLayer.GetSession();

                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                NarodniPoslanik r = await s.LoadAsync<NarodniPoslanik>(id); 

                await s.DeleteAsync(r);
                await s.FlushAsync();
            }
            catch (Exception)
            {
                return "Nemoguće obrisati radnika sa zadatim ID-jem.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
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
            catch (Exception)
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
            catch(Exception)
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
        public async static Task<Result<bool, ErrorMessage>> DodajPrisutnostAsync(BioPrisutanView radi)
        {
            ISession? s = null;

            try
            {
                s = DataLayer.GetSession();

                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                BioPrisutan r = new()
                {
                    Id = new BioPrisutanId
                    {
                        NPPrisutan = await s.LoadAsync<NarodniPoslanik>(radi.Id?.NPPrisutan?.Id),
                        SednicaZasedanja = await s.LoadAsync<Sednica>(radi.Id?.SednicaZasedanja?.Id)
                    },
                   
                    DatumOd = radi.DatumOd,
                    DatumDo = radi.DatumDo
                };

                await s.SaveOrUpdateAsync(r);
                await s.FlushAsync();
            }
            catch (Exception)
            {
                return "Nemoguće dodati Prisutnost.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }

            return true;
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
                return "StalniRadnik sa tim ID-em ne postoji.".ToError(400);
            }
            return srb;
        }

        ///////////////////////////////////////////////////////////////////////////
        public async static Task<Result<List<StalniRadniOdnosView>, ErrorMessage>> VratiSveStalneOdnosRadnike(int id) //mozda bude async mora se proba
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
                                                                                   where o.StalniRadnik!.StalniRadniOdnos==true                                                                           
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
        ////////////////////////////////////////////////////////////////////////////////////
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

        public async static Task<Result<bool,ErrorMessage>> ObrisiSluzbenaProstorija(int id)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                SluzbenaProstorija prost = await s.LoadAsync<SluzbenaProstorija>(id);
                //mzd nije samo obicna prostortija nego basic il tk nes


               await s.DeleteAsync(prost);
               await s.FlushAsync();



                

            }
            catch (Exception)
            {
                return "Nemoguće obrisati sluzbenu prostoriju.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return true;

        }
        public static Result<List<SluzbenaProstorijaView>, ErrorMessage> VratiPGrupaProstorije(int id)
        {
            ISession? s = null;
            List<SluzbenaProstorijaView> prost = new List<SluzbenaProstorijaView>();
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                IEnumerable<SluzbenaProstorija> svePros = from o in s.Query<SluzbenaProstorija>()
                                                                                    where o.ProstorijaPoslanickeGrupe.Id == id
                                                                                    select o;
                foreach(SluzbenaProstorija r in svePros)
                {
                    prost.Add(new SluzbenaProstorijaView(r));
                }
                
            }
            catch (Exception)
            {
                return "Nemoguće vratiti sve sluzbene prostorije koje su zauzete od strane PGrupe sa zadatim ID-jem.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return prost;
        }
        public async static Task<Result<SluzbenaProstorijaView,ErrorMessage>> VratiSluzbenaProstorijaAsync(int id)
        {
            ISession? s = null;
            SluzbenaProstorijaView o = default!;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                SluzbenaProstorija prostorija = await s.LoadAsync<SluzbenaProstorija>(id);

                o = new SluzbenaProstorijaView(prostorija);

            }
            catch (Exception)
            {
                return "Nemoguće vratiti sluzbenu prostoriju sa zadatim ID-jem.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }

            return o;

        }
        public async static Task<Result<List<SluzbenaProstorijaView>,ErrorMessage>> VratiSveSluzbeneProstorije()
        {
            ISession? s = null;
            List<SluzbenaProstorijaView> prostorije = new();
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                IEnumerable<SluzbenaProstorija> sveProstorije = from o in await s.QueryOver<SluzbenaProstorija>().ListAsync()
                                                                                          select o;
                foreach (SluzbenaProstorija p in sveProstorije)
                {
                    prostorije.Add(new SluzbenaProstorijaView(p));
                }
            }
            catch(Exception)
            {
                return "Nemoguće vratiti sve sluzbene prostorije.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return prostorije;
        }
        public static async Task<Result<SluzbenaProstorijaView,ErrorMessage>> AzurirajSluzbenuProstorijuAsync(SluzbenaProstorijaView r)
        {
            ISession? s = null;
            try
            {
                 s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                SluzbenaProstorija o = s.Load<SluzbenaProstorija>(r.Id);
                o.BrojProstorije = r.BrojProstorije;
                o.Sprat = r.Sprat;
                await s.UpdateAsync(o);
                await s.FlushAsync();
                
            }
            catch (Exception)
            {
                return "Nemoguće ažurirati sluzbenu prostoriju.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return r;
        }

        public static List<SluzbenaProstorijaView> vratiSluzbenaProstorijaPGrupa(int PGrupaID)
        {
            List<SluzbenaProstorijaView> odInfos = new List<SluzbenaProstorijaView>();
            try
            {
                ISession s = DataLayer.GetSession();

                IEnumerable<SluzbenaProstorija> prosto = from o in s.Query<SluzbenaProstorija>()
                                                          /*where o.ProstorijaPoslanickeGrupe.Id == PGrupaID*/
                                                         select o;

                foreach (SluzbenaProstorija o in prosto)
                {
                    odInfos.Add(new SluzbenaProstorijaView(o));
                }

                s.Close();

            }
            catch (Exception)
            {
                //handle exceptions
            }

            return odInfos;
        }
        public static Result<SluzbenaProstorijaView,ErrorMessage> IzmeniSluzbenaProstorija(SluzbenaProstorijaView sprost)
        {
            try
            {
                ISession? s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                SluzbenaProstorija o = s.Load<SluzbenaProstorija>(sprost.Id);

                o.BrojProstorije = sprost.BrojProstorije;
                o.Sprat = sprost.Sprat;
                



                s.Update(o);

                s.Flush();

                s.Close();
            }
            catch (Exception)
            {
                return "Nemoguće ažurirati sluzbenu prostoriju.".ToError(400);
            }
            return sprost;
        }

        public async static Task<Result<bool,ErrorMessage>>DodajSluzbenuProstoriju(SluzbenaProstorijaView sluzb)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                SluzbenaProstorija sp = new SluzbenaProstorija();
                sp.BrojProstorije = sluzb.BrojProstorije;
                sp.Sprat = sluzb.Sprat;

                await s.SaveOrUpdateAsync(sp);

                await s.FlushAsync();


            }
            catch (Exception)
            {
                return "Nemoguće dodati sluzbenu prostoriju.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return true;
        }

        #endregion

        #region PoslanickaGrupa

        public static Result<List<PoslanickaGrupaView>,ErrorMessage> VratiSvePGrupa()
        {
            ISession? s = null;
            List<PoslanickaGrupaView> grupe = new ();
            try
            {
                s = DataLayer.GetSession();

                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }


                IEnumerable<PoslanickaGrupa> svePGrupe = from o in s.Query<PoslanickaGrupa>()
                                                                                   select o;
                foreach (PoslanickaGrupa p in svePGrupe)
                {
                    grupe.Add(new PoslanickaGrupaView(p));
                }
            }
            catch (Exception)
            {
                return "Nemoguće vratiti sve PGrupe.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return grupe;
        }

        public async static Task<Result<bool,ErrorMessage>>DodajPoslanickuGrupuAsync(PoslanickaGrupaView p)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                PoslanickaGrupa o = new()
                {
                    Id = p.Id

                };
                //mzd treba stavimo ovde kolekciju za pgrupu
                

                await s.SaveOrUpdateAsync(o);
                await s.FlushAsync();
            }
            catch (Exception)
            {
                return GetError("Nemoguće dodati Pgrupu.", 404);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return true;
        }

        public async static Task<Result<PoslanickaGrupaView,ErrorMessage>> AzurirajPGrupuAsync(PoslanickaGrupaView p)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                PoslanickaGrupa o = s.Load<PoslanickaGrupa>(p.Naziv);
                o.Id = p.Id;

                await s.UpdateAsync(o);
                await s.FlushAsync();
            }
            catch (Exception)
            {
                return "Nemoguće ažurirati Pgrupu.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return p; //msm da ovde ide o
        }

        public async static Task<Result<PoslanickaGrupaView,ErrorMessage>> VratiPGrupuAsync(string id)
        {
            ISession? s = null;
            PoslanickaGrupaView pb = default!;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                PoslanickaGrupa o = await s.LoadAsync<PoslanickaGrupa>(id);
                pb = new PoslanickaGrupaView(o);
            }
            catch (Exception)
            {
                return "Nemoguće vratiti Pgrupu sa zadatim ID-jem.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return pb;
        }
        public async static Task<Result<PoslanickaGrupaView,ErrorMessage>> VratiNazivPGrupu(string naziv)
        {
            ISession? s = null;
            PoslanickaGrupaView pb = new();
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                PoslanickaGrupa o = await s.LoadAsync<PoslanickaGrupa>(naziv);
                pb = new PoslanickaGrupaView(o);
            }
            catch (Exception)
            {
                return "Nemoguće vratiti PGrupu sa zadatim Imenom".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return pb;
        }
        public async static Task<Result<PoslanickaGrupaView,ErrorMessage>> VratiPGrupuNaziv(string naziv)
        {
            ISession? s = null;
            PoslanickaGrupaView pb = new();
            try
            {
                s = DataLayer.GetSession();

                PoslanickaGrupa o = await s.LoadAsync<PoslanickaGrupa>(naziv);
                pb = new PoslanickaGrupaView(o);
            }
            catch (Exception)
            {
                return "Nemoguće vratiti PGrupu sa zadatim Imenom".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return pb;
        }

        public async static Task<Result<bool,ErrorMessage>>ObrisiPgrupuAsync(string id)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                PoslanickaGrupa o = s.Load<PoslanickaGrupa>(id);
              await  s.DeleteAsync(o);
              await  s.FlushAsync();
                s.Close();
            }
            catch (Exception)
            {
                return "Nemoguće obrisati PGrupu.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return true;
        }


        #endregion

        #region RadnoTelo

        public static async Task<Result<List<RadnoTeloView>, ErrorMessage>> VratiSvaRadnaTela()
        {
            List<RadnoTeloView> data = new();

            ISession? s = null;

            try
            {
                s = DataLayer.GetSession();

                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                data = (await s.QueryOver<RadnoTelo>().ListAsync())
                               .Select(p => new RadnoTeloView(p)).ToList();
            }
            catch (Exception)
            {
                return "Došlo je do greške prilikom prikupljanja informacija o odeljenjima.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }

            return data;
        }

        public async static Task<Result<RadnoTeloView,ErrorMessage>> VratiRadnoTelo(string gg)
        {
            ISession? s = null;
            RadnoTeloView rd = new ();
            try
            {
                s = DataLayer.GetSession();
                RadnoTelo r = await s.LoadAsync<RadnoTelo>(gg);
                rd = new RadnoTeloView(r);
            }
            catch (Exception)
            {
                return "Nemoguće vratiti RadnoTelo sa zadatim Tipom.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
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

        public static Result<List<BioPrisutanView>,ErrorMessage> VratiPrisutnost(int identif, int idSed)
        {
            List<BioPrisutanView> prisustvuje = new (); 
            try {
                ISession? s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                IEnumerable<BioPrisutan> b = from o in s.Query<BioPrisutan>()
                                             where o.Id.NPPrisutan.Id == identif
                                             //ovde moze bude identif umesto id vidi
                                             where o.Id.SednicaZasedanja.Id == idSed
                                             select o;
                foreach(BioPrisutan r in b)
                {
                   /* BioPrisutanIdView id = new ();
                    id.NPPrisutan = DTOManager.vratiNarodnog(r.Id.NPPrisutan.Id);
                    id.PrisutanSednica = DTOManager.vratiSednicu(r.Id.SednicaZasedanja.Id);*/
                    prisustvuje.Add(new BioPrisutanView(r));
                }
                s.Close();
            }
            catch (Exception)
            {
                return "Nemoguće vratiti prisutnost narodnog poslaniku u narodnoj skupstini.".ToError(400);
            }
            return prisustvuje;
        }

        public static Result<bool,ErrorMessage> IzmeniBioPrisutan(BioPrisutanView prisut)
        {
            try
            {
                ISession? s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                //BioPrisutanId id = new BioPrisutanId();
                BioPrisutanId id = new()
                {
                    SednicaZasedanja = s.Load<Sednica>(prisut.Id?.SednicaZasedanja?.Id),
                    NPPrisutan = s.Load<NarodniPoslanik>(prisut.Id?.NPPrisutan?.Id) //identif
                };
               
                BioPrisutan o = s.Load<BioPrisutan>(id);

                o.DatumDo = prisut.DatumDo;
                o.DatumOd = prisut.DatumOd;

                s.SaveOrUpdate(o);
                s.Flush();
                s.Close();
            }
            catch(Exception)
            {
                return "Nemoguće izmeniti prisutnost.".ToError(400);
            }
            return true;
        }

        #endregion

        #region Sednica

        public async static Task<Result<SednicaView,ErrorMessage>> VratiSednicuAsync(int id)
        {
            ISession? s = null;

            SednicaView sb = default!;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                Sednica o = await s.LoadAsync<Sednica>(id);
                sb = new SednicaView(o);
            }
            catch (Exception)
            {
                return "Nemoguće vratiti sednicu sa zadatim ID-jem.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return sb;
        }
        public async static Task<Result<bool, ErrorMessage>>ObrisiSednicuAsync(int id)
        {
            ISession? s = null;

            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                Sednica o = await s.LoadAsync<Sednica>(id);
                await s.DeleteAsync(o);
                await s.FlushAsync();
                
            }
            catch (Exception)
            {
                return "Nemoguće obrisati sednicu.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }

            return true;
        }
        public async static Task<Result<bool, ErrorMessage>>DodajSednicuAsync(SednicaView sb)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                Sednica o = new()
                {
                    BrojZasedanja = sb.BrojZasedanja,
                    BrojSaziva = sb.BrojSaziva,
                    DatumEND = sb.DatumEnd,
                    DatumStart = sb.DatumStart,
                    TipFlag = sb.TipFlag
            };
                

               await s.SaveOrUpdateAsync(o);
                await s.FlushAsync();
            }
            catch (Exception)
            {
                return GetError("Nemoguće dodati sednicu.", 404);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }

            return true;
        }
        public static Result<List<SednicaView>,ErrorMessage> VratiSveSednice()
        {
            ISession? s = null;
            List<SednicaView> sednice = new ();
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                IEnumerable<Sednica> sveSednice = from o in s.Query<Sednica>()
                                                                            select o;
                foreach (Sednica p in sveSednice)
                {
                    sednice.Add(new SednicaView(p));
                }
            }
            catch (Exception)
            {
                return "Nemoguće vratiti sve sednice.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return sednice;
        }

        public async static Task<Result<SednicaView, ErrorMessage>> AzurirajSednicuAsync(SednicaView sb)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                Sednica o = s.Load<Sednica>(sb.Id);
                o.BrojZasedanja = sb.BrojZasedanja;
                o.BrojSaziva = sb.BrojSaziva;
                o.DatumEND = sb.DatumEnd;
                o.DatumStart = sb.DatumStart;
                o.TipFlag = sb.TipFlag;

                await s.UpdateAsync(o);
                await s.FlushAsync();
            }
            catch (Exception)
            {
                return "Nemoguće ažurirati prodavnicu.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return sb;
        }
        #endregion

        #region RadniDan

        public async static Task<Result<List<RadniDanView>, ErrorMessage>> VratiSveRadneDaneAsync()
        {
            ISession? s = null;
            List<RadniDanView> dani = new ();
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                IEnumerable<RadniDan> sviDani = from o in await s.QueryOver<RadniDan>().ListAsync()
                                                                          select o;
                foreach(RadniDan rd in sviDani)
                {
                    dani.Add(new RadniDanView(rd));
                }
            }
            catch (Exception)
            {
                return "Nemoguće vratiti sve radnike.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return dani;
        }

        public async static Task<Result<List<RadniDanView>,ErrorMessage>> VratiSveRDaneSednice(int id)
        {
            ISession? s = null;
            List<RadniDanView> dani = new ();
            try
            {
                s = DataLayer.GetSession();
                IEnumerable<RadniDan> sviDani = from o in s.Query<RadniDan>()
                                                                          where o.Sedni.Id == id
                                                                          select o;
                foreach (RadniDan r in sviDani)
                {
                    var sed = await VratiSednicuAsync(r.Sedni?.Id ?? -1);
                    if(sed.IsError)
                    dani.Add(new RadniDanView(r));
                }
            }
            catch (Exception)
            {
                return "Nemoguće vratiti sve radnike koji rade u prodavnici sa zadatim ID-jem.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return dani;
        }

        public async static Task<Result<bool, ErrorMessage>>DodajRadniDanAsync(RadniDanView r)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                RadniDan o = new()
                {
                    BrojP = r.BrojP,
                VremeP = r.VremeP,
                VremeK = r.VremeK,
            };
               

                await s.SaveOrUpdateAsync(o);

                await s.FlushAsync();
                s.Close();
            }
            catch (Exception)
            {
                return "Nemoguće dodati radnika.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return true;
        }
        public async static Task<Result<RadniDanView, ErrorMessage>> AzurirajRadniDanAsync(RadniDanView r)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                RadniDan o = s.Load<RadniDan>(r.Id);

                o.BrojP = r.BrojP;
                o.VremeP = r.VremeP;
                o.VremeK = r.VremeK;
               await s.UpdateAsync(o);
                await s.FlushAsync();
                
            }
            catch (Exception)
            {
                return "Nemoguće ažurirati RadnogDana.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return r;
        }
        public async static Task<Result<RadniDanView, ErrorMessage>> VratiRadniDanAsync(int id)
        {
            ISession? s = null;
            RadniDanView rdb=default!;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                RadniDan rd = await s.LoadAsync<RadniDan>(id);
                rdb = new RadniDanView(rd);
               
            }
            catch (Exception)
            {
                return "Nemoguće vratiti RadniDan sa zadatim ID-jem.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return rdb;
        }

        public async static Task<Result<bool, ErrorMessage>>ObrisiRadniDan(int id)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                RadniDan r = s.Load<RadniDan>(id);
                await s.DeleteAsync(r);
                await s.FlushAsync();
            }
            catch (Exception) { return "Nemoguće obrisati RadniDan sa zadatim ID-jem.".ToError(400); }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return true;
        }

        #endregion

        #region PravniAkt


        #region PredlogBiraca
        public async static Task<Result<bool, ErrorMessage>>ObrisiPravniAktBiracaAsync(int id)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                PredlozioBiraci akt = await s.LoadAsync<PredlozioBiraci>(id);

                await s.DeleteAsync(akt);
                await s.FlushAsync();

            }
            catch (Exception)
            {
                return "Nemoguće obrisati Pravni Akt Biraca sa zadatim ID-jem.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return true;
        }

        public async static Task<Result<List<PravniAktView>,ErrorMessage>>VratiPAktAsync()
        {
            ISession? s = null;
            List<PravniAktView> ins = new ();
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                IEnumerable<PravniAkt> akt = from o in await s.Query<PravniAkt>().ToListAsync()
                                                    select o;
                foreach(PravniAkt p in akt)
                {
                    ins.Add(new PravniAktView(p));
                }
            }
            catch (Exception)
            {
                return "Nemoguće vratiti sve PravneAkte.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return ins;
        }
        public async static Task<Result<bool,ErrorMessage>>ObrisiPAktAsync(int id)
        {
            ISession? s = null;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                PravniAkt odeljenje = await s.LoadAsync<PravniAkt>(id);

                await s.DeleteAsync(odeljenje);
                await s.FlushAsync();




            }
            catch (Exception)
            {
                return "Greška prilikom brisanja PAkta.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }

            return true;
        }
        
            public async static Task<Result<int,ErrorMessage>>SacuvajPredlogBiraca(PredlozioBiraciView bir)
        {
            ISession? s = null;
            int id = default;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                PredlozioBiraci o = default!;

                o.Naziv = bir.Naziv;
                o.Tip = bir.Tip;
               /* o.brojBiraca = bir.broj;*/
                /*NarodnaSkupstina.Entiteti.Prodavnica p = s.Load<Prodavnica.Entiteti.Prodavnica>(odeljenje.Prodavnica.Id);
                o.PripadaProdavnici = p;*/


               await s.SaveAsync(o);

               await s.FlushAsync();

                id = o.Id;
            }
            catch (Exception)
            {
                return "Nemoguće sačuvati odeljenje bez prodavnice.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return id;
        }

            public async static Task<Result<int,ErrorMessage>>SacuvajPredlogPoslanika(PredlozioNarodniPoslanikView bir)
        {
            ISession? s = null;
            int id = default;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }

                PredlozioNarodniPoslanik o = new()
                {
                    Naziv = bir.Naziv,
                    Tip = bir.Tip
            };

                
                /*NarodnaSkupstina.Entiteti.Prodavnica p = s.Load<Prodavnica.Entiteti.Prodavnica>(odeljenje.Prodavnica.Id);
                o.PripadaProdavnici = p;*/


               await s.SaveAsync(o);

                await s.FlushAsync();

                id = o.Id;
            }
            catch (Exception)
            {
                return "Nemoguće sačuvati odeljenje bez prodavnice.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }
            return id;
        }

        public async static Task<Result<PravniAktView,ErrorMessage>>VratiPBAsync(int id)
        {
            ISession? s = null;
            PredlozioBiraciView o=default!;
            try
            {
                s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                PredlozioBiraci predlog = await s.LoadAsync<PredlozioBiraci>(id);
       
                /*o.Id = predlog.AktID;*/
                o.Naziv = predlog.Naziv;
                o.Tip = predlog.Tip;
                //o.broj=predlog.




                s.Close();

            }
            catch (Exception)
            {
                return "Nemoguće vratiti odeljenja.".ToError(400);
            }
            finally
            {
                s?.Close();
                s?.Dispose();
            }

            return o;

        }

        public static Result<PravniAktView,ErrorMessage>IzmenPredlogBiraca(PredlozioBiraciView pred)
        {
            try
            {
                ISession? s = DataLayer.GetSession();
                if (!(s?.IsConnected ?? false))
                {
                    return "Nemoguće otvoriti sesiju.".ToError(403);
                }
                PredlozioBiraci o = s.Load<PredlozioBiraci>(pred.Id);

                o.Naziv = pred.Naziv;
                o.Tip = pred.Tip;
                /*o.PredlozioBiraci = pred.PredlozioBiraci;*/




                s./*SaveOr*/Update(o);

                s.Flush();

                s.Close();
            }
            catch (Exception)
            {
                //handle exceptions
                //throw;
                return "Nemoguće ažurirati PredlogBiraca.".ToError(400);
            }

            return pred;
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
