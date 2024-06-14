using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    public class PoslanickaGrupa
    {
        public virtual int Id { get; set; }
        public virtual string Naziv { get; set; }

        /*public virtual string Predsednik { get; set; }
        public virtual string Zamenik { get; set; }*/
        /*public virtual int SluzbenaProstorija { get; set; }*/

        //veze
        //public virtual IList<sluzbenaProstorija> Prostorije { get; set; }
        public virtual IList<NarodniPoslanik> Clanovi { get; set; }
        public virtual IList<SluzbenaProstorija> Prostorije { get; set; }
        //
        public PoslanickaGrupa()
        {
            Clanovi = new List<NarodniPoslanik>();
            Prostorije = new List<SluzbenaProstorija>();
        }

    }
}
