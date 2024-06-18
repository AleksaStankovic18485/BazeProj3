using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NarodnaSkupstina.Entiteti
{
    internal class StalniRadniOdnos
    {
        internal protected virtual int Id { get; set; }
        internal protected virtual int BrojRadneKnjizice { get; set; }
        internal protected virtual int PrethodanRadniStazGod { get; set; }
        internal protected virtual int PrethodanRadniStazMesec { get; set; }
        internal protected virtual int PrethodanRadniStazDan { get; set; }
        internal protected virtual string ImeFirme { get; set; }

        internal protected virtual StalniRadnik? StalniRadnik { get; set; }

        internal StalniRadniOdnos() { }
    }
}
