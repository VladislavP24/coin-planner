using System.Windows;
using CoinPlanner.DataBase;
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
            _dbProcessing = new DataService();

        }

        private DataService _dbProcessing;
        private bool isEnd = false;

        /// <summary>
        /// Событие запуска основного окна и имитация загрузки
        /// </summary>
        private async void StartLoadingView_Loaded(object sender, RoutedEventArgs e)
        {
            await SomeLongRunningTask();

            Dispatcher.Invoke(() =>
            {
                MainWindowView mainWindowView = new MainWindowView(_dbProcessing);
                mainWindowView.Show();
                this.Close();
            });
        }

        /// <summary>
        /// Событие загрузки данных с БД
        /// </summary>
        private async void MainWindow_DBLoaded(object sender, RoutedEventArgs e)
        {
            bool isConnected = await _dbProcessing.CheckDatabaseConnectionAsync();

            if (isConnected)
            {
                bool isLoaded = await _dbProcessing.LoadDataFromDatabaseAsync();
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

    }
}
