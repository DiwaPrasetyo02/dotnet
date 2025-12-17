using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Url(ErrorMessage = "Gunakan URL gambar yang valid")]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }
    }
}
