
/*using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Windows.Forms;*/

namespace NarodnaSkupstina
{
    #region NarodniPoslanik
    public class NarodniPoslanikPregled
    {
        public int Id;
        public string JMBG;
        public int Identif;
        public string Ime;
        public string ImeRod;
        public string Prezime;
        public DateTime DatumR;
        public string MestoR;
        public string Grad;
        public string Adresa;
        public string BrojT;
        public string BrojM;
        public bool StalniR;

        public NarodniPoslanikPregled()
        {

        }
        public NarodniPoslanikPregled(int id, string jMBG, int identif, string ime, string imeRod, string prezime, DateTime datumR, string mestoR, string grad, string adresa, string brojT, string brojM, bool stalniR)
        {
            Id = id;
            JMBG = jMBG;
            Identif = identif;
            Ime = ime;
            ImeRod = imeRod;
            Prezime = prezime;
            DatumR = datumR;
            MestoR = mestoR;
            Grad = grad;
            Adresa = adresa;
            BrojT = brojT;
            BrojM = brojM;
            StalniR = stalniR;
        }
    }

    public class NarodniPoslanikBasic
    {
        public int Id;
        public string JMBG;
        public int Identif;
        public string Ime;
        public string ImeRod;
        public string Prezime;
        public DateTime DatumR;
        public string MestoR;
        public string Grad;
        public string Adresa;
        public string BrojT;
        public string BrojM;
        public bool StalniR;
        public PoslanickaBasic Pg;
        public RadnoTeloBasic Rd;
        //
        //liste
        //
        public NarodniPoslanikBasic()
        {
            //liste
        }
        public NarodniPoslanikBasic(int id, string jMBG, int identif, string ime, string imeRod, string prezime, DateTime datumR, string mestoR, string grad, string adresa, string brojT, string brojM, bool stalni, PoslanickaBasic pg, RadnoTeloBasic rd) : this()
        {
            Id = id;
            JMBG = jMBG;
            Identif = identif;
            Ime = ime;
            ImeRod = imeRod;
            Prezime = prezime;
            DatumR = datumR;
            MestoR = mestoR;
            Grad = grad;
            Adresa = adresa;
            BrojT = brojT;
            BrojM = brojM;
            StalniR = stalni;
            Pg = pg;
            Rd = rd;
        }
    }

    public class StalniRadnikPregled : NarodniPoslanikPregled
    {

    }
    public class StalniRadnikBasic : NarodniPoslanikBasic
    {
        public IList<StalniRadniOdnosBasic> stalniRa;

        public StalniRadnikBasic()
        {
            stalniRa = new List<StalniRadniOdnosBasic>();
        }
        public StalniRadnikBasic(int id, string jMBG, int identif, string ime, string imeRod, string prezime, DateTime datumR, string mestoR, string grad, string adresa, string brojT, string brojM, bool stalni)
        {
            Id = id;
            JMBG = jMBG;
            Identif = identif;
            Ime = ime;
            ImeRod = imeRod;
            Prezime = prezime;
            DatumR = datumR;
            MestoR = mestoR;
            Grad = grad;
            Adresa = adresa;
            BrojT = brojT;
            BrojM = brojM;
            StalniR = stalni;
        }
        public override string ToString()
        {
            return Ime + " " + ImeRod + " " + Prezime;
        }
    }

    #endregion

    #region StalniRadniOdnos

    public class StalniRadniOdnosPregled
    {
        public int Id;
        public int BrK;
        public int God;
        public int Mes;
        public int Dan;
        public string ImeF;
        public StalniRadnikBasic SR;

        public StalniRadniOdnosPregled() {
        }

        public StalniRadniOdnosPregled(int id, int brK, int god, int mes, int dan, string imeF, StalniRadnikBasic sR)
        {
            Id = id;
            BrK = brK;
            God = god;
            Mes = mes;
            Dan = dan;
            ImeF = imeF;
            SR = sR;
        }
    }
    public class StalniRadniOdnosBasic
    {
        public int Id;
        public int BrK;
        public int God;
        public int Mes;
        public int Dan;
        public string ImeF;
        public StalniRadnikBasic SR;

        public StalniRadniOdnosBasic() { }
        public StalniRadniOdnosBasic(int id, int brK, int god, int mes, int dan, string imeF, StalniRadnikBasic sR)
        {
            this.Id = id;
            this.BrK = brK;
            this.God = god;
            this.Mes = mes;
            this.Dan = dan;
            this.ImeF = imeF;
            this.SR = sR;
        }
    }

