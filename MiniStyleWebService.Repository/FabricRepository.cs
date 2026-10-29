using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MiniStyleWebService.Repository
{
    public class FabricRepository : IFabricRepository
    {
        public Domain.FabricLister GetAll(Domain.FabricLister mLister)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var query = from f in context.Fabrics
                                join at in context.AuditTrails on f.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = f.Id,
                                    Name = f.Name,
                                    IsActive = f.IsActive,
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
                    var efFabric = query.AsEnumerable();

                    // Pagination
                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = efFabric.Count();

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0
                            ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take)
                            : 1;

                    efFabric = efFabric.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Fabric>>(JsonSerializer.Serialize(efFabric.ToList()));
                }

            }
            catch (Exception)
            {
                throw;
            }

            return mLister;
        }

        public Domain.Fabric Upsert(Domain.Fabric mFabric)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efFabric = context.Fabrics.FirstOrDefault(x => x.Id == mFabric.Id);
                    if (efFabric == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mFabric.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();
                        efFabric = new Persistence.Fabric
                        {
                            AuditId = audit.Id
                        };
                        context.Fabrics.Add(efFabric);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efFabric.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mFabric.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }


                    efFabric.Name = mFabric.Name;
                    efFabric.IsActive = mFabric.IsActive;
                    context.SaveChanges();
                    mFabric.Id = efFabric.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mFabric;
        }

        public Domain.Fabric Get(int id)
        {
            var mFabric = new Domain.Fabric();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efFabric = (from f in context.Fabrics
                                    where f.Id == id
                                    select new
                                    {
                                        Id = f.Id,
                                        Name = f.Name,
                                        IsActive = f.IsActive,
                                        AuditId = f.AuditId,
                                    }).FirstOrDefault();

                    if (efFabric != null)
                    {
                        mFabric = JsonSerializer.Deserialize<Domain.Fabric>(JsonSerializer.Serialize(efFabric));
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return mFabric;
        }

        public List<Domain.Fabric> GetAllActiveFabric()
        {
            var mFabric = new List<Domain.Fabric>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efFabric = (from f in context.Fabrics
                                    where f.IsActive
                                    select new
                                    {
                                        Id = f.Id,
                                        Name = f.Name,
                                    }).AsEnumerable();

                    mFabric = JsonSerializer.Deserialize<List<Domain.Fabric>>(JsonSerializer.Serialize(efFabric.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mFabric;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efFabric = context.Fabrics.FirstOrDefault(x => x.Id == id);
                    if (efFabric != null)
                    {
                        var audit = context.AuditTrails.Find(efFabric.AuditId);
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
