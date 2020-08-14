using ITestWebApi.DataAcess;
using ITestWebApi.Structure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITestWebApi.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        EMARTContext db;
        public CategoryRepository(EMARTContext _db)
        {
            db = _db;
        }

        public async Task<PagedList<UICategory>> GetCategories(Parameters parameters)
        {
            if (db != null)
            {
                List<UICategory> categorylist;
                if (string.IsNullOrEmpty(parameters.ItemName))
                {


                    categorylist = await (from cat in db.Category
                                          join subcat in db.SubCategory on cat.Id equals subcat.CategoryId
                                          join items in db.Item on subcat.Id equals items.SubCategoryId
                                          select new UICategory
                                          {
                                              CategoryName = cat.Name,
                                              SubCategoryName = subcat.Name,
                                              ItemName = items.Name,
                                              ItemDescription = items.Description
                                          }).ToListAsync();
                }
                else
                {
                    categorylist = await (from cat in db.Category
                                          join subcat in db.SubCategory on cat.Id equals subcat.CategoryId
                                          join items in db.Item on subcat.Id equals items.SubCategoryId
                                          where items.Name.Contains(parameters.ItemName)
                                          select new UICategory
                                          {
                                              CategoryName = cat.Name,
                                              SubCategoryName = subcat.Name,
                                              ItemName = items.Name,
                                              ItemDescription = items.Description
                                          }).ToListAsync();
                }
                return PagedList<UICategory>.ToPagedList(categorylist, parameters.PageNumber, parameters.PageSize);
            }

            return null;
        }

        public async Task<int> DeleteCategory(string categoryName)
        {
            int result = 0;

            //Find the post for specific category id
            var category = await db.Category.FirstOrDefaultAsync(x => x.Name == categoryName);

            if (category != null)
            {
                var subCategoryList = db.SubCategory.Where(x => x.CategoryId == category.Id);
                var ItemList = db.Item.Where(c => subCategoryList.Any(c2 => c2.Id == c.SubCategoryId));
                //Delete Items
                if (ItemList.Count() > 0)
                    db.Item.RemoveRange(ItemList);

                if (subCategoryList.Count() > 0)
                    db.SubCategory.RemoveRange(subCategoryList);

                //Delete that category
                db.Category.Remove(category);

                //Commit the transaction
                result = await db.SaveChangesAsync();
            }
            return result;
        }

    }
}