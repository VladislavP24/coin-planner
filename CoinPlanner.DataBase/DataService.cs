using CoinPlanner.DataBase.ModelsDb;
using CoinPlanner.DataBase.ModelsDB;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;

namespace CoinPlanner.DataBase;

public class DataService
{
    public DataService() {}

    public List<Plans> PlansList { get; set; } = new();
    public List<Operations> OperationsList { get; set; } = new();
    public List<Categories> CategoriesList { get; set; } = new();

    //Переменные для хранения изменений
    public Dictionary<int, int> PlanCondition = new();
    public Dictionary<int, int> OperCondition = new();

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
    public async Task<bool> LoadDataFromDatabaseAsync()
    {
        try
        {
            using (AppDbContext db = new AppDbContext())
            {
                PlansList = await db.plans.FromSqlRaw("SELECT * FROM plans").ToListAsync();
                OperationsList = await db.Database.SqlQueryRaw<Operations>("SELECT o.oper_id, t.type_name AS type_name, ct.category_name AS category_name, o.oper_name, o.oper_sum, o.oper_completed, o.oper_next_date, o.oper_plan_id " +
                                                                           "FROM operations o " +
                                                                           "JOIN plans p ON o.oper_plan_id = p.plan_id " +
                                                                           "JOIN type_operations t ON o.oper_type_id = t.type_id " +
                                                                           "JOIN categories ct ON o.oper_category_id = ct.category_id " +
                                                                           "ORDER BY o.oper_id;").ToListAsync();
                CategoriesList = await db.categories.FromSqlRaw("SELECT * FROM categories ORDER BY category_id").ToListAsync();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


    /// <summary>
    /// Обновление изменений в БД (синхронизация)
    /// </summary>
    public bool SaveDataToDatabaseAsync()
    {
        try
        {
            using (AppDbContext db = new AppDbContext())
            {
                //Сохранение Plans
                foreach (var condition in PlanCondition)
                {
                    if (condition.Value == 1)
                    {
                        var plan = PlansList.Where(x => x.Plan_Id == condition.Key).First();
                        db.Database.ExecuteSqlRaw($"INSERT INTO plans (plan_id, plan_name, date_create, date_update) " +
                                                  $"VALUES ({plan.Plan_Id}, '{plan.Plan_Name}', '{plan.Date_Create}', '{plan.Date_Update}')");
                    } 
                    else if (condition.Value == 2)
                    {
                        var plan = PlansList.Where(x => x.Plan_Id == condition.Key).First();
                        db.Database.ExecuteSqlRaw($"UPDATE plans SET plan_name = {plan.Plan_Name}, date_update = {plan.Date_Update} " +
                                                  $"WHERE plan_id = {plan.Plan_Id}");
                    }  
                    else if (condition.Value == 3)
                        db.Database.ExecuteSqlRaw($"DELETE FROM plans WHERE plan_id = {condition.Key}");
                }

                //Сохранение Operations
                foreach (var condition in OperCondition)
                {
                    if (condition.Value == 1)
                    {
                        var oper = OperationsList.Where(x => x.Oper_Id == condition.Key).First();
                        db.Database.ExecuteSqlRaw($"INSERT INTO operations (oper_id, oper_plan_id, oper_name, oper_type_id, oper_category_id, oper_sum, oper_completed, oper_next_date) " +
                                                  $"VALUES ({oper.Oper_Id}, {oper.Oper_Plan_Id}, '{oper.Oper_Name}', {ConvertOperType(oper)}, {ConvertOperCategory(oper)}, {oper.Oper_Sum}, {oper.Oper_Completed}, '{oper.Oper_Next_Date}')");
                    } 
                    else if (condition.Value == 2)
                    {
                        var oper = OperationsList.Where(x => x.Oper_Id == condition.Key).First();
                        db.Database.ExecuteSqlRaw($"UPDATE operations SET oper_name = '{oper.Oper_Name}', oper_type_id = {ConvertOperType(oper)}, oper_category_id = {ConvertOperCategory(oper)}, " +
                                                  $"oper_sum = {oper.Oper_Sum}, oper_completed = {oper.Oper_Completed}, oper_next_date = '{oper.Oper_Next_Date}'" +
                                                  $"WHERE oper_id = {oper.Oper_Id}");
                    }
                    else if (condition.Value == 3)
                        db.Database.ExecuteSqlRaw($"DELETE FROM operations WHERE oper_id = {condition.Key}");
                }

                db.SaveChangesAsync();
                OperCondition.Clear();
                PlanCondition.Clear();
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


    public int ConvertOperType(Operations oper)
        => oper.Type_Name == "Зачисление" ? 1 : 2;

    public int ConvertOperCategory(Operations oper)
        => CategoriesList.Where(x => x.Category_Name == oper.Category_Name).Select(x => x.Category_Id).First();
}
