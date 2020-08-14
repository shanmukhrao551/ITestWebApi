using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITestWebApi.Repository;
using ITestWebApi.Structure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ITestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ICategoryRepository categoryRepository;
        public CategoriesController(ICategoryRepository _categoryRepository)
        {
            categoryRepository = _categoryRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemName"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ItemsByName")]
        public async Task<IActionResult> ItemsByName([FromQuery] Parameters parameters)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameters.ItemName))
                {
                    if ((parameters.ItemName.Length < 2 || parameters.ItemName.Length > 12))
                    {
                        return BadRequest();
                    }
                }

                var categories = await categoryRepository.GetCategories(parameters);
                if (categories == null)
                {
                    return NotFound();
                }

                var metadata = new
                {
                    categories.TotalCount,
                    categories.PageSize,
                    categories.CurrentPage,
                    categories.TotalPages,
                    categories.HasNext,
                    categories.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(categories);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpDelete]
        [Route("Category")]
        public async Task<IActionResult> Category(string categoryName)
        {
            int result = 0;

            if (string.IsNullOrEmpty(categoryName))
            {
                return BadRequest();
            }

            try
            {
                result = await categoryRepository.DeleteCategory(categoryName);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


    }
}
