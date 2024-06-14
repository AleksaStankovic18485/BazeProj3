using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class AnketniOdboriView : RadnoTeloView
    {
        public AnketniOdboriView()
        {

        }
        internal AnketniOdboriView(RadnoTelo? r) : base(r) { }
    }
}
