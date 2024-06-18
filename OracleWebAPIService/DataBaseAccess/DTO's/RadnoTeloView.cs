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
        public virtual int? Id { get;  set; }
        public virtual string? Tip { get; set; }
        public virtual int? Broj { get; set; }
        public virtual int? Sprat { get; set; }
        public virtual IList<NarodniPoslanikView>? Clanovi { get; set; }
        public SluzbenaProstorijaView? SluzbenaProst;

        //veze

        public RadnoTeloView()
        {
            Clanovi = new List<NarodniPoslanikView>();
        }
        internal RadnoTeloView(RadnoTelo? r) 
        {
            if (r != null)
            {
                Tip = r.Tip;
                //Id = r.Id;
                
                //Broj = r.SluzbenaProst.BrojProstorije;
                //Sprat=r.SluzbenaProst.Sprat;
                 
                SluzbenaProst = new SluzbenaProstorijaView(r.SluzbenaProst);
            }
        }
        internal RadnoTeloView(RadnoTelo? o, SluzbenaProstorija? s) : this(o)
        {
            SluzbenaProst = new SluzbenaProstorijaView(s);
        }
    }
}
