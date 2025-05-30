using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinPlanner.DataBase.ModelsDb;

[Table("categories")]
public class Categories
{
    [Key]
    [Column("category_id")]
    public int Category_Id { get; set; }

    [Column("category_name")]
    public string? Category_Name { get; set; }
}
