using MiniStyleWebService.Infrastructure.IRepository;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace MiniStyleWebService.Repository
{
    public class ProductSizeChartRepository : IProductSizeChartRepository
    {
        public Domain.ProductSizeChart Upsert(Domain.ProductSizeChart mProductSizeChart)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efSizeChart = context.ProductSizeCharts.Where(x => x.Id == mProductSizeChart.Id || (x.ProductAgeGroupId == mProductSizeChart.ProductAgeGroupId && x.UnitType == mProductSizeChart.UnitType && x.WearablePart == mProductSizeChart.WearablePart)).FirstOrDefault();

                    if (efSizeChart == null)
                    {
                        var audit = new Persistence.AuditTrail
                        {
                            CreatedBy = mProductSizeChart.CreatedBy,
                            CreatedDate = DateTime.Now
                        };

                        context.AuditTrails.Add(audit);
                        context.SaveChanges();

                        efSizeChart = new Persistence.ProductSizeChart
                        {
                            AuditId = audit.Id
                        };
                        context.ProductSizeCharts.Add(efSizeChart);
                    }
                    else
                    {
                        var audit = context.AuditTrails.Find(efSizeChart.AuditId);
                        if (audit != null)
                        {
                            audit.LastModifiedBy = mProductSizeChart.LastModifiedBy;
                            audit.LastModifiedDate = DateTime.Now;
                        }
                    }

                    efSizeChart.ProductId = mProductSizeChart.ProductId;
                    efSizeChart.ProductAgeGroupId = mProductSizeChart.ProductAgeGroupId;
                    efSizeChart.WearablePart = mProductSizeChart.WearablePart;
                    efSizeChart.UnitType = mProductSizeChart.UnitType;
                    efSizeChart.Shoulder = mProductSizeChart.Shoulder;
                    efSizeChart.Length = mProductSizeChart.Length;
                    efSizeChart.Number = mProductSizeChart.Number;
                    efSizeChart.SleeveLength = mProductSizeChart.SleeveLength;
                    efSizeChart.Chest = mProductSizeChart.Chest;
                    efSizeChart.Waist = mProductSizeChart.Waist;

                    context.SaveChanges();

                    mProductSizeChart.Id = efSizeChart.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mProductSizeChart;
        }

        public Domain.ProductSizeChart Get(int id)
        {
            var mProductSizeChart = new Domain.ProductSizeChart();
            try
            {
                using (var context = new MiniStyleWebService.Persistence.MiniStyleDB())
                {
                    var efSizeChart = (from psc in context.ProductSizeCharts
                                       where psc.Id == id
                                       select new
                                       {
                                           Id = psc.Id,
                                           ProductId = psc.ProductId,
                                           ProductAgeGroupId = psc.ProductAgeGroupId,
                                           UnitType = psc.UnitType,
                                           Shoulder = psc.Shoulder,
                                           WearablePart = psc.WearablePart,
                                           Length = psc.Length,
                                           Number = psc.Number,
                                           Chest = psc.Chest,
                                           SleeveLength = psc.SleeveLength,
                                           Waist = psc.Waist,
                                           AuditId = psc.AuditId,
                                       }).FirstOrDefault();

                    if (efSizeChart != null)
                    {
                        mProductSizeChart = JsonSerializer.Deserialize<Domain.ProductSizeChart>(JsonSerializer.Serialize(efSizeChart));
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return mProductSizeChart;
        }


        public void Delete(int id, int userId, int loginUserId)
        {
            try
            {
                using (var context = new MiniStyleWebService.Persistence.MiniStyleDB())
                {
                    var efItem = context.ProductSizeCharts.FirstOrDefault(x => x.Id == id);
                    if (efItem != null)
                    {
                        var audit = context.AuditTrails.Find(efItem.AuditId);
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

        public List<Domain.ProductSizeChart> GetAllActive(int productId, string unitType)
        {
            var mSizeChartRows = new List<Domain.ProductSizeChart>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efSizeChart = (from psc in context.ProductSizeCharts
                                       join pag in context.ProductAgeGroups on psc.ProductAgeGroupId equals pag.Id
                                       join ag in context.AgeGroups on pag.AgeGroupId equals ag.Id
                                       where psc.ProductId == productId && psc.UnitType == unitType
                                       select new
                                       {
                                           Id = psc.Id,
                                           AgeGroup = ag.Name,
                                           UnitType = psc.UnitType,
                                           Shoulder = psc.Shoulder,
                                           Length = psc.Length,
                                           Chest = psc.Chest,
                                           SleeveLength = psc.SleeveLength,
                                           Number = psc.Number,
                                           WearablePart = psc.WearablePart,
                                           Waist = psc.Waist,
                                       }).AsEnumerable();

                    mSizeChartRows = JsonSerializer.Deserialize<List<Domain.ProductSizeChart>>(JsonSerializer.Serialize(efSizeChart.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mSizeChartRows;
        }

    }
}
