using Naftan.Common.AccountManagement;
using Naftan.Common.Domain;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Users;
using Naftan.Maintenance.WebApplication.Dto;
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

        public UserDto Get(int id)
        {
            var user = repository.Get<User>(id);
            return new UserDto(user);
        }

        public UserDto Put(int id, [FromBody] UserDto dto)
        {
            var entity = repository.Get<User>(id);
            dto.Merge(entity, repository);
            repository.Save(entity);
            return new UserDto(entity);
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