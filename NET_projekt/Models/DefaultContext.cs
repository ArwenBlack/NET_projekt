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
        public DbSet<Dataset> Datasets { get; set; }
    }
}