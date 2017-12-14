using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Naftan.Common.AccountManagement
{
    public static class ActiveDirectory
    {
        private const string NaftanDomain = "lan.naftan.by";
        private const string PolymirDomain = "POLYMIR.NET";

        public static Account CurrentAccount
        {
            get { return new Account(UserPrincipal.Current); }
        }

        /// <summary>
        /// Получить базовый основной контекст
        /// </summary>
        /// <returns>Возвращает объект PrincipalContext</returns>
        private static PrincipalContext GetPrincipalContext(string domain)
        {
            return new PrincipalContext(ContextType.Domain, domain);
        }


        /// <summary>
        /// Получить аккаунт Active Directory по логину
        /// </summary>
        /// <param name="login">Логин пользователя для извлечения</param>
        /// <returns>Объект Account</returns>
        public static Account GetAccount(string login)
        {
            var principal =
                UserPrincipal.FindByIdentity(GetPrincipalContext(NaftanDomain), IdentityType.SamAccountName, login) ??
                UserPrincipal.FindByIdentity(GetPrincipalContext(PolymirDomain), IdentityType.SamAccountName, login);
            if (principal == null) return null;

            return new Account(principal);
        }


        /// <summary>
        /// Получить список аккаунтов по группе
        /// </summary>
        /// <param name="groupName">Наименование группы</param>
        /// <returns>Список аккаунтов</returns>
        public static IEnumerable<Account> GetAccountsByGroup(string groupName)
        {
            var context = GetPrincipalContext(NaftanDomain);
            var findByIdentity = GroupPrincipal.FindByIdentity(context, groupName);
            if (findByIdentity != null)
                return findByIdentity
                    .Members
                    .Where(x => x.GetType() == typeof (UserPrincipal))
                    .Select(x => new Account(x as UserPrincipal));
            return null;
        }
    }
}
