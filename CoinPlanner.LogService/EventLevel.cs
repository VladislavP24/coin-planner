using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.LogService;

public enum EventLevel
{
    Fatal, // Фатальная ошибка
    Error, // Ошибка
    Warn,  // Предупреждегние
    Info,  // Информация
    Debug, // Отладка
    Trace  // Трассировка
}
