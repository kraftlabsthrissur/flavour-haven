﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeSuiteApp.Web.Models
{
    public class FileModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string URL { get; set; }
    }
}