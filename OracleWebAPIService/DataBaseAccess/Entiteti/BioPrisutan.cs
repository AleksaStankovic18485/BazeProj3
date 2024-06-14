using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NarodnaSkupstina.Entiteti
{
    public class BioPrisutan
    {
        public virtual BioPrisutanId Id { get; set; }
        public virtual DateTime DatumOd { get; set; }
        public virtual DateTime? DatumDo { get; set; }

        public BioPrisutan()
        {
            Id = new BioPrisutanId();
        }

    }
}
