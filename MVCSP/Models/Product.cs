namespace MVCSP.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }  
        public string Description { get; set; }
        public bool IsSold { get; set; }
    }

    public class ProductListModel
    {
        public List<Product> Products { get; set; }
        public int TotalCount { get; set; }

        // Optional: Add these for pagination support
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
    }

}
