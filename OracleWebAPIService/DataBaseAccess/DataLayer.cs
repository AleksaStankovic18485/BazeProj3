using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NarodnaSkupstina.Mapiranja;
using System.Configuration;
using NarodnaSkupstina.Entiteti;

namespace NarodnaSkupstina
{
    class DataLayer
    {
        private static ISessionFactory _factory = null;
        private static object lockObj = new object();
        public static ISession GetSession()
        {
            if (_factory == null)
            {
                lock (lockObj)
                {
                    if (_factory == null)
                    {
                        _factory = CreateSessionFactory();
                    }
                }
            }

            return _factory.OpenSession();
        }

        private static ISessionFactory CreateSessionFactory()
        {
            try

            {

                var cfg = OracleManagedDataClientConfiguration.Oracle10
                        .ConnectionString(c =>
                        c.Is("DATA SOURCE=gislab-oracle.elfak.ni.ac.rs:1521/SBP_PDB;USER ID=S18485;Password=Hgfdasa02"));

                return Fluently.Configure()
                        .Database(cfg.ShowSql())
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NarodniPoslanikMapiranja>())
                        .BuildSessionFactory();
            }
            catch (Exception ec)
            {
                string error = ec.HandleError();
                return null;
                /*string error = ec.HandleError();*/
                /*System.Windows.Forms.MessageBox.Show(ec.InnerException.Message);
                return null;*/
            }
        }

        /*private static void BuildSchema(NHibernate.Cfg.Configuration cfg)
        {
            // Konfiguracija
        }*/
    }
}

