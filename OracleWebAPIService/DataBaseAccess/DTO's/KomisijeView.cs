using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class KomisijeView : RadnoTeloView
    {
        public KomisijeView() { }
        internal KomisijeView(RadnoTelo? r) : base(r) { }
    }
}
