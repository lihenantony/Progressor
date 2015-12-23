using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Progressor.Models
{
    public class Class1
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Class1DBContext : DbContext
    {
        public DbSet<Class1> Class1s { get; set; }
    }
}