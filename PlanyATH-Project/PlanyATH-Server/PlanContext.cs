using PlanyATH_Server.Models;
using System.Data.Entity;

namespace PlanyATH_Server
{
    public class PlanContext : DbContext
    {
        public DbSet<PlanModel> PlanModel { get; set; }

        public PlanContext() : base("Base")
        {

        }
    }
}
