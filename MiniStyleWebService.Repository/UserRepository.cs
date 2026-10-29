using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.Utils;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace MiniStyleWebService.Repository
{
    public class UserRepository : IUserRepository
    {
        public Domain.UserLister GetAll(Domain.UserLister mLister)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {

                    var query = from u in context.Users
                                join at in context.AuditTrails on u.AuditId equals at.Id
                                where at.DeletedBy == null && at.DeletedDate == null
                                select new
                                {
                                    Id = u.Id,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    EmailAddress = u.EmailAddress,
                                    IsActive = u.IsActive,
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        if (mLister.SearchCriteria.FirstName.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.FirstName != null && x.FirstName.ToUpper().Contains(mLister.SearchCriteria.FirstName.ToUpper()));

                        }

                        if (mLister.SearchCriteria.LastName.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.LastName != null && x.LastName.ToUpper().Contains(mLister.SearchCriteria.LastName.ToUpper()));
                        }

                        if (mLister.SearchCriteria.EmailAddress.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.EmailAddress != null && x.EmailAddress.ToUpper().Contains(mLister.SearchCriteria.EmailAddress.ToUpper()));
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

                    var efUser = query.AsEnumerable();

                    // Pagination
                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = efUser.Count();

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0
                            ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take)
                            : 1;

                    efUser = efUser.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.User>>(JsonSerializer.Serialize(efUser.ToList()));
                }
            }
            catch
            {
                throw;
            }

            return mLister;
        }

        public Domain.User Upsert(Domain.User mUser)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efUser = context.Users.FirstOrDefault(x => x.Id == mUser.Id);
                    if (efUser == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mUser.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efUser = new Persistence.User
                        {
                            Password = mUser.Password,
                            AuditId = audit.Id
                        };
                        context.Users.Add(efUser);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efUser.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mUser.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efUser.FirstName = mUser.FirstName;
                    efUser.LastName = mUser.LastName;
                    efUser.EmailAddress = mUser.EmailAddress;
                    efUser.IsActive = mUser.IsActive;
                    context.SaveChanges();
                    mUser.Id = efUser.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mUser;
        }

        public Domain.User Get(int id)
        {
            var mUser = new Domain.User();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efUser = (from u in context.Users
                                  where u.Id == id
                                  select new
                                  {
                                      Id = u.Id,
                                      FirstName = u.FirstName,
                                      LastName = u.LastName,
                                      EmailAddress = u.EmailAddress,
                                      IsActive = u.IsActive,
                                  }).FirstOrDefault();

                    if (efUser != null)
                    {
                        mUser = JsonSerializer.Deserialize<Domain.User>(JsonSerializer.Serialize(efUser));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mUser;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efUser = context.Users.FirstOrDefault(x => x.Id == id);
                    if (efUser != null)
                    {
                        var audit = context.AuditTrails.Find(efUser.AuditId);
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

        public Domain.User GetBy(string emailAddress)
        {
            var mUser = new Domain.User();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efUser = (from user in context.Users
                                  where user.EmailAddress.Trim().ToUpper() == emailAddress.Trim().ToUpper()
                                  select new
                                  {
                                      Id = user.Id,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      EmailAddress = user.EmailAddress,
                                      Password = user.Password,
                                      IsActive = user.IsActive
                                  }).AsEnumerable();

                    mUser = JsonSerializer.Deserialize<Domain.User>(JsonSerializer.Serialize(efUser.FirstOrDefault()));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mUser;
        }

        public string GetPasswordBy(int id)
        {
            string password = string.Empty;
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    password = context.Users.Where(x => x.Id == id).Select(s => s.Password).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return password;
        }

        public void SetPasswordBy(int id, string password)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efUser = context.Users.Where(x => x.Id == id).FirstOrDefault();
                    if (efUser != null && efUser.Id > default(int))
                    {
                        efUser.Password = password;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckEmailAddressExist(Domain.User mUser)
        {
            bool isExist = false;
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    isExist = context.Users.Where(x => x.Id != mUser.Id && x.EmailAddress.Trim().ToUpper() == mUser.EmailAddress.Trim().ToUpper()).Count() > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isExist;
        }
    }
}
