using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Naftan.Common.NHibernate.Conventions
{
    /// <summary>
    /// Соглашение о наименовании первичного ключа в базе данных. Имя сущности + Id.
    /// Для сущности User первичный ключ будет иметь наименование UserId
    /// </summary>
	public class PrimaryKeyConvention : IIdConvention
	{
		public void Apply(IIdentityInstance instance)
		{
		    instance.Column(String.Format("{0}Id", instance.EntityType.Name));
		}
	}
}