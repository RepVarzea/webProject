using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SiteVarzea.Models
{
    public class OurDbContext : DbContext
    {
        public DbSet<MORADOR> morador { get; set; }
    }
}