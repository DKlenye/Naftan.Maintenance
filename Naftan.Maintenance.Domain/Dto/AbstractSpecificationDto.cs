using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Specifications;
using System;
using System.Globalization;

namespace Naftan.Maintenance.Domain.Dto
{
    public abstract class AbstractSpecificationDto<TEntity> : EntityDto<TEntity>
        where TEntity : IEntity
    {
        protected AbstractSpecificationDto()
        {
        }

        public SpecificationType SpecificationType { get; set; }

        protected string SetValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return value;
        }

        protected string GetValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            switch (SpecificationType)
            {
                case SpecificationType.Date:
                    {
                        DateTime parsed;
                        if (DateTime.TryParseExact(value as string, "dd'.'MM'.'yyyy",
                            CultureInfo.CurrentCulture, DateTimeStyles.None, out parsed))
                        {
                            return value;
                        }

                        return null;
                    }
                default:
                    {
                        return value.ToString();
                    }
            }
        }

    }
}