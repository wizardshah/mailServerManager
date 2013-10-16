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
    public class MyMail
    {

        public int Id { get; set; }

        [Required]
        [Display (Name = "Email Address")]
        //[RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", ErrorMessage = "Not a Valid Email")] 
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display (Name = "Email Password")]
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool Active { get; set; }

        [Display(Name = "Email Box Max Size")]
        public int MaxSize { get; set; }
        
        public int MyMailServerId { get; set; }

        public virtual MyMailServer MyMailServer { get; set; }


        private string serverUser = "Administrator";//hmailserver username
        private string serverPass = "365connect";//hmailserver password


        public int createNewEmailAccount(string domain)
        {
            Application myMailServer = new Application();

            myMailServer.Authenticate(serverUser, serverPass);

            myMailServer.Connect();

            Domain mydomain = myMailServer.Domains.get_ItemByName(domain);
            
            try
            {

                if (mydomain.Active)
                {

                    Account newAccount = mydomain.Accounts.Add();

                    newAccount.Address = this.EmailAddress;
                    newAccount.Password = this.Password;
                    newAccount.Active = this.Active;
                    newAccount.MaxSize = this.MaxSize;

                    newAccount.Save();

                    mydomain.Save();
                }
            }
            catch
            { }           

            return 0;

        }

        public int deleteEmailAccount(string domain)
        {
            Application myMailServer = new Application();

            myMailServer.Authenticate(serverUser, serverPass);

            myMailServer.Connect();

            try
            {
                Domain mydomain = myMailServer.Domains.get_ItemByName(domain);

                if (mydomain.Active)
                {
                    Account delAccount = mydomain.Accounts.get_ItemByAddress(this.EmailAddress.ToString());

                    if (delAccount.Active)
                    {
                        delAccount.Delete();
                    }
                }

                mydomain.Save();
            }
            catch
            { }            
            
            return 0;
        }

        public int editEmailAccount(string domain)
        {
            Application myMailServer = new Application();

            myMailServer.Authenticate(serverUser, serverPass);

            myMailServer.Connect();

            try
            {
                Domain mydomain = myMailServer.Domains.get_ItemByName(domain);

                if (mydomain.Active)
                {
                    Account myAccount = mydomain.Accounts.get_ItemByAddress(this.EmailAddress.ToString());

                    if (myAccount.Active)
                    {
                        if (this.MaxSize < mydomain.MaxAccountSize)
                            myAccount.MaxSize = this.MaxSize;
                        else
                            return 0;
                        myAccount.Address = this.EmailAddress;
                        myAccount.Password = this.Password;
                        myAccount.Active = this.Active;

                        myAccount.Save();
                    }
                }

                mydomain.Save();
            }
            catch
            { }


            return 0;

        }

    }
}