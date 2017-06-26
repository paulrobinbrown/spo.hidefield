using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SPO.HideField
{
    class Program
    {
        private const string Username = "paul.brown@adepteq.com";
        private const string WebUrl = "https://productcleanltd.sharepoint.com/jobs";

        static void Main(string[] args)
        {
            using (ClientContext ctx = SharePointContext.GetContext(Username, WebUrl))
            {
                Web web = ctx.Web;
                ctx.Load(web, w => w.Title);
                ctx.ExecuteQueryWithRetry(5, 1000);

                MessageLog.Write(web.Title);

                var list = ctx.Web.Lists.GetByTitle("Jobs");
                var field = list.Fields.GetByTitle("Job time");

                field.SetShowInEditForm(false);
                field.Update();
                ctx.ExecuteQuery();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
