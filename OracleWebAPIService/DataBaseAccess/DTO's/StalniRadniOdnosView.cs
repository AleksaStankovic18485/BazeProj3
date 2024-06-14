using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class StalniRadniOdnosView
    {
        public int Id { get; protected set; }
        public int BrojRadneKnjizice { get; set; }
        public  int PrethodanRadniStazGod { get; set; }
        public int PrethodanRadniStazMesec { get; set; }
        public int PrethodanRadniStazDan { get; set; }
        public string ImeFirme { get; set; }

        public virtual StalniRadnikView StalniRadnik { get; set; }

        public StalniRadniOdnosView() { }

        internal StalniRadniOdnosView(StalniRadniOdnos? s)
        {
            if (s != null)
            {
                Id = s.Id;
                BrojRadneKnjizice = s.BrojRadneKnjizice;
                PrethodanRadniStazDan=s.PrethodanRadniStazDan;
                PrethodanRadniStazMesec = s.PrethodanRadniStazMesec;
                PrethodanRadniStazGod = s.PrethodanRadniStazGod;
                ImeFirme = s.ImeFirme;
                StalniRadnik= new StalniRadnikView(s.StalniRadnik);
                
            }
        }
    }
}
