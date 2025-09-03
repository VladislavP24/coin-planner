using CoinPlanner.Contracts.DTO.DataServieDTO;

namespace CoinPlanner.Contracts.Abstractions.DataBase
{
    public delegate void WarningEventHandler(object sender, string message);

    public interface IDataService
    {
        event WarningEventHandler OnWarning;
        bool IsDownLoad { get; set; }

        Dictionary<Guid, int> PlanCondition { get; set; }
        Dictionary<Guid, int> OperCondition { get; set; }
        Dictionary<Guid, int> FixCondition { get; set; }
        Dictionary<Guid, int> MarkCondition { get; set; }

        Task<bool> CheckDatabaseConnectionAsync();
        Task<bool> LoadDataFromDatabaseAsync();
        bool ExistsById(Guid id);
        bool SaveDataToDatabaseAsync(Guid planId);

        ICollection<PlansDTO> GetPlanList();
        ICollection<OperationsDTO> GetOperationsList();
        ICollection<CategoriesDTO> GetCategoryList();
        ICollection<FixationsDTO> GetFixationList();
        ICollection<MarksDTO> GetMarkList();

        void AddPlanList(PlansDTO plan);
        void AddOperationsList(OperationsDTO oper);
        void AddCategoryList(CategoriesDTO category);
        void AddFixationList(FixationsDTO fix);
        void AddMarkList(MarksDTO mark);

        void RemovePlanList(PlansDTO plan);
        void RemoveOperationsList(OperationsDTO oper);
        void RemoveCategoryList(CategoriesDTO category);
        void RemoveFixationList(FixationsDTO fix);
        void RemoveMarkList(MarksDTO mark);

        void RemoveAllFixationList(Guid id);
        void RemoveAllMarkList(Guid id);
    }
}
