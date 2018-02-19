using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Users;
using System.Collections.Generic;
using System.Linq;

namespace Naftan.Maintenance.WebApplication.Dto
{
    public class UserDto : AbstractDto<User>
    {

        public string Login { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public IEnumerable<int> Groups { get; set; }
        public IEnumerable<int> Plants { get; set; }
        public int? Site { get; set; }

        public UserDto() { }
        public UserDto(User entity)
        {
            SetEntity(entity);
        }

        public override User GetEntity(IRepository repository)
        {
            var entity = new User(Login, Name, Phone, Email);
            Merge(entity, repository);
            return entity;
        }

        public override void Merge(User entity, IRepository repository)
        {
            entity.Name = Name;
            entity.Phone = Phone;
            entity.Email = Email;
            entity.Site = Site;

            entity.ObjectGroups.Clear();
            entity.Plants.Clear();
            Groups.Select(repository.Get<ObjectGroup>).ToList().ForEach(x => entity.ObjectGroups.Add(x));
            Plants.Select(repository.Get<Plant>).ToList().ForEach(x => entity.Plants.Add(x));

        }

        public override void SetEntity(User entity)
        {
            Id = entity.Id;
            Login = entity.Login;
            Phone = entity.Phone;
            Name = entity.Name;
            Email = entity.Email;
            Site = entity.Site;

            Groups = entity.ObjectGroups.Select(x => x.Id);
            Plants = entity.Plants.Select(x => x.Id);
        }
    }
}