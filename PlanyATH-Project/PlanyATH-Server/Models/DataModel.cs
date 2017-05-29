using System.ComponentModel.DataAnnotations;

namespace PlanyATH_Server.Models
{
    public class PlanModel
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string FileName { get; set; }
    }
}