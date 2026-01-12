namespace Simulation1MPA201.Models;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
    public decimal Price { get; set; }
    public string ImagePath { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
}