    #endregion

    #region SluzbenaProstorija

    public class SluzbenaProstorijaPregled
    {
        public int Id;
        public int BrojProstorije;
        public int Sprat;

        public SluzbenaProstorijaPregled() { }
        public SluzbenaProstorijaPregled(int id, int brojProstorije, int sprat)
        {
            Id = id;
            BrojProstorije = brojProstorije;
            Sprat = sprat;
        }
    }
    public class SluzbenaProstorijaBasic
    {
        public int Id;
        public int BrojProstorije;
        public int Sprat;
        /*      public int Identif;*/
        //public PoslanickaGrupaBasic PGrupa;

        public SluzbenaProstorijaBasic(int id, int brp, int sprat/*,int ident*//*, */)
        {
            Id = id;
            BrojProstorije = brp;
            Sprat = sprat;
            /*            Identif = ident;*/
        }
        public SluzbenaProstorijaBasic() { }
    }

    #endregion

    #region PoslanickaGrupa

    public class PoslanickaPregled
    {
        public int Id;
        public string Naziv;


        public PoslanickaPregled() { }
        public PoslanickaPregled(int id, string naziv)
        {
            Id = id;
            Naziv = naziv;

        }
    }

    public class PoslanickaBasic
    {
        public int Id;
        public string Naziv;
        //mzd ovde idu liste
        public virtual IList<SluzbenaProstorijaBasic> Prostorija { get; set; }
        public virtual IList<NarodniPoslanikBasic> Poslanici { get; set; }

        public PoslanickaBasic() {
            Poslanici = new List<NarodniPoslanikBasic>();
            Prostorija = new List<SluzbenaProstorijaBasic>();
        }
        public PoslanickaBasic(int id, string naziv) : this()
        {
            this.Id = id;
            this.Naziv = naziv;

        }
    }


    #endregion

    #region RadnoTelo

    public class RadnoTeloBasic {
        public int Id;
        public string Tip;
        public virtual SluzbenaProstorijaBasic Prostorija { get; set; }
        public RadnoTeloBasic( string tip, int id)
        {
            this.Tip = tip;
            this.Id = id;
            
        }
        public RadnoTeloBasic()
        {

        }
    }
    public class StalniOdboriBasic : RadnoTeloBasic
    {
        public StalniOdboriBasic()
        {

        }
        public StalniOdboriBasic(string tip, int Id) : base(tip, Id)
        {

        }
    }

    public class AnketniOdboriBasic : RadnoTeloBasic
    {
        public AnketniOdboriBasic()
        {

        }
        public AnketniOdboriBasic(string tip, int Id) : base(tip, Id)
        {

        }
    }
    public class PrivremeniOdboriBasic : RadnoTeloBasic
    {
        public PrivremeniOdboriBasic()
        {

        }
        public PrivremeniOdboriBasic(string tip, int Id) : base(tip, Id)
        {

        }
    }
    public class KomisijaBasic : RadnoTeloBasic
    {
        public KomisijaBasic()
        {

        }
        public KomisijaBasic(string tip, int Id) : base(tip, Id)
        {

        }
    }

    public class RadnoTeloPregled
    {
        public int Id { get; set; }

        public RadnoTeloPregled(int id)
        {
            Id = id;
        }
        public RadnoTeloPregled() { }
    }
    public class StalniOdboriPregled : RadnoTeloPregled
    {
        public StalniOdboriPregled(int id)
        {

        }
    }
    public class AnketniOdboriPregled : RadnoTeloPregled
    {
        public AnketniOdboriPregled(int id)
        {

        }
    }
    public class PrivremeniOdboriPregled : RadnoTeloPregled
    {
        public PrivremeniOdboriPregled(int id)
        {

        }
    }
    public class KomisijePregled : RadnoTeloPregled
    {
        public KomisijePregled(int id)
        {

        }
    }




    #endregion

    #region BioPrisutan

   /* public class BioPrisutanPregled
    {
        public BioPrisutanId Id;
        public DateTime Dod;
        public DateTime? Ddo;

        public BioPrisutanPregled()
        {

        }
        public BioPrisutanPregled(BioPrisutanId id, DateTime dod, DateTime? ddo)
        {
            Id = id;
            Dod = dod;
            Ddo = ddo;
        }
    }*/

    public class BioPrisutanBasic
    {
        public BioPrisutanIdBasic Id;
        public DateTime Dod;
        public DateTime? Ddo;

