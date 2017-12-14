using NHibernate;

namespace Naftan.Common.NHibernate
{
    /// <summary>
    /// Провайдер сессии
    /// </summary>
    public interface ISessionProvider
    {
        ISession CurrentSession { get; } 
    }
}
