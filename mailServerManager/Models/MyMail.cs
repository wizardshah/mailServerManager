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
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display (Name = "Email Password")]
        public string Password { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }

        [Display(Name = "Email Box Max Size")]
        public int MaxSize { get; set; }
        
        public int MyMailServerId { get; set; }

        public virtual MyMailServer MyMailServer { get; set; }


        private string serverUser = "Administrator";//hmailserver username
        private string serverPass = "365connect";//hmailserver password


        public int createNewEmailAccount(string domain)
        {
            //interface to hMailserver
            Application myMailServer = new Application();

            //authentication for mail server
            myMailServer.Authenticate(serverUser, serverPass);

            //connect to mail server
            myMailServer.Connect();

            //current domain
            Domain mydomain = myMailServer.Domains.get_ItemByName(domain);
            
            try
            {
                if (mydomain != null)
                {
                    //create new Account object with Add() attribute
                    Account newAccount = mydomain.Accounts.Add();
                    //set up the attributes
                    newAccount.Address = this.EmailAddress;
                    newAccount.Password = this.Password;
                    newAccount.Active = this.Active;
                    newAccount.MaxSize = this.MaxSize;
                    //save account and domain
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

                if (mydomain != null)
                {
                    //find the account object by Email Address
                    Account delAccount = mydomain.Accounts.get_ItemByAddress(this.EmailAddress.ToString());

                    if (delAccount != null)
                    {
                        delAccount.Delete();
                    }
                    else
                        return 1;
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

                if (mydomain != null)
                {
                    Account myAccount = mydomain.Accounts.get_ItemByAddress(this.EmailAddress.ToString());

                    if (myAccount != null)
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
            {
 
            }


            return 0;

        }

        //this function  check that the email already exists or not
        public bool checkEmail(string domain, string email)
        {
            Application myMailServer = new Application();

            myMailServer.Authenticate(serverUser, serverPass);

            myMailServer.Connect();

            try
            {
                Domain mydomain = myMailServer.Domains.get_ItemByName(domain);

                if (mydomain != null)
                {
                    Account myAccount = mydomain.Accounts.get_ItemByAddress(email);

                    if (myAccount != null)
                    {
                        return true;
                    }
                    else
                        return false;
                }                
            }
            catch
            {
                return false;
            }

            return false;
        }


    }
}