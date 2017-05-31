using PlanyATH_Server;
using System.Collections.Generic;
using System.Linq;

namespace PlanyATH_Project.Models
{
    public class DataModelView
    {
        public string Name { get; set; }
        public string FileName { get; set; }

        public static IEnumerable<DataModelView> GetDataModelList()
        {
            using (var db = new PlanContext())
            {
                var list = new List<DataModelView>();


                var result = db.PlanModel.ToList();

                foreach (var item in result)
                {
                    var plan = new DataModelView()
                    {
                        FileName = item.FileName,
                        Name = item.Name
                    };
                    list.Add(plan);
                    
                }
                return list;
            }
        }
    }
}