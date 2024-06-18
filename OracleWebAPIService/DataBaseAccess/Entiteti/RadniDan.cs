using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    internal class RadniDan
    {
        internal protected virtual int Id { get; protected set; }
        internal protected virtual int BrojP { get; set; }
        internal protected virtual DateTime VremeP { get; set; }
        internal protected virtual DateTime VremeK { get; set; }


        /*public virtual int IdPrisutnogGradjana { get; set; }
         ADVANCED*/
        internal protected virtual Sednica Sedni { get; set; }

        //veze

        internal RadniDan()
        {
            /*PGrupe=new List<PoslanickaGrupa>();*/
        }
    }

}


