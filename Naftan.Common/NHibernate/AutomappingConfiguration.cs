using System;
using FluentNHibernate.Automapping;
using Naftan.Common.Domain;

namespace Naftan.Common.NHibernate
{

    public class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool AbstractClassIsLayerSupertype(Type type)
        {
            return true;
        }

        public override bool IsComponent(Type type)
        {
            return typeof (IEntityComponent).IsAssignableFrom(type);
        }

        public override bool IsDiscriminated(Type type)
        {
            return true;
        }

        public override bool ShouldMap(Type type)
        {
            return typeof (IEntity).IsAssignableFrom(type);
        }
    }
}