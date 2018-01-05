using Naftan.Common.AccountManagement;
using Naftan.Common.Domain;
using Naftan.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naftan.Maintenance.Domain.Users
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User:IEntity
    {

        static Dictionary<string, UserRoles> rolesMap = new Dictionary<string, UserRoles>();
        static User() {
            Enum.GetNames(typeof(UserRoles))
                .ToDictionary(x => x.ConvertToEnum<UserRoles>());
        }


        protected User() { }

        public User(string login, string name, string phone, string email)
        {
            Login = login;
            Name = name;
            Phone = phone;
            Email = email;
        }

        public int Id { get; set; }

        /// <summary>
        /// Фио
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Имя пользователя (логин)
        /// </summary>
        public string Login { get; private set; }
        
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; private set; }
        
        /// <summary>
        /// Электронная почта
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Роли пользователя
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserRoles> GetRoles()
        {
            var account = ActiveDirectory.GetAccount(Login);

            return account.Groups.Where(rolesMap.ContainsKey)
                .Select(x => x.ConvertToEnum<UserRoles>());

        }

    }
}