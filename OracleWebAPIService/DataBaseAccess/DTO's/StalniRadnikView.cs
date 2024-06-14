using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class StalniRadnikView : NarodniPoslanikView
    {
        public IList<StalniRadniOdnosView> Stalni { get; set; }
        public StalniRadnikView() {
            Stalni = new List<StalniRadniOdnosView>();

        }
        internal StalniRadnikView(StalniRadnik? s) :this() {
            if (s != null)
            {
                Id = s.Id;
                JMBG = s.JMBG;
                IdentifikacioniBroj = s.IdentifikacioniBroj;
                Ime = s.Ime;
                ImeJednogRoditelja = s.ImeJednogRoditelja;
                Prezime = s.Prezime;
                DatumRodjenja = s.DatumRodjenja;
                MestoRodjenja = s.MestoRodjenja;
                Grad = s.Grad;
                Adresa = s.Adresa;
                BrojTelefona = s.BrojTelefona;
                BrojMobilnog = s.BrojMobilnog;
                StalniRadniOdnos = s.StalniRadniOdnos;
            }
        }
    }
}
