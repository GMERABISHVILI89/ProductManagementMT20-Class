using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementMT20.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public List<Product> Products { get; set; }
    }
}
