namespace CoinPlanner.FileService
{
    public static class TxtExportHelper
    {
        public static void ExportToTxt(DataCollection data, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                // Заголовок
                writer.WriteLine("Информация о плане:");
                writer.WriteLine($"ID: {data.Plan.PlanId}");
                writer.WriteLine($"Наименование: {data.Plan.PlanName}");
                writer.WriteLine($"Дата создания: {data.Plan.DataCreate}");
                writer.WriteLine($"Дата изменения: {data.Plan.DataUpdate}");
                writer.WriteLine(new string('-', 50));

                // Операции
                writer.WriteLine("Информация об операциях:");
                foreach (var op in data.Operations)
                {
                    writer.WriteLine($"ID: {op.OperId}");
                    writer.WriteLine($"Наименование: {op.OperName}");
                    writer.WriteLine($"Тип: {op.OperType}");
                    writer.WriteLine($"Категория: {op.OperCategory}");
                    writer.WriteLine($"Сумма: {op.OperSum}");
                    writer.WriteLine($"Выполнен: {(op.OperCompleted == true ? "Да" : "Нет")}");
                    writer.WriteLine($"Дата изменения: {op.OperNextDate}");
                    writer.WriteLine(new string('-', 30));
                }

                // Фиксации
                writer.WriteLine("Информация о фиксациях:");
                foreach (var fixation in data.Fixations)
                {
                    writer.WriteLine($"ID: {fixation.FixId}");
                    writer.WriteLine($"Имя: {fixation.FixName}");
                    writer.WriteLine($"Тип: {fixation.FixType}");
                    writer.WriteLine($"Категория: {fixation.FixCategory}");
                    writer.WriteLine($"Сумма: {fixation.FixSum}");
                    writer.WriteLine($"Выполнен: {(fixation.FixCompleted == true ? "Да" : "Нет")}");
                    writer.WriteLine($"Дата изменения: {fixation.FixNextDate.ToShortDateString()}");
                    writer.WriteLine(new string('-', 30));
                }

                // Марки
                writer.WriteLine("Информация об отметках:");
                foreach (var mark in data.Marks)
                {
                    writer.WriteLine($"ID: {mark.MarkId}");
                    writer.WriteLine($"Описание: {mark.MarkName}");
                    writer.WriteLine($"Дата: {mark.MarkDate.ToShortDateString()}");
                    writer.WriteLine(new string('-', 30));
                }
            }
        }
    }
}
