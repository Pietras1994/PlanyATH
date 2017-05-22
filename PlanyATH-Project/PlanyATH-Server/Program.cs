using Db4objects.Db4o;
using HtmlAgilityPack;
using PlanyATH_Server.Models;
using RestSharp;
using RestSharp.Extensions;
using System.IO;
using System.Linq;
using System.Text;
using Topshelf;

namespace PlanyATH_Server
{
    public class Program
    {
        static void Main(string[] args)
        {
        }

        public static string filename = "DataBase.data";

        public void GetData()
        {
            var client = new RestClient("http://plany.ath.bielsko.pl/right_menu_result_plan.php");
            var request = new RestRequest(Method.POST);
            //request.AddHeader("postman-token", "0edcb9f6-0f5b-6bb2-ae00-03d2644d7b1e");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "search=plan&conductors=1&rooms=1&word=", ParameterType.RequestBody);
            client.DownloadData(request).SaveAs("temp.html");
            IRestResponse response = client.Execute(request);

        }
        public void ReadDataFromFile()
        {

            using (IObjectContainer db = Db4oEmbedded.OpenFile(filename))
            {
                string html = @"C:\Program Files (x86)\IIS Express\temp.html";
                HtmlDocument doc = new HtmlDocument();
                //doc.Load(html);

                StreamReader reader = new StreamReader(html, Encoding.UTF8);         
                doc.Load(reader);

                var nodes = doc.DocumentNode.Descendants().Where(n => n.Name == "a").Select(n => new { Name = n.InnerText, value = n.Attributes[0].Value }).ToList();

                foreach (var item in nodes)
                {

                    DataModel dm = new DataModel();
                    dm.Name = item.Name;
                    dm.Link = "http://plany.ath.bielsko.pl/" + item.value;

                    db.Store(dm);
                }
                db.Commit();
            }
        }

    }
}
