using FluentNHibernate.Mapping;
using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Mapiranja
{
    class RadniDanMapiranje : ClassMap<RadniDan>
    {
        public RadniDanMapiranje()
        {
            Table("RADNIDAN");

            Id(x => x.Id, "ID").GeneratedBy.TriggerIdentity();

            Map(x => x.BrojP, "BROJPOSLANIKANAPOCETKU");
            Map(x => x.VremeP, "POCETAKRADNOGDANA");
            Map(x => x.VremeK, "KRAJRADNOGDANA");


            References(x => x.Sedni).Column("SEDNICA");
        }
    }
}
