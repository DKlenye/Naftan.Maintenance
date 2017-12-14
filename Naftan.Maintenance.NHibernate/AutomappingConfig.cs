using System;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.NHibernate
{
    public class AutomappingConfig:DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return typeof(IEntity).IsAssignableFrom(type);
        }

        public override bool IsComponent(Type type)
        {
            return typeof (IEntityComponent).IsAssignableFrom(type);
        }

        public override string GetComponentColumnPrefix(Member member)
        {
            return "";
        }
    }
}
