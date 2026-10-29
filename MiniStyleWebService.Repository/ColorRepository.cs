using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.Utils;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace MiniStyleWebService.Repository
{
    public class ColorRepository : IColorRepository
    {
        public Domain.ColorLister GetAll(Domain.ColorLister mLister)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var query = from c in context.Colors
                                join at in context.AuditTrails on c.AuditId equals at.Id
                                where at.DeletedDate == null && at.DeletedBy == null
                                select new
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    HashCode = c.HashCode,
                                    IsActive = c.IsActive,
                                    AuditId = c.AuditId
                                };

                    //Apply search filters
                    if (mLister.SearchCriteria != null)
                    {
                        if (mLister.SearchCriteria.Name.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(mLister.SearchCriteria.Name.ToUpper()));
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
                    var efColor = query.AsEnumerable();

                    // Pagination
                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = efColor.Count();

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0
                            ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take)
                            : 1;

                    efColor = efColor.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Color>>(JsonSerializer.Serialize(efColor.ToList()));
                }

            }
            catch (Exception)
            {
                throw;
            }

            return mLister;
        }

        public Domain.Color Upsert(Domain.Color mColor)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efColor = context.Colors.FirstOrDefault(x => x.Id == mColor.Id);
                    if (efColor == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mColor.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efColor = new Persistence.Color
                        {
                            AuditId = audit.Id
                        };
                        context.Colors.Add(efColor);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efColor.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mColor.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efColor.Name = mColor.Name;
                    efColor.HashCode = mColor.HashCode;
                    efColor.IsActive = mColor.IsActive;
                    context.SaveChanges();
                    mColor.Id = efColor.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mColor;
        }

        public Domain.Color Get(int id)
        {
            var mColor = new Domain.Color();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efColor = (from c in context.Colors
                                   where c.Id == id
                                   select new
                                   {
                                       Id = c.Id,
                                       Name = c.Name,
                                       HashCode = c.HashCode,
                                       IsActive = c.IsActive,
                                       AuditId = c.AuditId
                                   }).FirstOrDefault();

                    if (efColor != null)
                    {
                        mColor = JsonSerializer.Deserialize<Domain.Color>(JsonSerializer.Serialize(efColor));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mColor;
        }

        public List<Domain.Color> GetAllActiveColor()
        {
            var mColor = new List<Domain.Color>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efColor = (from c in context.Colors
                                   where c.IsActive
                                   select new
                                   {
                                       Id = c.Id,
                                       Name = c.Name,
                                       HashCode = c.HashCode,
                                   }).AsEnumerable();

                    mColor = JsonSerializer.Deserialize<List<Domain.Color>>(JsonSerializer.Serialize(efColor.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mColor;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efColor = context.Colors.FirstOrDefault(x => x.Id == id);
                    if (efColor != null)
                    {
                        var audit = context.AuditTrails.Find(efColor.AuditId);
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
    }
}
