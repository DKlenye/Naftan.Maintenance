using Naftan.Common.AccountManagement;
using Naftan.Common.Domain;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Users;
using System.Collections.Generic;
using System.Web.Http;

namespace Naftan.Maintenance.WebApplication.Controllers.DtoControllers
{
    public class UserController:ApiController
    {
        private readonly IRepository repository;
        private readonly IQueryFactory query;

        public UserController(IQueryFactory query, IRepository repository)
        {
            this.query = query;
            this.repository = repository;
        }

        public IEnumerable<User> Get()
        {
            return repository.All<User>();
        }

        [HttpGet, Route("api/user/current")]
        public User Current ()
        {
            var account = ActiveDirectory.CurrentAccount;
            var user = query.FindUserByLogin(account.Login);
            if (user == null)
            {
                user = new User(account.Login, account.Name, account.Phone, account.Email);
                repository.Save(user);
            }
            return user;
        }
    }
}