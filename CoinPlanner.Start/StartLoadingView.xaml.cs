using System.IO;
using System.Windows;
using CoinPlanner.DataBase;
using CoinPlanner.LogService;
using CoinPlanner.UI.View;
using Microsoft.EntityFrameworkCore.Migrations.Operations;


namespace CoinPlanner.Start
{
    /// <summary>
    /// Логика взаимодействия для StartLoadingView.xaml
    /// </summary>
    public partial class StartLoadingView : Window
    {
        public StartLoadingView()
        {
            InitializeComponent();
            Loaded += StartLoadingView_Loaded;
            Loaded += MainWindow_DBLoaded;
            _dataService = new DataService();
        }

        private const string logSender = "Loading Window";
        private DataService _dataService;
        private bool isEnd = false;

        /// <summary>
        /// Событие запуска основного окна и имитация загрузки
        /// </summary>
        private async void StartLoadingView_Loaded(object sender, RoutedEventArgs e)
        {       
            InitLogging();
            Log.Send(EventLevel.Info, logSender, "Начало загрузки окна");
            await SomeLongRunningTask();
            
            Log.Send(EventLevel.Info, logSender, "Открытие главного окна");
            Dispatcher.Invoke(() =>
            {
                MainWindowView mainWindowView = new MainWindowView(_dataService);
                mainWindowView.Show();
                this.Close();
            });
        }

        /// <summary>
        /// Событие загрузки данных с БД
        /// </summary>
        private async void MainWindow_DBLoaded(object sender, RoutedEventArgs e)
        {
            Log.Send(EventLevel.Info, logSender, "Проверка подключения к БД");
            bool isConnected = await _dataService.CheckDatabaseConnectionAsync();

            if (isConnected)
            {
                bool isLoaded = await _dataService.LoadDataFromDatabaseAsync();
                if (!isLoaded)
                    MessageBox.Show("Не удалось загрузить данные из базы данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к базе данных. Проверьте соединение. Работа программы переходит в Offline-режим.", 
                                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            isEnd = true;
        }

        /// <summary>
        /// Имитация работы загрузки
        /// </summary>
        public async Task SomeLongRunningTask()
        {
            for (int i = 0; i <= 100; i++)
            {
                if (isEnd)
                    await Task.Delay(50);
                else
                    await Task.Delay(1000);

                Dispatcher.Invoke(() =>
                {
                    StartProgressBar.Value = i;
                });
            }
        }


        // <summary>
        /// Инициализация логирования
        /// </summary>
        public void InitLogging()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string logPath = Path.Combine(baseDirectory, "LOG");

            Log.LogPatch = logPath;
            Log.LifeTimeLogDays = 30;
            Log.maxsize = 90;
            Log.DebugMessage = true;
            Log.CurUser = "User";
            Log.InitialLog();
            Log.Send(EventLevel.Info, logSender, "Старт " + logSender);
            Log.Send(EventLevel.Info, logSender, "Инициализация логирования");
        }
    }
}
