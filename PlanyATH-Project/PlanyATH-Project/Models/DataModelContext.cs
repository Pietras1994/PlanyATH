using PlanyATH_Server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PlanyATH_Project.Models
{
    public class DataModelContext : DbContext
    {
        public DbSet<DataModelView> DataModel { get; set; }
    }
}