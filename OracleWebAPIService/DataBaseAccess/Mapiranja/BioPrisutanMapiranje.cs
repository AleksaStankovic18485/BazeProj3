using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NarodnaSkupstina.Entiteti;
using FluentNHibernate.Mapping;

namespace NarodnaSkupstina.Mapiranja
{
    class BioPrisutanMapiranje : ClassMap<BioPrisutan>
    {
        public BioPrisutanMapiranje()
        {
            Table("BIOPRISUTAN");

            CompositeId(x => x.Id)
                .KeyReference(x => x.NPPrisutan, "IDENTIFIKACIONIBROJ")
                .KeyReference(x => x.SednicaZasedanja, "ID");

            Map(x => x.DatumOd).Column("DATUM_OD");
            Map(x => x.DatumDo).Column("DATUM_DO");
        }
    }
}
