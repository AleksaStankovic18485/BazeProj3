using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public  class RadniDanView
    {
        public  int Id { get; protected set; }
        public  int BrojP { get; set; }
        public  DateTime VremeP { get; set; }
        public  DateTime VremeK { get; set; }

        public virtual Sednica Sedni { get; set; }

        //veze

        public RadniDanView()
        {
            /*PGrupe=new List<PoslanickaGrupa>();*/
        }
        internal RadniDanView(RadniDan? r) 
        {
            if (r != null)
            {
                Id = r.Id;
                BrojP = r.BrojP;
                VremeP = r.VremeP;
                VremeK = r.VremeK;
                Sedni = r.Sedni;
            }
        }
    }
}
