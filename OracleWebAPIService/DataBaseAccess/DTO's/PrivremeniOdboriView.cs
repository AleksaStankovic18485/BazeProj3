using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class PrivremeniOdboriView : RadnoTeloView
    {
        public PrivremeniOdboriView() { }
        internal PrivremeniOdboriView(RadnoTelo? r) : base(r) { }
    }
}