        public BioPrisutanBasic()
        {

        }
        public BioPrisutanBasic(BioPrisutanIdBasic id, DateTime dod, DateTime? ddo)
        {
            Id = id;
            Dod = dod;
            Ddo = ddo;
        }
    }
    public class BioPrisutanIdBasic
    {
        public NarodniPoslanikBasic NPBP { get; set; }
        public SednicaBasic PrisutanSednica { get; set; }
        public BioPrisutanIdBasic() { }
    }

    #endregion

    #region Sednica

    public class SednicaPregled
    {
        public int Id;
        public int BrZ;
        public int BrS;
        public DateTime DEND;
        public DateTime DSTR;
        public bool TF;

        public SednicaPregled()
        {

        }
        public SednicaPregled(int id, int brZ, int brS, DateTime dEND, DateTime dSTR, bool tF)
        {
            Id = id;
            BrZ = brZ;
            BrS = brS;
            DEND = dEND;
            DSTR = dSTR;
            TF = tF;
        }
    }
    public class SednicaBasic
    {
        public int Id;
        public int BrZ;
        public int BrS;
        public DateTime DEND;
        public DateTime DSTR;
        public bool TF; 

       // public virtual IList<NarodniPoslanik> Poslanici { get; set; }
        public virtual IList<RadniDanBasic> RD { get; set; }
        public SednicaBasic() {
       //     Poslanici=new List<NarodniPoslanik>();
            RD=new List<RadniDanBasic>();
        }
        public SednicaBasic(int id, int brZ, int brS, DateTime dEND, DateTime dSTR, bool tF) :this()
        {
            Id = id;
            BrZ = brZ;
            BrS = brS;
            DEND = dEND;
            DSTR = dSTR;
            TF = tF;
        }
    }


    #endregion

    #region RadniDani

    public class RadniDanPregled
    {
        public int Id;
        public int BrP;
        public DateTime Poc;
        public DateTime Kraj;

        public RadniDanPregled() { }
        public RadniDanPregled(int id, int brP, DateTime poc, DateTime kraj)
        {
            Id = id;
            BrP = brP;
            Poc = poc;
            Kraj = kraj;
        }
    }
    public class RadniDanBasic
    {
        public int Id;
        public int BrP;
        public DateTime Poc;
        public DateTime Kraj;

        public RadniDanBasic() { }
        public RadniDanBasic(int id, int brP, DateTime poc, DateTime kraj)
        {
            Id = id;
            BrP = brP;
            Poc = poc;
            Kraj = kraj;
        }
    }

    #endregion

    #region PraviAkt

    public class PravniAktBasic
    {
        public int AktID { get; set; }
        public string Naziv { get; set; }
        public string Tip { get; set; }
        public PravniAktBasic()
        {

        }
        public PravniAktBasic(int aktID, string naziv, string tip)
        {
            AktID = aktID;
            Naziv = naziv;
            Tip = tip;
        }
    }
    public class PredlozioNarodniPoslanikBasic : PravniAktBasic
    {
        public virtual IList<PredlozioNarodniPoslanikBasic> predlog {  get; set; }
        public PredlozioNarodniPoslanikBasic() {
            predlog= new List<PredlozioNarodniPoslanikBasic>();
        }
        public PredlozioNarodniPoslanikBasic(int aktID, string naz, string t) :base(aktID, naz, t)
        {

        }
    }
    public class PredlozioBiraciBasic : PravniAktBasic
    {
        public virtual int broj { get; set; }
        public PredlozioBiraciBasic()
        {

        }
        public PredlozioBiraciBasic(int aktId, string naz, string t, int b) : base(aktId, naz, t)
        {

        }
    }
    public class PravniAktPregled
    {
        public int AktID { get; set; }
        public string Naziv { get; set; }
        public string Tip { get; set; }
        public PravniAktPregled() { }
        public PravniAktPregled(int aktID, string naziv, string tip)
        {
            AktID = aktID;
            Naziv = naziv;
            Tip = tip;
        }
    }
    public class PredlozioNarodniPoslanikPregled : PravniAktPregled { 
        public PredlozioNarodniPoslanikPregled(int aid, string naz, string t) { }
    }
    public class PredlozioBiraci : PravniAktPregled
    {
        public PredlozioBiraci(int aid, string naz, string t, int broj) { }
    }
    #endregion
    internal class DTOs
    {
    }
}
