using SQLite;
namespace BeUP.Models;

[Table("Breakfasts")]
public class Breakfast
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Recipe { get; set; }
    public string Image { get; set; }
    public string Category { get; set; }
    [Ignore]
    public List<string> CategoryList 
    {
        get { return Category.Split(',').ToList(); }
    }
    public string Ingredients { get; set; }
    [Ignore]
    public List<string> IngredientsList
    {
        get { return Ingredients.Split(',').ToList(); }
    }
    public int Chosen { get; set; }
    public int Favorite { get; set; }
    public int Own { get; set; }
}