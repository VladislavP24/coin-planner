using System.Windows;
using CoinPlanner.UI;
using CoinPlanner.UI.View;


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
        }

        private async void StartLoadingView_Loaded(object sender, RoutedEventArgs e)
        {
            await SomeLongRunningTask();

            // После завершения задачи, открываем главное окно и закрываем текущее.
            Dispatcher.Invoke(() =>
            {
                MainWindowView mainWindowView = new MainWindowView();
                mainWindowView.Show();
                this.Close();
            });
        }


        /// <summary>
        /// Имитация работы загрузки
        /// </summary>
        /// <returns></returns>
        public async Task SomeLongRunningTask()
        {
            for (int i = 0; i <= 100; i++)
            {
                await Task.Delay(50);

                Dispatcher.Invoke(() =>
                {
                    StartProgressBar.Value = i;
                });
            }
        }
    }
}
