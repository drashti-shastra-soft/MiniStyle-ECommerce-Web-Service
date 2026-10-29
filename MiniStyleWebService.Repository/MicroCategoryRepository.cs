using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.Utils;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace MiniStyleWebService.Repository
{
    public class MicroCategoryRepository : IMicroCategoryRepository
    {
        public Domain.MicroCategoryLister GetAll(Domain.MicroCategoryLister mLister)
        {
            try
            {
                using (var context = new MiniStyleWebService.Persistence.MiniStyleDB())
                {
                    var query = from mc in context.MicroCategories
                                join subcategory in context.SubCategories on mc.SubCategoryId equals subcategory.Id
                                join at in context.AuditTrails on mc.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = mc.Id,
                                    Name = mc.Name,
                                    Description = mc.Description,
                                    SubCategoryId = mc.SubCategoryId,
                                    IsActive = mc.IsActive,
                                    SubCategory = subcategory.Name
                                };

                    // Apply search filters
                    if (mLister.SearchCriteria != null)
                    {
                        if (mLister.SearchCriteria.Name.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(mLister.SearchCriteria.Name.ToUpper()));
                        }

                        if (mLister.SearchCriteria.SubCategory.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.SubCategory != null && x.SubCategory.ToUpper().Contains(mLister.SearchCriteria.SubCategory.ToUpper()));
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

                    }

                    // Pagination
                    var efMc = query.AsEnumerable();

                    // Pagination
                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = efMc.Count();

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0
                            ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take)
                            : 1;

                    efMc = efMc.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.MicroCategory>>(JsonSerializer.Serialize(efMc.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mLister;
        }

        public Domain.MicroCategory Upsert(Domain.MicroCategory mMicroCategory)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efMicroCategory = context.MicroCategories.FirstOrDefault(x => x.Id == mMicroCategory.Id);
                    if (efMicroCategory == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mMicroCategory.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efMicroCategory = new Persistence.MicroCategory
                        {
                            AuditId = audit.Id
                        };
                        context.MicroCategories.Add(efMicroCategory);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efMicroCategory.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mMicroCategory.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efMicroCategory.Name = mMicroCategory.Name;
                    efMicroCategory.Description = mMicroCategory.Description;
                    efMicroCategory.IsActive = mMicroCategory.IsActive;
                    efMicroCategory.SubCategoryId = mMicroCategory.SubCategoryId;

                    context.SaveChanges();

                    mMicroCategory.Id = efMicroCategory.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mMicroCategory;
        }

        public Domain.MicroCategory Get(int id)
        {
            var mMicroCategory = new Domain.MicroCategory();
            try
            {
                using (var context = new MiniStyleWebService.Persistence.MiniStyleDB())
                {
                    var efMicroCategory = (from mc in context.MicroCategories
                                           where mc.Id == id
                                           select new
                                           {
                                               Id = mc.Id,
                                               Name = mc.Name,
                                               Description = mc.Description,
                                               IsActive = mc.IsActive,
                                               SubCategoryId = mc.SubCategoryId,
                                               AuditId = mc.AuditId,
                                           }).FirstOrDefault();

                    if (efMicroCategory != null)
                    {
                        mMicroCategory = JsonSerializer.Deserialize<Domain.MicroCategory>(JsonSerializer.Serialize(efMicroCategory));
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return mMicroCategory;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new MiniStyleWebService.Persistence.MiniStyleDB())
                {
                    var efMicroCategory = context.MicroCategories.FirstOrDefault(x => x.Id == id);
                    if (efMicroCategory != null)
                    {
                        var audit = context.AuditTrails.Find(efMicroCategory.AuditId);
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

        public List<Domain.MicroCategory> GetAllActiveMicroCategories()
        {
            var mMicroCategorys = new List<Domain.MicroCategory>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efMicroCategories = (from mc in context.MicroCategories
                                             where mc.IsActive == true
                                             select new
                                             {
                                                 Id = mc.Id,
                                                 Name = mc.Name
                                             }).AsEnumerable();

                    mMicroCategorys = JsonSerializer.Deserialize<List<Domain.MicroCategory>>(JsonSerializer.Serialize(efMicroCategories.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mMicroCategorys;
        }


    }
}
