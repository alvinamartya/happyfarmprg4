using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class CategoryRepository
    {
        /// <summary>
        /// Delete Category Repository
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCategory(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var category = db.Categories.Where(x => x.Id == id).FirstOrDefault();
                category.RowStatus = "D";
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Edit Category Repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryRequest"></param>
        public void EditCategory(int id, EditCategoryRequest categoryRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var category = db.Categories.Where(x => x.Id == id).FirstOrDefault();
                category.Name = categoryRequest.Name;
                category.ModifiedAt = DateTime.Now;
                category.ModifiedBy = categoryRequest.ModifiedBy;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Add Category Repository
        /// </summary>
        /// <param name="categoryRequest"></param>
        public void AddCategory(AddCategoryRequest categoryRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {

                // get last id user login
                int lastUserLoginId = db.UserLogins
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault()
                    .Id;

                // create new category
                Category newCategory = new Category()
                {
                    Name = categoryRequest.Name,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    CreatedBy = categoryRequest.CreatedBy,
                    ModifiedBy = categoryRequest.CreatedBy,
                    RowStatus = "A"
                };

                db.Categories.Add(newCategory);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get category with paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ResponsePagingModel<List<Category>> GetCategories(int currentPage, int limitPage, string search)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get category
                var category = search == null ? db.Categories
                    .OrderBy(x => x.Id)
                    .Skip((currentPage - 1) * limitPage)
                    .Take(limitPage)
                    .ToList() : db.Categories
                    .Where(x =>
                        x.Name.ToLower().Contains(search.ToLower())
                    )
                    .OrderBy(x => x.Id)
                    .Skip((currentPage - 1) * limitPage)
                    .Take(limitPage)
                    .ToList();

                // get total category
                var totalPages = Math.Ceiling((decimal)db.Categories.Count() / limitPage);

                // return category
                return new ResponsePagingModel<List<Category>>()
                {
                    Data = category,
                    CurrentPage = currentPage,
                    TotalPage = (int)totalPages
                };
            }
        }

        /// <summary>
        /// Get Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetCategoryById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var categories = db.Categories
                    .Where(x => x.Id == id)
                     .Select(x => new
                     {
                         x.Id,
                         x.Name
                     })
                    .FirstOrDefault();
                return categories;
            }
        }
    }
}