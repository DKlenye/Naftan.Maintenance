using NHibernate.Cfg;

namespace Naftan.Common.NHibernate
{
    ///<summary>
    ///  Начальная загрузка для NHibernate
    ///</summary>
    public interface INHibernateInitializer
    {
        ///<summary>
        ///  Построить конфиг
        ///</summary>
        ///<returns> Конфиг </returns>
        Configuration GetConfiguration();
    }
}