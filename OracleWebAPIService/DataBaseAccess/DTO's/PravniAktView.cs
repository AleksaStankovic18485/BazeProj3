using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class PravniAktView
    {
        public  int Id { get; protected set; }
        public  string Naziv { get; set; }
        public  string Tip { get; set; }

        //
        public PravniAktView()
        {

        }

        internal PravniAktView(PravniAkt? p)
        {
            Id = p.Id;
            Naziv = p.Naziv;
            Tip = p.Tip;
        }
    }
}
