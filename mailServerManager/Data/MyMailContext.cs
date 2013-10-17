using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using mailServerManager.Models;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;

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

        public void Detach(object entity)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            objectContext.Detach(entity);
        }
    }
}