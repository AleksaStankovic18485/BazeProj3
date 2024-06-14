using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    public class NarodniPoslanik
    {
        public virtual int Id { get; protected set; }
        public virtual string JMBG { get; set; }
        public virtual int IdentifikacioniBroj { get; set; }
        public virtual string Ime { get; set; }
        public virtual string ImeJednogRoditelja { get; set; }
        public virtual string Prezime { get; set; }
        /*public virtual string IzbornaLista { get; set; } ??*/
        public virtual DateTime DatumRodjenja { get; set; }
        public virtual string MestoRodjenja { get; set; }
        public virtual string Grad { get; set; }
        public virtual string Adresa { get; set; }
        public virtual string BrojTelefona { get; set; }
        public virtual string BrojMobilnog { get; set; }
        public virtual bool StalniRadniOdnos { get;  set; }

        /*public virtual int IdPrisutnogGradjana { get; set; }
         ADVANCED*/
        public virtual PoslanickaGrupa PGrupa { get; set; }
        public virtual RadnoTelo RadnoT { get; set; }

        //veze

        public NarodniPoslanik()
        {
            //PGrupa=new List<PoslanickaGrupa>();
        }
    }
    public class StalniRadnik : NarodniPoslanik
    {
        public virtual IList<StalniRadniOdnos> Stalni { get; set; }

        public StalniRadnik() { Stalni = new List<StalniRadniOdnos>(); }
    }
}


