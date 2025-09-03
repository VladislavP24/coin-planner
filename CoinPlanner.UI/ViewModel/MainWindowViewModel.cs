using CoinPlanner.Contracts.Abstractions.DataBase;
using CoinPlanner.Contracts.Abstractions.ViewModel;
using CoinPlanner.Contracts.Abstractions.ViewModel.Factory;
using CoinPlanner.Contracts.DTO.DataServieDTO;
using CoinPlanner.UI.ViewModel.Controls;

namespace CoinPlanner.UI.ViewModel;

public class MainWindowViewModel : IMainWindowViewModel
{
    public MainWindowViewModel(IDataService dataService, IDialogFactory dialogFactory)
    {
        // Определение ViewModel`ей
        DiagramViewModel = new DiagramViewModel(dataService);
        ContentViewModel = new ContentViewModel(dataService);
        CalendarViewModel = new CalendarViewModel(dataService);
        PanelViewModel = new PanelViewModel(dataService, ContentViewModel, CalendarViewModel, dialogFactory);

        InitializeComponent();
    }

    public CalendarViewModel CalendarViewModel { get; }
    public PanelViewModel PanelViewModel { get; }
    public ContentViewModel ContentViewModel { get; }
    public DiagramViewModel DiagramViewModel { get; }

    private void InitializeComponent()
    {
        ContentViewModel.OnCreateDiagram += ContentViewModel_OnCreateDiagram;
        CalendarViewModel.OnUpdateAndCreate += CalendarViewModel_OnUpdateAndCreate;
        PanelViewModel.OnUpdateAndCreate += PanelViewModel_OnUpdateAndCreate;
        PanelViewModel.OnChangeType += PanelViewModel_OnChangeType;
        PanelViewModel.OnVisibleContent += PanelViewModel_OnVisibleContent;
    }

    private void PanelViewModel_OnVisibleContent(object? sender, bool isVisible)
    {
        ContentViewModel.IsVisibleContent = isVisible;
        DiagramViewModel.IsVisibleDiagram = !isVisible;
    }

    private void PanelViewModel_OnChangeType(object? sender, string type)
    {
        ContentViewModel.IsType = type;
        ContentViewModel.UpdateOperation();
    }

    private void PanelViewModel_OnUpdateAndCreate(object sender, PlansDTO planModel)
    {
        ContentViewModel.Plan = planModel;
        CalendarViewModel.PlanId = planModel.Plan_Id;
        DiagramViewModel.CreatDiagram(PanelViewModel.SelectedItemPlan.Plan_Id);
        ContentViewModel.UpdateOperation();
    }

    private void CalendarViewModel_OnUpdateAndCreate(object sender, DateTime start, DateTime? end, Guid guid)
    {
        ContentViewModel.StartDate = start;
        ContentViewModel.EndDate = end;
        ContentViewModel.UpdateOperation();

        DiagramViewModel.Start = start;
        DiagramViewModel.End = end;
        DiagramViewModel.CreatDiagram(guid);
    }

    private void ContentViewModel_OnCreateDiagram(object sender, Guid guid) => DiagramViewModel.CreatDiagram(guid);
}
