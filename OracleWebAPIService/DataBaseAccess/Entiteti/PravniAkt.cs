using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    public class PravniAkt
    {
        public virtual int Id { get; protected set; }
        public virtual string Naziv { get; set; }
        public virtual string Tip { get; set; }

        //
        public PravniAkt()
        {

        }

    }
    public class Vlada : PravniAkt 
    {
        public Vlada()
        {

        }
    }
    public class PredlozioNarodniPoslanik : PravniAkt
    {
        public virtual IList<PredlozioNarodniPoslanik> PredlogPos { get; set; }
        public PredlozioNarodniPoslanik()
        {
            PredlogPos = new List<PredlozioNarodniPoslanik>();
        }
    }
    public class PredlozioBiraci : PravniAkt
    {
        public virtual int brojBiraca { get; set; }
        public PredlozioBiraci() { }
    }
}
