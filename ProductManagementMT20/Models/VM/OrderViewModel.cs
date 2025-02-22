using ProductManagementMT20.Models.Entities;

namespace ProductManagementMT20.Models.VM
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public List<int> ProductIds { get; set; }
    }
}
