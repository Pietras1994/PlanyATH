using System.Data.Entity;

namespace PlanyATH_Project.Models
{
    public class DataModelContext : DbContext
    {
        public DbSet<DataModelView> DataModels { get; set; }
    }
}