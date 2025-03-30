using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Model
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // ✅ Navigation Property
        public List<Product>? Products { get; set; }
    }
}
