using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    internal class PoslanickaGrupa
    {
        internal protected virtual int Id { get; set; }
        internal protected virtual string Naziv { get; set; }

        /*public virtual string Predsednik { get; set; }
        public virtual string Zamenik { get; set; }*/
        /*public virtual int SluzbenaProstorija { get; set; }*/

        //veze
        //public virtual IList<sluzbenaProstorija> Prostorije { get; set; }
        internal protected virtual IList<NarodniPoslanik>? Clanovi { get; set; }
        internal protected virtual IList<SluzbenaProstorija>? Prostorije { get; set; }
        //
        internal PoslanickaGrupa()
        {
            Clanovi = new List<NarodniPoslanik>();
            Prostorije = new List<SluzbenaProstorija>();
        }

    }
}
