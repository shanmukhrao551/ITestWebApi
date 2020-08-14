using System;
using System.Collections.Generic;

namespace ITestWebApi.DataAcess
{
    public partial class Item
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual SubCategory SubCategory { get; set; }
    }
}
