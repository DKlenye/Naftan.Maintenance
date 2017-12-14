using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Specifications;
using System;

namespace Naftan.Maintenance.WebApplication.Dto
{
    public abstract class AbstractSpecificationDto<TEntity> : AbstractDto<TEntity>
        where TEntity : IEntity
    {
        protected AbstractSpecificationDto()
        {
        }

        protected object GetValue(SpecificationType type, string value)
        {

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                switch (type)
                {
                    case SpecificationType.Boolean:
                        {
                            return bool.Parse(value);
                        }
                    case SpecificationType.Reference:
                        {
                            return int.Parse(value);
                        }
                    case SpecificationType.Int:
                        {
                            return int.Parse(value);
                        }
                    case SpecificationType.Decimal:
                        {
                            return decimal.Parse(value);
                        }
                    case SpecificationType.Date:
                        {
                            return DateTime.Parse(value);
                        }
                    default:
                        {
                            return value;
                        }
                }
            }
        }
    }
}