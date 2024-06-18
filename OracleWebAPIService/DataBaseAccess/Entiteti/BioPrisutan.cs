using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NarodnaSkupstina.Entiteti
{
    internal class BioPrisutan
    {
        internal protected virtual BioPrisutanId Id { get; set; }
        internal protected virtual DateTime DatumOd { get; set; }
        internal protected virtual DateTime? DatumDo { get; set; }

        internal BioPrisutan()
        {
            Id = new BioPrisutanId();
        }

    }
}
