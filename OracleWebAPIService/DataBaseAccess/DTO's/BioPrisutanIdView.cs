using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class BioPrisutanIdView
    {
        public NarodniPoslanikView? NPPrisutan { get; set; }
        public SednicaView? SednicaZasedanja { get; set; }
        public BioPrisutanIdView()
        {

        }

        internal BioPrisutanIdView(BioPrisutanId? b)
        {
            if(b!=null)
            {
                NPPrisutan = new NarodniPoslanikView(b.NPPrisutan);
                SednicaZasedanja=new SednicaView(b.SednicaZasedanja);
            }
        }
    }
}
