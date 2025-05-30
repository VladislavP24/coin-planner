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
    public List<Fixations> FixationsList { get; set; } = new();
    public List<Marks> MarksList { get; set; } = new();

    //Переменные для хранения изменений
    public Dictionary<Guid, int> PlanCondition = new();
    public Dictionary<Guid, int> OperCondition = new();
    public Dictionary<Guid, int> FixCondition = new();
    public Dictionary<Guid, int> MarkCondition = new();

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
                PlansList = await db.plans.FromSqlRaw("SELECT * FROM plans ORDER BY plan_id").ToListAsync();
                MarksList = await db.marks.FromSqlRaw("SELECT * FROM marks ORDER BY mark_id").ToListAsync();
                CategoriesList = await db.categories.FromSqlRaw("SELECT * FROM categories ORDER BY category_id").ToListAsync();
                OperationsList = await db.operations.FromSqlRaw("SELECT * FROM operations ORDER BY oper_id").ToListAsync();
                FixationsList = await db.fixations.FromSqlRaw("SELECT * FROM fixations ORDER BY fix_id").ToListAsync();
                
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Поиск плана по Guid
    /// </summary>
    public bool ExistsById(Guid id)
    {
        using (AppDbContext db = new AppDbContext())
        {
            return db.plans.Any(p => p.Plan_Id == id);
        }
    }


    /// <summary>
    /// Обновление изменений в БД (синхронизация)
    /// </summary>
    public bool SaveDataToDatabaseAsync(Guid planId)
    {
        try
        {
            using (AppDbContext db = new AppDbContext())
            {
                //Сохранение Plans
                foreach (var condition in PlanCondition.Where(x => x.Key == planId))
                {
                    var plan = PlansList.Where(x => x.Plan_Id == condition.Key).First();
                    if (condition.Value == 1)                   
                        db.Database.ExecuteSqlRaw($"INSERT INTO plans (plan_id, plan_name, date_create, date_update) " +
                                                  $"VALUES ('{plan.Plan_Id}', '{plan.Plan_Name}', '{plan.Date_Create}', '{plan.Date_Update}')");
                    else if (condition.Value == 2)
                        db.Database.ExecuteSqlRaw($"UPDATE plans SET plan_name = '{plan.Plan_Name}', date_update = '{plan.Date_Update}' " +
                                                  $"WHERE plan_id = '{plan.Plan_Id}'");
                    else if (condition.Value == 3)
                        db.Database.ExecuteSqlRaw($"DELETE FROM plans WHERE plan_id = {condition.Key}");
                }
                PlanCondition.Clear();

                //Сохранение Operations
                foreach (var condition in OperCondition)
                {
                    var oper = OperationsList.Where(x => x.Oper_Id == condition.Key && x.Oper_Plan_Id == planId).FirstOrDefault();
                    if (oper == null) continue;

                    if (condition.Value == 1)
                        db.Database.ExecuteSqlRaw($"INSERT INTO operations (oper_id, oper_plan_id, oper_name, oper_type_id, oper_category_id, oper_sum, oper_completed, oper_next_date) " +
                                                    $"VALUES ('{oper.Oper_Id}', '{oper.Oper_Plan_Id}', '{oper.Oper_Name}', {oper.Oper_Type_Id}, {oper.Oper_Category_Id}, {oper.Oper_Sum}, {oper.Oper_Completed}, '{oper.Oper_Next_Date}')");
                    else if (condition.Value == 2)
                        db.Database.ExecuteSqlRaw($"UPDATE operations SET oper_name = '{oper.Oper_Name}', oper_type_id = {oper.Oper_Type_Id}, oper_category_id = {oper.Oper_Category_Id}, " +
                                                    $"oper_sum = {oper.Oper_Sum}, oper_completed = {oper.Oper_Completed}, oper_next_date = '{oper.Oper_Next_Date}'" +
                                                    $"WHERE oper_id = '{oper.Oper_Id}'");
                    else if (condition.Value == 3)
                        db.Database.ExecuteSqlRaw($"DELETE FROM operations WHERE oper_id = {condition.Key}");
                }
                OperCondition.Clear();

                //Сохранение Fixations
                foreach (var condition in FixCondition)
                {
                    var fix = FixationsList.Where(x => x.Fix_Id == condition.Key && x.Fix_Plan_Id == planId).FirstOrDefault();
                    if (fix == null) continue;

                    if (condition.Value == 1)
                        db.Database.ExecuteSqlRaw($"INSERT INTO fixations (fix_id, fix_plan_id, fix_name, fix_type_id, fix_category_id, fix_sum, fix_completed, fix_next_date) " +
                                                  $"VALUES ('{fix.Fix_Id}', '{fix.Fix_Plan_Id}', '{fix.Fix_Name}', {fix.Fix_Type_Id}, {fix.Fix_Category_Id}, {fix.Fix_Sum}, {fix.Fix_Completed}, '{fix.Fix_Next_Date}')");
                    else if (condition.Value == 2)
                        db.Database.ExecuteSqlRaw($"UPDATE fixations SET fix_name = '{fix.Fix_Name}', fix_type_id = {fix.Fix_Type_Id}, fix_category_id = {fix.Fix_Category_Id}, " +
                                                  $"fix_sum = {fix.Fix_Sum}, fix_completed = {fix.Fix_Completed}, fix_next_date = '{fix.Fix_Next_Date}'" +
                                                  $"WHERE fix_id = '{fix.Fix_Id}'");
                    else if (condition.Value == 3)
                        db.Database.ExecuteSqlRaw($"DELETE FROM fixations WHERE fix_id = {condition.Key}");
                }
                FixCondition.Clear();

                //Сохранение Fixations
                foreach (var condition in MarkCondition)
                {
                    var mark = MarksList.Where(x => x.Mark_Id == condition.Key && x.Mark_Plan_Id == planId).FirstOrDefault();
                    if (mark == null) continue;

                    if (condition.Value == 1)
                        db.Database.ExecuteSqlRaw($"INSERT INTO marks (mark_id, mark_plan_id, mark_name, mark_date) VALUES ('{mark.Mark_Id}', '{mark.Mark_Plan_Id}', '{mark.Mark_Name}', '{mark.Mark_Date}')");
                    else if (condition.Value == 2)
                        db.Database.ExecuteSqlRaw($"UPDATE marks SET mark_name = '{mark.Mark_Name}', mark_date = '{mark.Mark_Date}' WHERE mark_id = '{mark.Mark_Id}'");
                    else if (condition.Value == 3)
                        db.Database.ExecuteSqlRaw($"DELETE FROM marks WHERE mark_id = {condition.Key}");
                }
                MarkCondition.Clear();

                db.SaveChangesAsync(); 
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
