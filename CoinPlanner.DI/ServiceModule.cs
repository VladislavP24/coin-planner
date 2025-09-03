using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel;
using CoinPlanner.Contracts.Abstractions.ViewModel.Controls;
using CoinPlanner.Contracts.Abstractions.ViewModel.Factory;
using CoinPlanner.DataBase;
using CoinPlanner.UI.View;
using CoinPlanner.UI.ViewModel;
using CoinPlanner.UI.ViewModel.Controls;
using CoinPlanner.UI.ViewModel.Factory;
using Microsoft.EntityFrameworkCore.Metadata;
using Ninject;
using Ninject.Parameters;

namespace CoinPlanner.DI
{
    public class ServiceModule
    {
        public static ServiceModule GetInstance() => lazy.Value;
        private static readonly Lazy<ServiceModule> lazy = new(() => new ServiceModule());

        private readonly StandardKernel kernel;

        public ServiceModule()
        {
            kernel = new StandardKernel();

            // DataService
            kernel.Bind<IDataService>().To<DataService>().InSingletonScope();

            // MainWindow
            kernel.Bind<IMainWindowViewModel>().To<MainWindowViewModel>().InSingletonScope();

            // Factory
            kernel.Bind<IDialogFactory>().To<DialogFactory>().InSingletonScope();

            //View
            kernel.Bind<Contracts.Abstractions.View.IView>().To<MainWindowView>();
        }

        public Contracts.Abstractions.View.IView View(IDataService dataService) 
            => kernel.Get<Contracts.Abstractions.View.IView>(new ConstructorArgument[]{ new("mainWindowViewModel", MainWindowViewModel(dataService))});


        private IMainWindowViewModel? _mainWindowViewModel;
        public IMainWindowViewModel MainWindowViewModel(IDataService dataService) 
            => _mainWindowViewModel ??= kernel.Get<IMainWindowViewModel>(new ConstructorArgument[]
            { 
                new("dataService", dataService),
                new("dialogFactory", DialogFactory()),
            });

        private IDialogFactory _dialogFactory;
        public IDialogFactory DialogFactory()
            => _dialogFactory ??= kernel.Get<IDialogFactory>(new ConstructorArgument[]{ });
    }
}
