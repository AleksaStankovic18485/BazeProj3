using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    internal class NarodniPoslanik
    {
        internal protected virtual int Id { get; protected set; }
        internal protected virtual string JMBG { get; set; }
        internal protected virtual int IdentifikacioniBroj { get; set; }
        internal protected virtual string Ime { get; set; }
        internal protected virtual string ImeJednogRoditelja { get; set; }
        internal protected virtual string Prezime { get; set; }
        /*public virtual string IzbornaLista { get; set; } ??*/
        internal protected virtual DateTime DatumRodjenja { get; set; }
        internal protected virtual string MestoRodjenja { get; set; }
        internal protected virtual string Grad { get; set; }
        internal protected virtual string Adresa { get; set; }
        internal protected virtual string BrojTelefona { get; set; }
        internal protected virtual string BrojMobilnog { get; set; }
        internal protected virtual bool StalniRadniOdnos { get;  set; }

        /*public virtual int IdPrisutnogGradjana { get; set; }
         ADVANCED*/
        internal protected virtual PoslanickaGrupa PGrupa { get; set; }
        internal protected virtual RadnoTelo RadnoT { get; set; }

        //veze

        internal NarodniPoslanik()
        {
            //PGrupa=new List<PoslanickaGrupa>();
        }
    }
    internal class StalniRadnik : NarodniPoslanik
    {
        internal protected virtual IList<StalniRadniOdnos> Stalni { get; set; }

        internal StalniRadnik() { Stalni = new List<StalniRadniOdnos>(); }
    }
}


