using PlanyATH_Server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanyATH_Server.Concrete
{
    public class PlanStoreContext : DbContext
    {
        public DbSet<DataModel> DataModel { get; set; }
    }
}
