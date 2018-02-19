using Naftan.Common.AccountManagement;
using Naftan.Common.Domain;
using Naftan.Common.Extensions;
using Naftan.Maintenance.Domain.Objects;
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
        
        protected User() {
            Plants = new HashSet<Plant>();
            ObjectGroups = new HashSet<ObjectGroup>();
        }

        
        public User(string login, string name, string phone, string email): this()
        {
            Login = login;
            Name = name;
            Phone = phone;
            Email = email;
        }

        ///<inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Фио
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Имя пользователя (логин)
        /// </summary>
        public string Login { get; private set; }
        
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }
        
        /// <summary>
        /// Электронная почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Установки, разрешённые для пользователя
        /// </summary>
        public ISet<Plant> Plants { get; set; }

        /// <summary>
        /// Участок электодвигателей, разрешённый для пользователя
        /// </summary>
        public int? Site { get; set; }

        /// <summary>
        /// Группы, оазрешённые для пользователя
        /// </summary>
        public ISet<ObjectGroup> ObjectGroups { get; set; }


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