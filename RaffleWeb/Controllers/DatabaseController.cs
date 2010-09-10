using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RaffleLib.Domain.Repositories.NHibernateRepositories;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Cfg;

namespace RaffleWeb.Controllers
{
    public class DatabaseController : Controller
    {
        private Configuration _config;
        public DatabaseController(Configuration config)
        {
            _config = config;
        }

        public string Update()
        {
            new SchemaUpdate(_config).Execute(false, true);
            return "Updated DB...";
        }

        public string Create()
        {
            new SchemaExport(_config).Execute(false, true, false);
            return "Created the DB...";
        }
    }
}
