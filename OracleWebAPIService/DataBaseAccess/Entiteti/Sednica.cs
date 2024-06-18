using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    internal class Sednica
    {
        internal protected virtual int Id { get; protected set; }
        internal protected virtual int BrojZasedanja { get; set; }
        internal protected virtual int BrojSaziva { get; set; }
        internal protected virtual DateTime DatumEND { get; set; }
        internal protected virtual DateTime DatumStart { get; set; }
        internal protected virtual bool TipFlag { get; set; }

        /*internal protected virtual int IdPrisutnogGradjana { get; set; }
         ADVANCED*/
        internal protected virtual IList<BioPrisutan> PrustniSednici { get; set; }
        internal protected virtual IList<NarodniPoslanik> Poslanici { get; set; }
        internal protected virtual IList<RadniDan> RDani { get; set; }

        //veze
        internal  Sednica()
        {
            Poslanici=new List<NarodniPoslanik>();
            PrustniSednici=new List<BioPrisutan>();
            RDani=new List<RadniDan>();
        }

    }
    internal  class Vandredna : Sednica
    {
        internal protected virtual bool Zahtevana { get; set; }
        internal protected Vandredna() { }
    }
}


