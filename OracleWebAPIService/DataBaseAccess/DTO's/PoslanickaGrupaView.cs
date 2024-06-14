using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class PoslanickaGrupaView
    {
        public int Id { get; set; }
        public  string Naziv { get; set; }
        public virtual IList<NarodniPoslanikView>? Clanovi { get; set; }
        public  virtual IList<SluzbenaProstorijaView>? Prostorije { get; set; }

        public PoslanickaGrupaView()
        {
            Clanovi = new List<NarodniPoslanikView>();
            Prostorije = new List<SluzbenaProstorijaView>();
        }
        internal PoslanickaGrupaView(PoslanickaGrupa? p) :this()
        {
            if (p != null)
            {
                Id = p.Id;
                Naziv = p.Naziv;
            }
        }
    }
}
