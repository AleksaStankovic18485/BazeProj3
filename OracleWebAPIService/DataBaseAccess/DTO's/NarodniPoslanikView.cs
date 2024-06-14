using NarodnaSkupstina.Entiteti;
/*using DataBaseAccess.DTO_s;*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class NarodniPoslanikView
    {
        public  int Id { get;  set; }
        public  string JMBG { get; set; }
        public  int IdentifikacioniBroj { get; set; }
        public  string Ime { get; set; }
        public  string ImeJednogRoditelja { get; set; }
        public  string Prezime { get; set; }
        public  DateTime DatumRodjenja { get; set; }
        public string MestoRodjenja { get; set; }
        public  string Grad { get; set; }
        public string Adresa { get; set; }
        public string BrojTelefona { get; set; }
        public  string BrojMobilnog { get; set; }
        public  bool StalniRadniOdnos { get; set; }

        public  PoslanickaGrupaView? PGrupa { get; set; }
        public  RadnoTeloView? RadnoT { get; set; }

        //veze

        public NarodniPoslanikView()
        {
        }
        internal NarodniPoslanikView(NarodniPoslanik? n) :this()
        {
            if (n != null) {
                Id = n.Id;
                JMBG = n.JMBG;
                IdentifikacioniBroj = n.IdentifikacioniBroj;
                Ime = n.Ime;
                ImeJednogRoditelja = n.ImeJednogRoditelja;
                Prezime = n.Prezime;
                DatumRodjenja = n.DatumRodjenja;
                MestoRodjenja=n.MestoRodjenja;
                Grad=n.Grad;
                Adresa=n.Adresa;
                BrojTelefona=n.BrojTelefona;
                BrojMobilnog = n.BrojMobilnog;
                StalniRadniOdnos = n.StalniRadniOdnos;
                PGrupa = new PoslanickaGrupaView(n.PGrupa);
                RadnoT=new RadnoTeloView(n.RadnoT);
                    }
        }
    }
}
