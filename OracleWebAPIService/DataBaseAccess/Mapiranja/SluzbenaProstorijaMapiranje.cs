using FluentNHibernate.Mapping;
using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Mapiranja
{
     class SluzbenaProstorijaMapiranje : ClassMap<SluzbenaProstorija>
    {
        public SluzbenaProstorijaMapiranje()
        {
            Table("SLUZBENAPROSTORIJA");

            Id(x => x.Id, "ID").GeneratedBy.TriggerIdentity();

            Map(x => x.Sprat, "SPRAT");
            Map(x => x.BrojProstorije, "BROJPROSTORIJE");


            References(x => x.ProstorijaPoslanickeGrupe).Column("IDENTIF");       
        }
    }
}
