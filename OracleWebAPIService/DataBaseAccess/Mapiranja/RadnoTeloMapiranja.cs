using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NarodnaSkupstina.Entiteti;
using FluentNHibernate.Mapping;

namespace Prodavnica.Mapiranja
{
    internal class RadnoTeloMapiranja : ClassMap<RadnoTelo>
    {
        public RadnoTeloMapiranja()
        {

            //Mapiranje tabele
            Table("RADNOTELO");

            //mapiranje podklasa
            /*DiscriminateSubClassesOnColumn("TIP");*/

            //mapiranje primarnog kljuca
            // Id(x => x.Id, "ID").GeneratedBy.TriggerIdentity().UnsavedValue(-1);
            Id(x => x.Tip, "TIP").GeneratedBy.TriggerIdentity();

            //mapiranje svojstava
            //Map(x => x.Tip, "TIP");
            Map(x => x.Id, "ID");
            //mapiranje veze 1:N
            References(x => x.SluzbenaProst).Column("BROJPROSTORIJE").LazyLoad();


            HasMany(x => x.Clanovi).KeyColumn("IDENTIF").LazyLoad().Cascade.All().Inverse();
        }
    }

    internal class StalniOdboriMapiranja : SubclassMap<StalniOdbori>
    {
        public StalniOdboriMapiranja()
        {
            DiscriminatorValue("STALNIODBOR");
        }
    }

    internal class AnketniOdboriMapiranja : SubclassMap<AnketniOdbori>
    {
        public AnketniOdboriMapiranja()
        {
            DiscriminatorValue("ANKETNIODBOR");
        }
    }

    internal class KomisijeMapiranja : SubclassMap<Komisije>
    {
        public KomisijeMapiranja()
        {
            DiscriminatorValue("KOMISIJE");
        }
    }
    internal class PrivremeniOdbori : SubclassMap<PrivremeniOdbori>
    {
        public PrivremeniOdbori()
        {
            DiscriminatorValue("PRIVREMENODBOR");
        }
      
    }
}
