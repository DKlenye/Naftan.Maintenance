using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Naftan.Common.AccountManagement
{
    public class Account
    {
        public Account(string employeeId, string name, string login, string email, string phone, IEnumerable<string> groups)
        {
            Groups = groups;
            EmployeeId = employeeId;
            Phone = phone;
            Email = email;
            Login = login;
            Name = name;
        }

        public Account(UserPrincipal principal) : this(
            principal.EmployeeId,
            principal.DisplayName,
            principal.SamAccountName,
            principal.EmailAddress,
            principal.VoiceTelephoneNumber, 
            principal.GetGroups().ToList().Select(x=>x.Name))
        {
        }
        
        public string EmployeeId { get; private set; }
        public string Name { get; private set; }
        public string Login { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public IEnumerable<string> Groups { get; private set; }

    }
}
