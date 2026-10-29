using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.Utils;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace MiniStyleWebService.Repository
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        public Domain.SubCategoryLister GetAll(Domain.SubCategoryLister mLister)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var query = from sub in context.SubCategories
                                join category in context.Categories on sub.CategoryId equals category.Id
                                join at in context.AuditTrails on sub.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = sub.Id,
                                    Name = sub.Name,
                                    Description = sub.Description,
                                    IsActive = sub.IsActive,
                                    CategoryId = sub.CategoryId,
                                    Category = category.Name,
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        if (mLister.SearchCriteria.Name.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(mLister.SearchCriteria.Name.ToUpper()));
                        }

                        if (mLister.SearchCriteria.Category.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => !string.IsNullOrEmpty(x.Category) && x.Category.ToUpper().Contains(mLister.SearchCriteria.Category.ToUpper()));
                        }

                        if (mLister.SearchCriteria.Status.IsNotNullOrEmpty())
                        {
                            if (mLister.SearchCriteria.Status == "Active")
                            {
                                query = query.Where(x => x.IsActive);
                            }
                            else if (mLister.SearchCriteria.Status == "Disabled")
                            {
                                query = query.Where(x => !x.IsActive);
                            }
                        }

                        //if (mLister.SearchCriteria.Category.IsNotNullOrEmpty())
                        //{
                        //       query = query.Where(x => x.Category != null && x.Category.ToUpper().Contains(mLister.SearchCriteria.Category.ToUpper())).AsEnumerable();
                        //}

                    }

                    // Pagination
                    var efCategory = query.AsEnumerable();

                    // Pagination
                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = efCategory.Count();

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0
                            ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take)
                            : 1;

                    efCategory = efCategory.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.SubCategory>>(JsonSerializer.Serialize(efCategory.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.SubCategory Upsert(Domain.SubCategory msubCategory)
        {
            try
            {
                using (var context = new MiniStyleWebService.Persistence.MiniStyleDB())
                {
                    var efSubCategory = context.SubCategories.FirstOrDefault(x => x.Id == msubCategory.Id);
                    if (efSubCategory == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = msubCategory.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efSubCategory = new Persistence.SubCategory
                        {
                            AuditId = audit.Id
                        };
                        context.SubCategories.Add(efSubCategory);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efSubCategory.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = msubCategory.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }
                    
                    efSubCategory.Name = msubCategory.Name;
                    efSubCategory.Description = msubCategory.Description;
                    efSubCategory.IsActive = msubCategory.IsActive;
                    efSubCategory.CategoryId = msubCategory.CategoryId;
                    context.SaveChanges();
                    msubCategory.Id = efSubCategory.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return msubCategory;
        }

        public Domain.SubCategory Get(int id)
        {
            var mSubCategory = new Domain.SubCategory();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efSubCategory = (from sub in context.SubCategories
                                         where sub.Id == id
                                         select new
                                         {
                                             Id = sub.Id,
                                             Name = sub.Name,
                                             Description = sub.Description,
                                             IsActive = sub.IsActive,
                                             AuditId = sub.AuditId,
                                             CategoryId = sub.CategoryId
                                         }).FirstOrDefault();

                    if (efSubCategory != null)
                    {
                        mSubCategory = JsonSerializer.Deserialize<Domain.SubCategory>(JsonSerializer.Serialize(efSubCategory));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mSubCategory;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efSubCategory = context.SubCategories.FirstOrDefault(x => x.Id == id);
                    if (efSubCategory != null)
                    {
                        var audit = context.AuditTrails.Find(efSubCategory.AuditId);
                        if (audit != null)
                        {
                            audit.DeletedBy = loginUserId;
                            audit.DeletedDate = DateTime.Now;
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //SubCategory GetAll
        public List<Domain.SubCategory> GetAllActiveSubCategories()
        {
            var mSubCategories = new List<Domain.SubCategory>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efSubCategories = (from sc in context.SubCategories
                                           join c in context.Categories on sc.CategoryId equals c.Id                      
                                           where sc.IsActive && c.IsActive
                                           select new
                                           {
                                               Id = sc.Id,
                                               Name = sc.Name + " (" + c.Name + ")",
                                           }).AsEnumerable();

                    mSubCategories = JsonSerializer.Deserialize<List<Domain.SubCategory>>(JsonSerializer.Serialize(efSubCategories.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mSubCategories;
        }


        public List<Domain.SubCategory> GetAllActiveSubCategoriesBy(int categoryId)
        {
            var mSubCategories = new List<Domain.SubCategory>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efSubCategories = (from sc in context.SubCategories                                           
                                           where sc.CategoryId == categoryId
                                           select new
                                           {
                                               Id = sc.Id,
                                               Name = sc.Name,
                                           }).AsEnumerable();

                    mSubCategories = JsonSerializer.Deserialize<List<Domain.SubCategory>>(JsonSerializer.Serialize(efSubCategories.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mSubCategories;
        }

    }
}
