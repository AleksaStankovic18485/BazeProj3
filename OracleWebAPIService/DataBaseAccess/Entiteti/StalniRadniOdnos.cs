using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NarodnaSkupstina.Entiteti
{
    public class StalniRadniOdnos
    {
        public virtual int Id { get; protected set; }
        public virtual int BrojRadneKnjizice { get; set; }
        public virtual int PrethodanRadniStazGod { get; set; }
        public virtual int PrethodanRadniStazMesec { get; set; }
        public virtual int PrethodanRadniStazDan { get; set; }
        public virtual string ImeFirme { get; set; }

        public virtual StalniRadnik StalniRadnik { get; set; }

        public StalniRadniOdnos() { }
    }
}
