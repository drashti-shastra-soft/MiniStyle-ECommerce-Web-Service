using MiniStyleWebService.Infrastructure.IRepository;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace MiniStyleWebService.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Domain.Category> GetAll()
        {
            var mCategories = new List<Domain.Category>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efCategories = (from c in context.Categories
                                        join at in context.AuditTrails on c.AuditId equals at.Id
                                        where at.DeletedBy == null && at.DeletedDate == null && c.IsActive
                                        select new
                                        {
                                            Id = c.Id,
                                            Name = c.Name
                                        }).AsEnumerable();

                    mCategories = JsonSerializer.Deserialize<List<Domain.Category>>(JsonSerializer.Serialize(efCategories.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mCategories;
        }
    }
}
