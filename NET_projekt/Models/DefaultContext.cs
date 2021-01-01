using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NET_projekt.Models
{
    public class DefaultContext : DbContext
    {
        public DefaultContext() : base("DefaultContext")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<EcgDataset> EcgDatasets { get; set; }
        public DbSet<EmgDataset> EmgDatasets { get; set; }
        public DbSet<EcgDataPoint> EcgDataPoints { get; set; }
        public DbSet<EmgDataPoint> EmgDataPoints { get; set; }
    }
}