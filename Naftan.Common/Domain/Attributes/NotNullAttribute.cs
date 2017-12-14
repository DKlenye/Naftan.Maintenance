using System;

namespace Naftan.Common.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =false,Inherited =true)]
    public class NotNullAttribute:Attribute
    {
    }
}
