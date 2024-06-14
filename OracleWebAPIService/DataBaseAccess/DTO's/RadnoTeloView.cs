using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class RadnoTeloView
    {
        public virtual int Id { get; protected set; }
        public virtual string Tip { get; set; }
        public virtual IList<NarodniPoslanikView> Clanovi { get; set; }
        public virtual SluzbenaProstorijaView SluzbenaProst { get; set; }

        //veze

        public RadnoTeloView()
        {
            Clanovi = new List<NarodniPoslanikView>();
        }
        internal RadnoTeloView(RadnoTelo? r) :this()
        {
            if (r != null)
            {
                Id = r.Id;
                Tip = r.Tip;
                SluzbenaProst = new SluzbenaProstorijaView(r.SluzbenaProst);
            }
        }
    }
}
