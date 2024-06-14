using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using NarodnaSkupstina.Entiteti;

namespace NarodnaSkupstina.Mapiranja
{
    class PravniAktMapiranje : ClassMap<NarodnaSkupstina.Entiteti.PravniAkt>
    {
        public PravniAktMapiranje()
        {
            Table("PRAVNIAKT");

            /*kljucevi???*/
            Id(x => x.Id, "ID").GeneratedBy.TriggerIdentity();

            //nema stalniradniodnos ako je 0
            //DiscriminateSubClassesOnColumn("STALNIRADNIODNOSFLAG", 0);

            Map(x => x.Naziv, "NAZIV");
            Map(x => x.Tip, "TIP");
 
            //mapiranje za Poslanik - Org
            /* References(x => x.PripadaOrganizaciji).Column("ORGANIZACIJA").Cascade.All();*/




        }

    }

    public class PredlozioNarodniPoslanikMapiranje : SubclassMap<PravniAkt>
    {
        public PredlozioNarodniPoslanikMapiranje()
        {
            //DiscriminatorValue(1);

            //HasMany(x => x.Stalni).KeyColumn("BROJRADNEKNJIZICE").LazyLoad().Cascade.All().Inverse();
        }
    }
    public class PredlozioBiraciMapiranje : SubclassMap<PravniAkt>
    {
        public PredlozioBiraciMapiranje()
        {

        }
    }
}
