using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    public class Sednica
    {
        public virtual int Id { get; protected set; }
        public virtual int BrojZasedanja { get; set; }
        public virtual int BrojSaziva { get; set; }
        public virtual DateTime DatumEND { get; set; }
        public virtual DateTime DatumStart { get; set; }
        public virtual bool TipFlag { get; set; }

        /*public virtual int IdPrisutnogGradjana { get; set; }
         ADVANCED*/
        public virtual IList<BioPrisutan> PrustniSednici { get; set; }
        public virtual IList<NarodniPoslanik> Poslanici { get; set; }
        public virtual IList<RadniDan> RDani { get; set; }

        //veze
        public Sednica()
        {
            Poslanici=new List<NarodniPoslanik>();
            PrustniSednici=new List<BioPrisutan>();
            RDani=new List<RadniDan>();
        }

    }
    public class Vandredna : Sednica
    {
        public virtual bool Zahtevana { get; set; }
        public Vandredna() { }
    }
}


