using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MiniStyleWebService.Repository
{
    public class AgeGroupRepository : IAgeGroupRepository
    {
        public Domain.AgeGroupLister GetAll(Domain.AgeGroupLister mLister)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var query = from a in context.AgeGroups
                                join at in context.AuditTrails on a.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    Description = a.Description,
                                    IsActive = a.IsActive,
                                    AuditId = a.AuditId
                                };

                    // Apply search filters
                    if (mLister.SearchCriteria != null)
                    {
                        if (mLister.SearchCriteria.Name.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(mLister.SearchCriteria.Name.ToUpper()));
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
                    var efAgeGroup = query.AsEnumerable();

                    // Pagination
                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = efAgeGroup.Count();

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0
                            ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take)
                            : 1;

                    efAgeGroup = efAgeGroup.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.AgeGroup>>(JsonSerializer.Serialize(efAgeGroup.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mLister;
        }

        public Domain.AgeGroup Upsert(Domain.AgeGroup mAgeGroup)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efAgeGroups = context.AgeGroups.FirstOrDefault(x => x.Id == mAgeGroup.Id);
                    if (efAgeGroups == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mAgeGroup.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efAgeGroups = new Persistence.AgeGroup
                        {
                            AuditId = audit.Id
                        };
                        context.AgeGroups.Add(efAgeGroups);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efAgeGroups.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mAgeGroup.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efAgeGroups.Name = mAgeGroup.Name;
                    efAgeGroups.Description = mAgeGroup.Description;
                    efAgeGroups.IsActive = mAgeGroup.IsActive;
                    context.SaveChanges();
                    mAgeGroup.Id = efAgeGroups.Id;

                }
            }
            catch (Exception)
            {
                throw;
            }

            return mAgeGroup;
        }

        public Domain.AgeGroup Get(int id)
        {
            var mAgeGroup = new Domain.AgeGroup();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efAgeGroup = (from a in context.AgeGroups
                                      where a.Id == id
                                      select new
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                          Description = a.Description,
                                          IsActive = a.IsActive,
                                          AuditId = a.AuditId,
                                      }).FirstOrDefault();

                    if (efAgeGroup != null)
                    {
                        mAgeGroup = JsonSerializer.Deserialize<Domain.AgeGroup>(JsonSerializer.Serialize(efAgeGroup));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mAgeGroup;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efAgeGroup = context.AgeGroups.FirstOrDefault(x => x.Id == id);
                    if (efAgeGroup != null)
                    {
                        var audit = context.AuditTrails.Find(efAgeGroup.AuditId);
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

        public List<Domain.AgeGroup> GetAllActiveAgeGroup()
        {
            var mAgeGroup = new List<Domain.AgeGroup>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efAgeGroup = (from ag in context.AgeGroups
                                      where ag.IsActive
                                      select new
                                      {
                                          Id = ag.Id,
                                          Name = ag.Name,
                                      }).AsEnumerable();

                    mAgeGroup = JsonSerializer.Deserialize<List<Domain.AgeGroup>>(JsonSerializer.Serialize(efAgeGroup.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mAgeGroup;
        }
    }
}
