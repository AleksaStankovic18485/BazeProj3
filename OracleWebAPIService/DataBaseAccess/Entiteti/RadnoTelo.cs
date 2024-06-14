using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    public class RadnoTelo
    {
        public virtual int Id { get; protected set; }
        public virtual string Tip { get; set; }
        public virtual IList<NarodniPoslanik> Clanovi { get; set; }
        public virtual SluzbenaProstorija SluzbenaProst { get; set; }

        //veze

        public RadnoTelo()
        {
            Clanovi=new List<NarodniPoslanik>();
        }
    }
    public class StalniOdbori : RadnoTelo
    {
        
    }
    public class AnketniOdbori : RadnoTelo
    {

    }
    public class Komisije : RadnoTelo
    {

    }
    public class PrivremeniOdbori : RadnoTelo
    {

    }
}


