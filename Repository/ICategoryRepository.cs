using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITestWebApi.Structure;

namespace ITestWebApi.Repository
{
    public interface ICategoryRepository
    {
        Task<PagedList<UICategory>> GetCategories(Parameters parameters);
        Task<int> DeleteCategory(string categoryName);
    }
}
