using CoinPlanner.DataBase.ModelsDb;
using CoinPlanner.DataBase.ModelsDB;
using Microsoft.EntityFrameworkCore;

namespace CoinPlanner.DataBase;

public class DBProcessing
{
    public DBProcessing() {}

    public List<Plans> PlansList { get; set; } = new();
    public List<Operations> OperationsList { get; set; } = new();
    public List<Categories> CategoriesList { get; set; } = new();

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
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Получение данных с БД
    /// </summary>
    public bool LoadDataFromDatabaseAsync()
    {
        try
        {
            using (AppDbContext db = new AppDbContext())
            {
                PlansList = db.plans.FromSqlRaw("SELECT * FROM plans").ToList();
                OperationsList = db.Database.SqlQueryRaw<Operations>("SELECT o.oper_id, t.type_name AS type_name, ct.category_name AS category_name, o.oper_name, o.oper_sum, o.oper_completed, o.oper_next_date, o.oper_plan_id " +
                                                                     "FROM operations o " +
                                                                     "JOIN plans p ON o.oper_plan_id = p.plan_id " +
                                                                     "JOIN type_operations t ON o.oper_type_id = t.type_id " +
                                                                     "JOIN categories ct ON o.oper_category_id = ct.category_id " +
                                                                     "ORDER BY o.oper_id;").ToList();
                CategoriesList = db.categories.FromSqlRaw("SELECT * FROM categories ORDER BY category_id").ToList();
            }
            return true;
        }
        catch (Exception )
        {
            return false;
        }
    }
}
