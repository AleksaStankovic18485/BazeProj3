using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Entiteti
{
    internal class SluzbenaProstorija
    {
        internal protected virtual int Id { get; protected set; }
        internal protected virtual int BrojProstorije { get; set; }
        internal protected virtual int Sprat { get; set; }
        //veza
        internal protected virtual PoslanickaGrupa ProstorijaPoslanickeGrupe { get; set; }

        internal SluzbenaProstorija()
        {

        }
    }
}
