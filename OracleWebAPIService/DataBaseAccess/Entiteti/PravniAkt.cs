using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    internal class PravniAkt
    {
        internal protected virtual int Id { get; protected set; }
        internal protected virtual string Naziv { get; set; }
        internal protected virtual string Tip { get; set; }

        //
        internal PravniAkt()
        {

        }

    }
    internal class Vlada : PravniAkt 
    {
        public Vlada()
        {

        }
    }
    internal class PredlozioNarodniPoslanik : PravniAkt
    {
        internal protected virtual IList<PredlozioNarodniPoslanik> PredlogPos { get; set; }
        internal PredlozioNarodniPoslanik()
        {
            PredlogPos = new List<PredlozioNarodniPoslanik>();
        }
    }
    internal class PredlozioBiraci : PravniAkt
    {
        internal protected virtual int brojBiraca { get; set; }
        internal PredlozioBiraci() { }
    }
}
