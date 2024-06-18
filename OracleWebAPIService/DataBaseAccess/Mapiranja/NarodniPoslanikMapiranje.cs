using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using NarodnaSkupstina.Entiteti;

namespace NarodnaSkupstina.Mapiranja
{
    class NarodniPoslanikMapiranja : ClassMap<NarodnaSkupstina.Entiteti.NarodniPoslanik>
    {
        public NarodniPoslanikMapiranja()
        {
            Table("NARODNIPOSLANIK");

            /*kljucevi???*/
            Id(x => x.Id, "IDENTIFIKACIONIBROJ").GeneratedBy.TriggerIdentity();

            //nema stalniradniodnos ako je 0
            DiscriminateSubClassesOnColumn("STALNIRADNIODNOSFLAG", 0);

            Map(x => x.JMBG, "JMBG");
            Map(x => x.IdentifikacioniBroj, "ID");
            Map(x => x.Ime, "IME");
            Map(x => x.ImeJednogRoditelja, "IMEJEDNOGRODITELJA");
            Map(x => x.Prezime, "PREZIME");
            /*Map(x => x.IzbornaLista, "IZBORNALISTA");*/
            Map(x => x.DatumRodjenja, "DATUMRODENJA");
            Map(x => x.MestoRodjenja, "MESTORODENJA");
            Map(x => x.Grad, "GRAD");
            Map(x => x.Adresa, "ADRESA");
            Map(x => x.BrojTelefona, "BROJTELEFONA");
            Map(x => x.BrojMobilnog, "BROJMOBILNOG");
            /*Map(x=>x.IdPrisutnogGradnjana)*/
            References(x => x.PGrupa).Column("PGRUPA");
            References(x => x.RadnoT).Column("RADNOTELO");
            //mapiranje za Poslanik - Org
            /* References(x => x.PripadaOrganizaciji).Column("ORGANIZACIJA").Cascade.All();*/




        }

    }

    internal class StalniRadnikMapiranje : SubclassMap<StalniRadnik>
    {
        public StalniRadnikMapiranje()
        {
            DiscriminatorValue(1);

            HasMany(x => x.Stalni).KeyColumn("BROJRADNEKNJIZICE").LazyLoad().Cascade.All().Inverse();
        }
    }
}
