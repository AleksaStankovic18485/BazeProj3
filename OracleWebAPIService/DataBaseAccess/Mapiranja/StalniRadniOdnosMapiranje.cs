using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NarodnaSkupstina.Entiteti;
using FluentNHibernate.Mapping;

namespace NarodnaSkupstina.Mapiranja
{
    class StalniRadniOdnosMapiranje : ClassMap<StalniRadniOdnos>
    {
        public StalniRadniOdnosMapiranje()
        {
            //Mapiranje tabele
            Table("STALNIRADNIODNOS");

            //mapiranje primarnog kljuca
            Id(x => x.Id, "ID").GeneratedBy.SequenceIdentity("S18485.STALNIRADNIODNOS_ID_SEQ");

            //mapiranje svojstava.
            Map(x => x.PrethodanRadniStazDan, "PRETHODNIRADNISTAZDAN");
            Map(x => x.PrethodanRadniStazMesec, "PRETHODNIRADNISTAZMES");
            Map(x => x.PrethodanRadniStazGod, "PRETHODNIRADNISTAZGOD");
            Map(x => x.ImeFirme, "IMEFIRME");

            //mapiranje veza
            References(x => x.StalniRadnik).Column("BROJRADNEKNJIZICE");

        }

    }
}
