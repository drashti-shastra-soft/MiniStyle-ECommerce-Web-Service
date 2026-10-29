using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MiniStyleWebService.Repository
{
    public class ProductRepository : IProductRepository
    {
        public Domain.ProductLister GetAll(Domain.ProductLister mLister)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var query = from p in context.Products
                                join sc in context.SubCategories on p.SubCategoryId equals sc.Id
                                join c in context.Categories on sc.CategoryId equals c.Id
                                join mc in context.MicroCategories on p.MicroCategoryId equals mc.Id into joinMCP
                                from mc in joinMCP.DefaultIfEmpty()
                                join at in context.AuditTrails on p.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Code = p.Code,
                                    ShortInfo = p.ShortInfo,
                                    IsActive = p.IsActive,
                                    SubCategoryId = p.SubCategoryId,
                                    SubCategory = sc.Name,
                                    CategoryId = c.Id,
                                    Category = c.Name,
                                    MicroCategoryId = mc != null ? mc.Id : 0,
                                    MicroCategory = mc != null ? mc.Name : null
                                };

                    // Apply search filters
                    if (mLister.SearchCriteria != null)
                    {
                        if (mLister.SearchCriteria.Name.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(mLister.SearchCriteria.Name.ToUpper()));
                        }

                        if (mLister.SearchCriteria.Category.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.Category != null && x.SubCategory.ToUpper().Contains(mLister.SearchCriteria.Category.ToUpper()));
                        }

                        if (mLister.SearchCriteria.SubCategory.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.SubCategory != null && x.SubCategory.ToUpper().Contains(mLister.SearchCriteria.SubCategory.ToUpper()));
                        }

                        if (mLister.SearchCriteria.MicroCategory.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.MicroCategory != null && x.MicroCategory.ToUpper().Contains(mLister.SearchCriteria.MicroCategory.ToUpper()));
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

                    var efProduct = query.AsEnumerable();

                    // Pagination
                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = efProduct.Count();

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0
                            ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take)
                            : 1;

                    efProduct = efProduct.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Product>>(JsonSerializer.Serialize(efProduct.ToList()));

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.Product Upsert(Domain.Product mProduct)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efProduct = context.Products.FirstOrDefault(x => x.Id == mProduct.Id);
                    if (efProduct == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mProduct.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efProduct = new Persistence.Product
                        {
                            AuditId = audit.Id
                        };
                        context.Products.Add(efProduct);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efProduct.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mProduct.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efProduct.Name = mProduct.Name;
                    efProduct.Code = mProduct.Code;
                    efProduct.ShortInfo = mProduct.ShortInfo;
                    efProduct.SubCategoryId = mProduct.SubCategoryId;
                    efProduct.MicroCategoryId = mProduct.MicroCategoryId;
                    efProduct.IsActive = mProduct.IsActive;
                    context.SaveChanges();
                    mProduct.Id = efProduct.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mProduct;
        }

        public Domain.Product Get(int id)
        {
            var mProduct = new Domain.Product();
            try
            {
                using (var context = new MiniStyleWebService.Persistence.MiniStyleDB())
                {
                    var efProduct = (from p in context.Products
                                     where p.Id == id
                                     select new
                                     {
                                         Id = p.Id,
                                         Name = p.Name,
                                         Code = p.Code,
                                         ShortInfo = p.ShortInfo,
                                         SubCategoryId = p.SubCategoryId,
                                         MicroCategoryId = p.MicroCategoryId,
                                         IsActive = p.IsActive,
                                         AuditId = p.AuditId,
                                     }).FirstOrDefault();

                    if (efProduct != null)
                    {
                        mProduct = JsonSerializer.Deserialize<Domain.Product>(JsonSerializer.Serialize(efProduct));
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return mProduct;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efProduct = context.Products.FirstOrDefault(x => x.Id == id);
                    if (efProduct != null)
                    {
                        var audit = context.AuditTrails.Find(efProduct.AuditId);
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

        //Product All
        public Domain.ProductLister GetAllActiveProducts(Domain.ProductLister mLister)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efProducts = (from p in context.Products
                                      join sc in context.SubCategories on p.SubCategoryId equals sc.Id
                                      join pi in context.ProductImages on p.Id equals pi.ProductId into joinPPI
                                      from pi in joinPPI.DefaultIfEmpty()
                                      join pp in context.ProductPrices on p.Id equals pp.ProductId
                                      where p.IsActive == true
                                      select new
                                      {
                                          Id = p.Id,
                                          Name = p.Name,
                                          Code = p.Code,
                                          ShortInfo = p.ShortInfo,
                                          ImageFileName = pi.FileName,
                                          CategoryId = sc.CategoryId,
                                          SellPrice = pp.SellPrice,
                                          DiscountPercentage = pp.DiscountPercentage
                                      }).AsEnumerable();

                    if (mLister.SearchCriteria.CategoryId > 0)
                        efProducts = efProducts.Where(x => x.CategoryId == mLister.SearchCriteria.CategoryId);

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Product>>(JsonSerializer.Serialize(efProducts.DistinctBy(p => p.Id).ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }
    }
}
