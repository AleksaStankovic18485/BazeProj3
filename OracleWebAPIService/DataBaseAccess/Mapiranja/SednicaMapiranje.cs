using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using NarodnaSkupstina.Entiteti;

namespace NarodnaSkupstina.Mapiranja
{
    class SednicaMapiranje : ClassMap<NarodnaSkupstina.Entiteti.Sednica>
    {
        public SednicaMapiranje()
        {
            Table("SEDNICA");

            /*kljucevi???*/
            Id(x => x.Id, "ID").GeneratedBy.SequenceIdentity("S18485.SEDNICA_SEQ");

            DiscriminateSubClassesOnColumn("TIPSEDNICE", 0);

            Map(x => x.BrojZasedanja, "BROJZASEDANJA");
            Map(x => x.BrojSaziva, "BROJSAZIVA");
            Map(x => x.DatumEND, "DATUMZAVRSETKA");
            Map(x => x.DatumStart, "DATUMPOCETKA");
            /*Map(x => x.TipFlag, "TIPSEDNICE");*/

            /*Map(x=>x.IdPrisutnogGradnjana)*/
            HasManyToMany(x => x.Poslanici)
                .Table("BIOPRISUTAN")
                .ParentKeyColumn("ID")
                .ChildKeyColumn("IDENTIFIKACIONIBROJ")
                .Inverse()
                .Cascade.All();

            HasMany(x=>x.PrustniSednici).KeyColumn("IDENTIFIKACIONIBROJ").Cascade.All().Inverse();
            //mapiranje za Poslanik - Org
            /* References(x => x.PripadaOrganizaciji).Column("ORGANIZACIJA").Cascade.All();*/




        }

    }

    internal class VandrednaMapiranje : SubclassMap<Sednica>
    {
        public VandrednaMapiranje()
        {
            DiscriminatorValue(1);
        }
    }
}
