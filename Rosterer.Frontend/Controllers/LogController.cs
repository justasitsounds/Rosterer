using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rosterer.Frontend.Models;

namespace Rosterer.Frontend.Controllers
{
    public class LogController : Controller
    {
        //
        // GET: /Log/

        public ActionResult Index()
        {
            var logfiles = new List<LogFile>();
            var folderInfo = new DirectoryInfo(Server.MapPath("~/Logs/"));
            foreach (var fileInfo in folderInfo.EnumerateFiles())
            {
                logfiles.Add(new LogFile(){LogDate = DateTime.Now,Name = fileInfo.Name});
            }
            folderInfo = null;
            
            return View(logfiles);
        }

        //
        // GET: /Log/Details/5

        public ActionResult Details(string id)
        {
            var logFileContent = new LogFileContent(){FileName = id};
            using (FileStream fs = new FileStream(Server.MapPath("~/Logs/" + id), FileMode.Open, FileAccess.Read,FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (sr.Peek() >= 0) // reading the old data
                    {
                        logFileContent.Lines.Add(sr.ReadLine());
                    }
                    sr.Close();
                }
            }
           
            return View(logFileContent);
        }

        //
        // GET: /Log/Create

       
    }
}
