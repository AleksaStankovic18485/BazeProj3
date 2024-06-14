using FluentNHibernate.Mapping;
using NarodnaSkupstina.Entiteti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarodnaSkupstina.Mapiranja
{
    class PoslanickaGrupaMapiranje : ClassMap<PoslanickaGrupa>
    {
        public PoslanickaGrupaMapiranje()
        {
            Table("POSLANICKAGRUPA");

            Id(x => x.Naziv, "NAZIV_POSLANICKE_GRUPE").GeneratedBy.TriggerIdentity();

            Map(x => x.Id, "ID");


            HasMany(x => x.Prostorije).KeyColumn("IDENTIF").LazyLoad().Cascade.All().Inverse();
            HasMany(x => x.Clanovi).KeyColumn(/*"IDENTIFIKACIONIBROJ"*/"PGRUPA").LazyLoad().Cascade.All().Inverse();
        }
    }
}
