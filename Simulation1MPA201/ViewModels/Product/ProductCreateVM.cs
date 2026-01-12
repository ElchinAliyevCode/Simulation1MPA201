using System.ComponentModel.DataAnnotations;

namespace Simulation1MPA201.ViewModels.Product;

public class ProductCreateVM
{
    [Required, MaxLength(256)]
    public string Title { get; set; }
    [Required, MaxLength(512)]
    public string Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required, Range(0, 5)]
    public int Rating { get; set; }
    [Required]
    public IFormFile Image { get; set; }
    [Required]
    public int CategoryId { get; set; }

}
