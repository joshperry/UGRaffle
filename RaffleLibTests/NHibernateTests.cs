using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaffleLib.Domain.Repositories.NHibernateRepositories;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using RaffleLib.Domain.Entities;
using System.Data.SQLite;
using NHibernate;
using RaffleLib.Domain;

namespace RaffleLibTests
{
    [TestClass]
    public class NHibernateTests
    {
        const string CONNECTION_STRING = "Data Source=:memory:;Version=3;New=True;";
        static NHibernateConfigurator _memoryConfigurator;
        private static NHibernateConfigurator GetMemoryConfigurator()
        {
            if(_memoryConfigurator == null)
                _memoryConfigurator = new NHibernateConfigurator(SQLiteConfiguration.Standard.ConnectionString(CONNECTION_STRING));

            return _memoryConfigurator;
        }

        static SQLiteConnection _conn;
        private static System.Data.IDbConnection GetMemoryConnection()
        {
            if (_conn == null)
            {
                _conn = new SQLiteConnection(CONNECTION_STRING);
                _conn.Open();
            }

            return _conn;
        }

        private static ISession OpenMemorySession()
        {
            return GetMemoryConfigurator().Factory.OpenSession(GetMemoryConnection());
        }

        private static void ExportSchema()
        {
            new SchemaExport(GetMemoryConfigurator().Configuration).Execute(false, true, false, GetMemoryConnection(), null);
        }

        [TestMethod]
        public void Should_create_configuration()
        {
            var configurator = GetMemoryConfigurator();
            Assert.IsNotNull(configurator.Configuration);
        }

        [TestMethod]
        public void Should_map_entities()
        {
            var configurator = GetMemoryConfigurator();

            var meetingMap = configurator.Configuration.GetClassMapping(typeof(Meeting));

            Assert.IsNotNull(meetingMap);
        }

        [TestMethod]
        public void Should_create_session_factory()
        {
            var configurator = GetMemoryConfigurator();
            Assert.IsNotNull(configurator.Factory);
        }

        [TestMethod]
        public void Should_create_database()
        {
            ExportSchema();

            var session = OpenMemorySession();
            
            try
            {
                var meetings = session.CreateCriteria<Meeting>().List<Meeting>();
                Assert.IsNotNull(meetings);
            }
            catch (NHibernate.Exceptions.GenericADOException ex)
            {
                Assert.Fail(ex.InnerException.Message);
            }
        }
    }

}
