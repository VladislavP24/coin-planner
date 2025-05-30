using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CoinPlanner.DataBase;

public static class DatabaseHelper
{
    private const string AdminConnectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=Gm04stb";
    private const string AppDatabaseName = "MyTestDb";

    public static void EnsureDatabaseCreated()
    {
        // 1. Подключаемся к системной базе postgres
        using (var connection = new NpgsqlConnection(AdminConnectionString))
        {
            connection.Open();

            // 2. Проверяем, существует ли база с нужным именем
            using (var cmd = new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname = '{AppDatabaseName}'", connection))
            {
                var exists = cmd.ExecuteScalar();
                if (exists == null)
                {
                    // 3. Если базы нет — создаём её
                    using (var createCmd = new NpgsqlCommand($"CREATE DATABASE \"{AppDatabaseName}\"", connection))
                    {
                        createCmd.ExecuteNonQuery();
                        Console.WriteLine($"База данных {AppDatabaseName} успешно создана.");
                    }
                }
                else
                {
                    Console.WriteLine($"База данных {AppDatabaseName} уже существует.");
                }
            }
        }

        // 4. Теперь можно использовать EF Core для создания таблиц
        using (var context = new AppDbContext())
        {
            context.Database.Migrate(); // или EnsureCreated(), но Migrate предпочтительнее для продакшена
            Console.WriteLine("Таблицы созданы (если их не было).");
        }
    }
}
