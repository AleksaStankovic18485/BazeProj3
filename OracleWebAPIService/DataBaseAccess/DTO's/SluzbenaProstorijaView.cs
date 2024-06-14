using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class SluzbenaProstorijaView
    {
        public virtual int Id { get; protected set; }
        public virtual int BrojProstorije { get; set; }
        public virtual int Sprat { get; set; }
        //veza
        public virtual PoslanickaGrupa ProstorijaPoslanickeGrupe { get; set; }

        public SluzbenaProstorijaView()
        {

        }

        internal SluzbenaProstorijaView(SluzbenaProstorija? p) 
        {
            if (p != null)
            {
                Id = p.Id;
                BrojProstorije = p.BrojProstorije;
                Sprat= p.Sprat;
            }
        }
    }
}
