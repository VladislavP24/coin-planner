using System.ComponentModel.DataAnnotations;

namespace CoinPlanner.DataBase.ModelsDb;

public class Categories
{
    [Key]
    public int Category_Id { get; set; }
    public string? Category_Name { get; set; }
}
