using System;
using System.Linq;
using Naftan.Common.AccountManagement;
using NUnit.Framework;

namespace Naftan.Common.Tests.AccountManagement
{
    public class ActiveDirectoryTests
    {
        private void PrintAccount(Account account)
        {
            var tpl = "{0}: {1}";

            if (account == null)
            {
                Console.WriteLine("Account is null");
                return;
            }

            Console.WriteLine(tpl, "Name", account.Name);
            Console.WriteLine(tpl, "Login", account.Login);
            Console.WriteLine(tpl, "EmployeeId", account.EmployeeId);
            Console.WriteLine(tpl, "Email", account.Email);
            Console.WriteLine(tpl, "Phone", account.Phone);
            Console.WriteLine("Groups:");
            account.Groups.ToList().ForEach(Console.WriteLine);
        }
        
        [Test]
        public void CurrentAccount()
        {
            var account  = ActiveDirectory.CurrentAccount;
            PrintAccount(account);
        }

        [Test]
        public void GetAccount()
        {
            var account = ActiveDirectory.GetAccount("otord_mech11");
            PrintAccount(account);
        }

        [Test]
        public void GetAccountsByGroup()
        {
            var accounts = ActiveDirectory.GetAccountsByGroup("Transport_Оператор_АЗС");
            accounts.ToList().ForEach(x=>Console.WriteLine(x.Login));
        }


    }
}
