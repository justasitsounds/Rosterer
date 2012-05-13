using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rosterer.Frontend.Models
{
    public class LogFile
    {
        public string Name { get; set; }
        public DateTime LogDate { get; set; }
    }

    public class LogFileContent
    {
        public string FileName { get; set; }
        public List<string> Lines { get; set; }

        public LogFileContent()
        {
            Lines = new List<string>();
        }
    }
}