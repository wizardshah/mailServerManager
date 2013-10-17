using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


using hMailServer;//hmailserver com api



namespace mailServerManager.Models
{
       
    public class MyMailServer
    {

        public int Id { get; set; }

        [Display (Name = "Domain Name")]
        [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?", ErrorMessage = "Domain Name Error")]
        public string DomainName { get; set; }

        [Range(1, 65536)]
        [Display (Name = "Domain Max Account Size")]
        public int DomainMaxAccountSize { get; set; }

        [Range(1, 65536000)]
        [Display (Name = "Domain Max Size")]
        public int DomainMaxSize { get; set; }

        [Display (Name = "Domain Active Status")]
        [DefaultValue(false)]
        public bool Active { get; set; }


        public virtual ICollection<MyMail> MyMails { get; set; }


        private string serverUser = "Administrator";//hmailserver username
        private string serverPass = "365connect";//hmailserver password

        public void createNewDomain()
        {
            Application myMailServer = new Application();

            myMailServer.Authenticate(serverUser, serverPass);

            myMailServer.Connect();

            Domain newDomain = myMailServer.Domains.Add();

            newDomain.Name = this.DomainName;
            newDomain.MaxAccountSize = this.DomainMaxAccountSize;
            newDomain.MaxSize = this.DomainMaxSize;
            newDomain.Active = this.Active;

            newDomain.Save();

        }

        public int editDomain()
        {
            Application myMailServer = new Application();

            myMailServer.Authenticate(serverUser, serverPass);

            myMailServer.Connect();

            Domain myDomain = myMailServer.Domains.get_ItemByName(this.DomainName.ToString());

            try
            {
                myDomain.Active = this.Active;
                myDomain.Name = this.DomainName;
                myDomain.MaxSize = this.DomainMaxSize;
                myDomain.MaxAccountSize = this.DomainMaxAccountSize;
            }
            catch { }

            myDomain.Save();

            return 0;

        }

        public void deleteDomain()
        {
            Application myMailServer = new Application();

            myMailServer.Authenticate(serverUser, serverPass);

            myMailServer.Connect();

            Domain delDomain = myMailServer.Domains.get_ItemByName(this.DomainName.ToString());

            try
            {
                if(delDomain.Active)
                    delDomain.Delete();
            }
            catch { }

        }

        public bool checkDomain()
        {
            Application myMailServer = new Application();

            myMailServer.Authenticate(serverUser, serverPass);

            myMailServer.Connect();
            try
            {

            Domain checkDomain = myMailServer.Domains.get_ItemByName(this.DomainName.ToString());

            
                if (checkDomain != null)
                    return true;
                else
                    return false;
            }
            catch { }

            return false;
        }

    }
}