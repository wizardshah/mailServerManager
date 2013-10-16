using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using mailServerManager.Models;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace mailServerManager.Data
{
    public class MyMailContext : DbContext
    {
        public MyMailContext()
            : base("name=MailContext")
        {

        }

        public DbSet<MyMailServer> MyMailServers { get; set; }
        public DbSet<MyMail> MyMails { get; set; }

        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}