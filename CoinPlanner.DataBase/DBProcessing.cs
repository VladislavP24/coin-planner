using CoinPlanner.DataBase.ModelsDB;
using Microsoft.EntityFrameworkCore;

namespace CoinPlanner.DataBase;

public class DBProcessing
{
    public DBProcessing() {}

    public List<Plans> PlansList { get; set; }
    public List<Operations> OperationsList { get; set; }

    /// <summary>
    /// Проверка подключения к БД
    /// </summary>
    public async Task<bool> CheckDatabaseConnectionAsync()
    {
        try
        {
            using (var context = new AppDbContext())
            {
                await context.Database.CanConnectAsync();
                return true;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    /// <summary>
    /// Получение данных с БД
    /// </summary>
    public async Task<bool> LoadDataFromDatabaseAsync()
    {
        try
        {
            using (AppDbContext db = new AppDbContext())
            {
                PlansList = db.Plans.FromSqlRaw("SELECT * FROM plans").ToList();
                OperationsList = db.Database.SqlQueryRaw<Operations>("SELECT o.oper_id, t.type_name as type_name, o.oper_name, o.oper_sum, o.oper_completed, o.oper_next_date, o.oper_plan_id " +
                                                                     "FROM operations o, type_operations t " +
                                                                     "WHERE o.oper_plan_id = t.type_id " +
                                                                     "ORDER BY o.oper_id;").ToList();
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
