using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace CoinPlanner.LogService;

/// <summary>
/// Класс записи событий в файл
/// </summary>
public static class Log
{
    private static Logger log;
    private static readonly object _lock = new();

    public static string LogPatch;
    public static int LifeTimeLogDays;
    public static int maxsize;
    public static bool DebugMessage;
    public static string CurUser;

    static public List<LogMessage> LogBuffer;
    static public bool IsBufferChangedDiagnostic;
    static public bool IsBufferChangedOperational;


    /// <summary>
    /// Инициализация логирования
    /// </summary>
    public static void InitialLog()
    {
        LogBuffer = new List<LogMessage>();
        IsBufferChangedDiagnostic = false;
        IsBufferChangedOperational = false;

        CreateLogger();

        Send(EventLevel.Info, "Logger start", "------------------------------------------------------------------");

        DeleteOldArchiveFiles("Diagnostic");
        DeleteOldArchiveFiles("Operational");
    }

    /// <summary>
    /// Создание логгера от NLog
    /// </summary>
    public static void CreateLogger()
    {
        LoggingConfiguration config = new LoggingConfiguration();                                   // Конфигурация логгера

        // Создание и настройка Target для Diagnostic логов
        FileTarget targetDiagnostic = new FileTarget();
        config.AddTarget("Diagnostic", targetDiagnostic);

        // Создание и настройка Target для Operational логов
        FileTarget targetOperational = new FileTarget();
        config.AddTarget("Operational", targetOperational);

        targetDiagnostic.FileName = LogPatch + "/Diagnostic_${date:format=yyMMdd}_000.txt";          // Diagnostic_141230_000.txt
        targetDiagnostic.MaxArchiveFiles = 999;                                                      // Макс. кол-во архивных файлов
        targetDiagnostic.ArchiveNumbering = ArchiveNumberingMode.Rolling;                            // Способ нумерации архивных файлов
        targetDiagnostic.ArchiveAboveSize = 1048576;                                                 // Размер файла в байтах (76)             
        targetDiagnostic.ArchiveFileName = LogPatch + "/Diagnostic_${date:format=yyMMdd}_{###}.txt"; // Diagnostic_141230_00.txt - шаблон имени архивного файла
        targetDiagnostic.Layout = "${date:format=HH\\:mm\\:ss.fff} |${level:padding=-5} |${message}";// Формат лога
        targetDiagnostic.KeepFileOpen = false;                                                       // Файл не будет оставатья открытым постоянно

        targetOperational.FileName = LogPatch + "/Operational_${date:format=yyMMdd}_000.txt";
        targetOperational.MaxArchiveFiles = 999;
        targetOperational.ArchiveNumbering = ArchiveNumberingMode.Rolling;
        targetOperational.ArchiveAboveSize = 1048576;
        targetOperational.ArchiveFileName = LogPatch + "/Operational_${date:format=yyMMdd}_{###}.txt";
        targetOperational.Layout = "${date:format=HH\\:mm\\:ss.fff} |${level:padding=-5} |${message}";
        targetOperational.KeepFileOpen = false;

        // Создание и добавление правил логирования:
        LoggingRule RuleDiagnostic = new LoggingRule("*", LogLevel.Trace, targetDiagnostic);         // Diagnostic - Отправляем от Trace
        config.LoggingRules.Add(RuleDiagnostic);
        LoggingRule RuleOperational = new LoggingRule("*", LogLevel.Warn, targetOperational);        // Operation - Отправляем от Warn
        config.LoggingRules.Add(RuleOperational);

        // Применение конфигурации и получение логгера
        LogManager.Configuration = config;                                                           // Применяем созданную конфигурацию к LogManager
        log = LogManager.GetLogger("MyLogger");                                                      // Получаем логгер с именем "MyLogger"
    }


    /// <summary>
    /// Функция отправки сообщений
    /// </summary>
    public static void Send(EventLevel eventLevel, string eventPoint, string message)
    {
        string logString;

        if ((eventLevel == EventLevel.Trace || eventLevel == EventLevel.Debug) && !DebugMessage)
            return;

        eventPoint += ":";
        logString = eventPoint.PadRight(25, ' ') + " |" + CurUser.PadRight(15, ' ') + "|" + message;

        switch (eventLevel)
        {
            case EventLevel.Trace: { log.Trace(logString); break; }
            case EventLevel.Debug: { log.Debug(logString); break; }
            case EventLevel.Info:  { log.Info(logString); break;  }
            case EventLevel.Warn:  { log.Warn(logString); break;  }
            case EventLevel.Error: { log.Error(logString); break; }
            case EventLevel.Fatal: { log.Fatal(logString); break; }
            default: break;
        }

        AddLogBuffer(eventLevel, eventPoint, message);
    }


    /// <summary>
    /// Удаление старых логов согласно LifeTimeLogDays
    /// </summary>
    private static void DeleteOldArchiveFiles(string filename)
    {
        // Првоерка на имя файла
        if (string.IsNullOrEmpty(filename))
        {
            Send(EventLevel.Warn, "Logger warning", "Filename cannot be null or empty.");
            return;
        }

        string oldTime = (DateTime.Now - new TimeSpan(LifeTimeLogDays, 0, 0, 0, 0)).ToString("yyMMdd", CultureInfo.InvariantCulture);

        // Проверка на существование данной директорри
        if (!Directory.Exists(LogPatch))
        {
            Send(EventLevel.Warn, "Logger warning", $"Directory {LogPatch} does not exist.");
            return;
        }

        // Получение информации о директории и файлах логов
        DirectoryInfo dirinfo = new DirectoryInfo(LogPatch);
        FileInfo[] fileinfo = dirinfo.GetFiles(filename + "_*.txt");

        // Проверка на наличие файлов
        if (fileinfo.Length <= 0)
            return;

        // Определение шаблона имени файла и создание объекта Regex
        string pattern = filename + @"_(\d{6})_\d{3}.txt";
        Regex r = new Regex(pattern);

        // Удаление старых файлов
        for (int i = 0; i < fileinfo.Length; i++)
        {
            try
            {
                Match m = r.Match(fileinfo[i].Name);
                if (m.Success && Int32.Parse(m.Groups[1].Value.ToString()) < Int32.Parse(oldTime))
                {
                    fileinfo[i].Delete();
                    Send(EventLevel.Debug, "Logger info", "Delete oldArchive: " + fileinfo[i].Name);
                }
            }
            catch (Exception ex)
            {
                Send(EventLevel.Error, "Logger error", $"Error deleting file {fileinfo[i].Name}: {ex.Message}");
            }
        }
    }



    /// <summary>
    /// Добавляем сообщения в буфер List<LogMessage> LogBuffer для мониторинга (maxsize)
    /// </summary>
    static public void AddLogBuffer(EventLevel eventlevel, string eventpoint, string message)
    {
        // Форматирование сообщения
        string today = DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
        string outstr = today + " |" + eventlevel.ToString().PadRight(7) + " |" + eventpoint.PadRight(20) + "\t|" + message;

        // Для потокобезопасности
        lock (_lock)
        {
            // Добавление сообщения в буфер
            LogBuffer.Insert(0, new LogMessage(eventlevel, eventpoint, outstr));

            // Ограничение размера буфера
            if (LogBuffer.Count > maxsize)
                LogBuffer.RemoveAt(LogBuffer.Count - 1);
        }

        // Установка флагов изменения буфера
        if (!IsBufferChangedDiagnostic)
            IsBufferChangedDiagnostic = true;

        if ((int)eventlevel <= 2)
        {
            if (!IsBufferChangedOperational)
                IsBufferChangedOperational = true;
        }
    }
}
