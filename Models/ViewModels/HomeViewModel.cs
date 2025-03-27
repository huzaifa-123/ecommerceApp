using System.Collections.Generic;

namespace ECommerceApp.Models
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public int SelectedCategoryId { get; set; }
    }
}
