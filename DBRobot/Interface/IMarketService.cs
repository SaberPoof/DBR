using DBRobot.Models;

namespace DBRobot.Interface
{
    public interface IMarketService
    {
        /// <summary>
        /// Получаем данные из Api
        /// </summary>
        /// <returns>Список данных, преобразованных в модель</returns>
        Task<List<Test>> FetchDataAsync();

    }
}
