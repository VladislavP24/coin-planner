using CoinPlanner.DataBase.ModelsDb;
using CoinPlanner.DataBase.ModelsDB;
using CoinPlanner.LogService;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;

namespace CoinPlanner.DataBase;

public class DataService
{
    public DataService() {}

    public delegate void WarningEventHandler(object sender, string message);
    public event WarningEventHandler OnWarning;
    public bool IsDownLoad { get; set; }
    public static string logSender = "Data Service";

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
                Log.Send(EventLevel.Info, logSender, "Установлено подключение к БД");
                return true;
            }
        }
        catch (Exception ex)
        {
            Log.Send(EventLevel.Error, logSender, "Подключение не установлено к БД. ");
            Log.Send(EventLevel.Trace, logSender, ex.ToString());
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
            List<Plans> planDBList = new();
            List<Marks> markDBList = new();
            List<Operations> operDBList = new();
            List<Fixations> fixDBList = new();

            Log.Send(EventLevel.Info, logSender, "Запуск получения данных с БД");

            using (AppDbContext db = new AppDbContext())
            {
                CategoriesList = await db.categories.FromSqlRaw("SELECT * FROM categories ORDER BY category_id").ToListAsync();
                planDBList = await db.plans.FromSqlRaw("SELECT * FROM plans ORDER BY plan_id").ToListAsync();
                markDBList = await db.marks.FromSqlRaw("SELECT * FROM marks ORDER BY mark_id").ToListAsync();               
                operDBList = await db.Database.SqlQueryRaw<Operations>("SELECT o.oper_id, t.type_name AS type_name, ct.category_name AS category_name, o.oper_name, o.oper_sum, o.oper_completed, o.oper_next_date, o.oper_plan_id " +
                                                                       "FROM operations o " +
                                                                       "JOIN plans p ON o.oper_plan_id = p.plan_id " +
                                                                       "JOIN type_operations t ON o.oper_type_id = t.type_id " +
                                                                       "JOIN categories ct ON o.oper_category_id = ct.category_id " +
                                                                       "ORDER BY o.oper_id;").ToListAsync();

                fixDBList = await db.Database.SqlQueryRaw<Fixations>("SELECT f.fix_id, t.type_name AS type_name, ct.category_name AS category_name, f.fix_name, f.fix_sum, f.fix_completed, f.fix_next_date, f.fix_plan_id " +
                                                                     "FROM fixations f " +
                                                                     "JOIN plans p ON f.fix_plan_id = p.plan_id " +
                                                                     "JOIN type_operations t ON f.fix_type_id = t.type_id " +
                                                                     "JOIN categories ct ON f.fix_category_id = ct.category_id " +
                                                                     "ORDER BY f.fix_id;").ToListAsync();
            }
            Log.Send(EventLevel.Info, logSender, "Данные получены с БД");

            foreach (var plan in planDBList)
            {
                if (PlansList.Any(x => x.Plan_Id == plan.Plan_Id && x.Date_Update == plan.Date_Update))
                    continue;
                else if (!PlansList.Any(x => x.Plan_Id == plan.Plan_Id))
                    PlansList.Add(plan);
                else
                {
                    OnWarning?.Invoke(this, $"Информация о загружаемом плане:\nID: {plan.Plan_Id}\nИмя: {plan.Plan_Name}\nДата создания: {plan.Date_Create}\nДата изменения: {plan.Date_Update}");
                    if(IsDownLoad)
                    {
                        PlansList.RemoveAll(x => x.Plan_Id == plan.Plan_Id);
                        PlansList.Add(plan);

                        MarksList.RemoveAll(x => x.Mark_Plan_Id == plan.Plan_Id);
                        MarksList.AddRange(markDBList.Where(x => x.Mark_Plan_Id == plan.Plan_Id));

                        OperationsList.RemoveAll(x => x.Oper_Plan_Id == plan.Plan_Id);
                        OperationsList.AddRange(operDBList.Where(x => x.Oper_Plan_Id == plan.Plan_Id));

                        FixationsList.RemoveAll(x => x.Fix_Plan_Id == plan.Plan_Id);
                        FixationsList.AddRange(fixDBList.Where(x => x.Fix_Plan_Id == plan.Plan_Id));
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Log.Send(EventLevel.Error, logSender, "Получение данных не произошло. ");
            Log.Send(EventLevel.Trace, logSender, ex.ToString());
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
                                                    $"VALUES ('{oper.Oper_Id}', '{oper.Oper_Plan_Id}', '{oper.Oper_Name}', {ConvertOperType(oper)}, {ConvertOperCategory(oper)}, {oper.Oper_Sum}, {oper.Oper_Completed}, '{oper.Oper_Next_Date}')");
                    else if (condition.Value == 2)
                        db.Database.ExecuteSqlRaw($"UPDATE operations SET oper_name = '{oper.Oper_Name}', oper_type_id = {ConvertOperType(oper)}, oper_category_id = {ConvertOperCategory(oper)}, " +
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
                                                  $"VALUES ('{fix.Fix_Id}', '{fix.Fix_Plan_Id}', '{fix.Fix_Name}', {ConvertFixType(fix)}, {ConvertFixCategory(fix)}, {fix.Fix_Sum}, {fix.Fix_Completed}, '{fix.Fix_Next_Date}')");
                    else if (condition.Value == 2)
                        db.Database.ExecuteSqlRaw($"UPDATE fixations SET fix_name = '{fix.Fix_Name}', fix_type_id = {ConvertFixType(fix)}, fix_category_id = {ConvertFixCategory(fix)}, " +
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


    public int ConvertOperType(Operations oper)
        => oper.Type_Name == "Зачисление" ? 1 : 2;
    public int ConvertFixType(Fixations fix)
        => fix.Type_Name == "Зачисление" ? 1 : 2;

    public int ConvertOperCategory(Operations oper)
        => CategoriesList.Where(x => x.Category_Name == oper.Category_Name).Select(x => x.Category_Id).First();

    public int ConvertFixCategory(Fixations fix)
        => CategoriesList.Where(x => x.Category_Name == fix.Category_Name).Select(x => x.Category_Id).First();
}
