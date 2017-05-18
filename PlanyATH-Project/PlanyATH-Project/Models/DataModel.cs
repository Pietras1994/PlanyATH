using Db4objects.Db4o;
using PlanyATH_Project.Controllers;
using System.Collections.Generic;

namespace PlanyATH_Project.Models
{
    public class DataModel
    {
        public string Name { get; set; }
        public string Link { get; set; }

        public static IEnumerable<DataModel> GetDataModelList()
        {
            //DataModel result = new DataModel();
            //result = db.Query<DataModel>(n => n.Name == nazwa).FirstOrDefault();
            //return result;
            using (IObjectContainer db = Db4oEmbedded.OpenFile(HomeController.filename))
            {
                var list = new List<DataModel>();
                // IEnumerable<DataModel> ResultList = new IEnumerable<DataModel>();

                IObjectSet result = db.QueryByExample(typeof(DataModel));
                foreach (object item in result)
                {
                    DataModel res = (DataModel)item;
                    list.Add(res);

                }
                return list;
            }
        }
    }
}