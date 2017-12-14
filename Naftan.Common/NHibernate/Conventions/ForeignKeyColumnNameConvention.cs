using System;
using FluentNHibernate;
using FluentNHibernate.Conventions;

namespace Naftan.Common.NHibernate.Conventions
{
	public class ForeignKeyColumnNameConvention : ForeignKeyConvention
	{
		protected override string GetKeyName(Member member, Type type)
		{
		    return String.Format("{0}Id", type.Name);
		}
	}
}