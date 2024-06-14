using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.DTO_s
{
    public class PredlozioBiraciView: PravniAktView
    {
        PredlozioBiraciView() { }
        internal PredlozioBiraciView(PravniAkt? p) : base(p) { }
    }
}
