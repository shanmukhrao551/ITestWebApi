using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITestWebApi.Structure
{
    public class Parameters
    {
        public string ItemName { get; set; }
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; }

    }
}
