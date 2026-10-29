using MiniStyleWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MiniStyleWebService.Repository
{
    public class ProductAgeGroupRepository : IProductAgeGroupRepository
    {
        public void Create(List<Domain.ProductAgeGroup> mAgeGroups)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    foreach (var item in mAgeGroups)
                    {
                        var efAgeGroup = new Persistence.ProductAgeGroup();
                        efAgeGroup.ProductId = item.ProductId;
                        efAgeGroup.AgeGroupId = item.AgeGroupId;
                        context.ProductAgeGroups.Add(efAgeGroup);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Domain.ProductAgeGroup> Get(int productId)
        {
            var mAgeGroups = new List<Domain.ProductAgeGroup>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efAgeGroups = (from pag in context.ProductAgeGroups
                                       join ag in context.AgeGroups on pag.AgeGroupId equals ag.Id
                                       where pag.ProductId == productId
                                       select new
                                       {
                                           Id = pag.Id,
                                           ProductId = pag.ProductId,
                                           AgeGroupId = pag.AgeGroupId,
                                           AgeGroup = ag.Name
                                       }).AsEnumerable();

                    mAgeGroups = JsonSerializer.Deserialize<List<Domain.ProductAgeGroup>>(JsonSerializer.Serialize(efAgeGroups.ToList()));
                }

            }
            catch (Exception)
            {
                throw;
            }
            return mAgeGroups;
        }

        public void Delete(int productId)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efAgeGroups = context.ProductAgeGroups.Where(x => x.ProductId == productId).ToList();
                    if (efAgeGroups != null && efAgeGroups.Count > 0)
                    {
                        foreach (var item in efAgeGroups)
                        {
                            context.ProductAgeGroups.Remove(item);
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
