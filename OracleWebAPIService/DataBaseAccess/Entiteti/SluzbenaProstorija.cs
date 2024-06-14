using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    public class SluzbenaProstorija
    {
        public virtual int Id { get; protected set; }
        public virtual int BrojProstorije { get; set; }
        public virtual int Sprat { get; set; }
        //veza
        public virtual PoslanickaGrupa ProstorijaPoslanickeGrupe { get; set; }

        public SluzbenaProstorija()
        {

        }
    }
}
