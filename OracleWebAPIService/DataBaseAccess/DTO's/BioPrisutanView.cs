using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class BioPrisutanView
    {
        public  BioPrisutanIdView? Id { get; set; }
        public  DateTime DatumOd { get; set; }
        public  DateTime? DatumDo { get; set; }

        public BioPrisutanView()
        {
            Id = new BioPrisutanIdView(); //mzd ovo ide
        }
        internal BioPrisutanView(BioPrisutan? b)
        {
            Id = new BioPrisutanIdView(b.Id);
            DatumOd = b.DatumOd;
            DatumDo = b.DatumDo;
        }
    }
}
