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
        public int BrojZasedanja { get; set; }
        public int BrojSaziva { get; set; }
        public DateTime DatumP { get; set; }
        public DateTime DatumEnd { get; set; }
        public bool TipS { get; set; }

        public  SednicaView? Sedni;

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
                BrojZasedanja = r.Sedni.BrojZasedanja;
                BrojSaziva=r.Sedni.BrojSaziva;
                DatumP = r.Sedni.DatumStart;
                DatumEnd = r.Sedni.DatumEND;
                TipS=r.Sedni.TipFlag;
            }
        }
    }
}
