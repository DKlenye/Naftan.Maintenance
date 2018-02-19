using System.Linq;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Tests.RepairObjectFactories
{
    public abstract class AbstractFactory<T>
        where T:IEntity
    {
        protected AbstractFactory(IRepository repository)
        {
            if (!repository.All<T>().Any())
            {
                Build(repository);
            }
        }
        
        /*
            Описание алгоритма создания объекта ремонта(ОР) для работы в системе:
            
            1. Добавить группу, в которую входит ОР .
            2. Определить межремонтные интервалы для группы ОР.
            3. Если учёт по наработке, то добавить плановую наработку (если она предусмотрена).
            3. Определить технические характеристики для группы ОР.
            3. Добавить ОР (самостоятельный, либо подчинённый другому ОР).
            4. Определить технические характеристики для объекта ремонта.
            5. Определить зап. части, входящие в состав ОР
            
            6. Запустить ОР в работу
            7. Включить в план ППР  
            8. Ввести наработку (если учёт по наработке)
            9. Ввести фактически проведённые ремонты
                    
        */

        protected abstract void Build(IRepository repository);
         
    }
}
