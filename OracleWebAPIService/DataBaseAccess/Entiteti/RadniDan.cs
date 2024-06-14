using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    public class RadniDan
    {
        public virtual int Id { get; protected set; }
        public virtual int BrojP { get; set; }
        public virtual DateTime VremeP { get; set; }
        public virtual DateTime VremeK { get; set; }


        /*public virtual int IdPrisutnogGradjana { get; set; }
         ADVANCED*/
        public virtual Sednica Sedni { get; set; }

        //veze

        public RadniDan()
        {
            /*PGrupe=new List<PoslanickaGrupa>();*/
        }
    }

}


