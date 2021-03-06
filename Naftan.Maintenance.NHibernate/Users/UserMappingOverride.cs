﻿using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Naftan.Maintenance.Domain.Users;

namespace Naftan.Maintenance.NHibernate.Users
{
    public class UserMappingOverride : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
            mapping.Table("Users");
            mapping.HasManyToMany(x => x.Plants).AsSet().Cascade.SaveUpdate().LazyLoad();
            mapping.HasManyToMany(x => x.ObjectGroups).AsSet().Cascade.SaveUpdate().LazyLoad();
        }
    }
}