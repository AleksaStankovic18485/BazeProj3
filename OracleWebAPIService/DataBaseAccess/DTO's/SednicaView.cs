using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class SednicaView
    {
        public  int Id { get; protected set; }
        public  int BrojZasedanja { get; set; }
        public  int BrojSaziva { get; set; }
        public  DateTime DatumEnd{ get; set; }
        public  DateTime  DatumStart { get; set; }
        public  bool TipFlag { get; set; }

        public virtual IList<BioPrisutanView> PrustniSednici { get; set; }
        public virtual IList<NarodniPoslanikView> Poslanici { get; set; }
        public virtual IList<RadniDanView> RDani { get; set; }

        public SednicaView()
        {
            Poslanici = new List<NarodniPoslanikView>();
            PrustniSednici = new List<BioPrisutanView>();
            RDani = new List<RadniDanView>();
        }

        internal SednicaView(Sednica? p) : this()
        {
            if (p!=null)
            {
                Id = p.Id;
                BrojZasedanja=p.BrojZasedanja;
                BrojSaziva=p.BrojSaziva;
                DatumEnd=p.DatumEND;
                DatumStart = p.DatumStart;
                TipFlag = p.TipFlag;
            }
        }
    }
}
