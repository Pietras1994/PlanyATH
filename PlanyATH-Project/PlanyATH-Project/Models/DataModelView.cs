﻿using Db4objects.Db4o;
using PlanyATH_Project.Controllers;
using System.Collections.Generic;

namespace PlanyATH_Project.Models
{
    public class DataModelView
    {
        public string Name { get; set; }
        public string Link { get; set; }

        public static IEnumerable<DataModelView> GetDataModelList()
        {
            //DataModel result = new DataModel();
            //result = db.Query<DataModel>(n => n.Name == nazwa).FirstOrDefault();
            //return result;
            using (IObjectContainer db = Db4oEmbedded.OpenFile(HomeController.filename))
            {
                var list = new List<DataModelView>();
                // IEnumerable<DataModel> ResultList = new IEnumerable<DataModel>();

                IObjectSet result = db.QueryByExample(typeof(DataModelView));
                foreach (object item in result)
                {
                    DataModelView res = (DataModelView)item;
                    list.Add(res);

                }
                db.Close();
                return list;
            }
        }
    }
}