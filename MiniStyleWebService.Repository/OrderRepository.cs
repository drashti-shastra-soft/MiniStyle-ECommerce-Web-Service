using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.Utils;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace MiniStyleWebService.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public Domain.OrderLister GetAll(Domain.OrderLister mLister)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var query = from o in context.Orders
                                orderby o.Id descending
                                select new
                                {
                                    Id = o.Id,
                                    FullName = o.FullName,
                                    Line1 = o.Line1,
                                    Line2 = o.Line2,
                                    Landmark = o.Landmark,
                                    ZipCode = o.ZipCode,
                                    City = o.City,
                                    State = o.State,
                                    OrderDate = o.OrderDate,
                                    TotalAmount = o.TotalAmount,
                                    PaymentStatus = o.PaymentStatus,
                                    ShippingStatus = o.ShippingStatus
                                };

                    //Apply search filters
                    if (mLister.SearchCriteria != null)
                    {
                        if (mLister.SearchCriteria.FullName.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.FullName != null && x.FullName.ToUpper().Contains(mLister.SearchCriteria.FullName.ToUpper()));
                        }
                        if (mLister.SearchCriteria.Line1.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.Line1 != null && x.Line1.ToUpper().Contains(mLister.SearchCriteria.Line1.ToUpper()));
                        }
                        if (mLister.SearchCriteria.Line2.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.Line2 != null && x.Line2.ToUpper().Contains(mLister.SearchCriteria.Line2.ToUpper()));
                        }
                        if (mLister.SearchCriteria.Landmark.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.Landmark != null && x.Landmark.ToUpper().Contains(mLister.SearchCriteria.Landmark.ToUpper()));
                        }
                        if (mLister.SearchCriteria.City.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.City != null && x.City.ToUpper().Contains(mLister.SearchCriteria.City.ToUpper()));
                        }
                        if (mLister.SearchCriteria.State.IsNotNullOrEmpty())
                        {
                            query = query.Where(x => x.State != null && x.State.ToUpper().Contains(mLister.SearchCriteria.State.ToUpper()));
                        }

                    }

                    // Pagination
                    var efOrder = query.AsEnumerable();

                    // Pagination
                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = efOrder.Count();

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0
                                    ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take)
                                    : 1;

                    efOrder = efOrder.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Order>>(JsonSerializer.Serialize(efOrder.ToList()));

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.Order Upsert(Domain.Order mOrder)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efOrder = context.Orders.Where(x => x.Id == mOrder.Id).FirstOrDefault();
                    if (efOrder == null || efOrder.Id <= default(int))
                    {
                        efOrder = new Persistence.Order();
                        efOrder.RazorpayOrderId = mOrder.RazorpayOrderId;
                        efOrder.RazorpayPaymentId = mOrder.RazorpayPaymentId;
                        efOrder.OrderDate = DateTime.Now;
                        context.Orders.Add(efOrder);
                    }
                    efOrder.FullName = mOrder.FullName;
                    efOrder.Line1 = mOrder.Line1;
                    efOrder.Line2 = mOrder.Line2;
                    efOrder.Landmark = mOrder.Landmark;
                    efOrder.ZipCode = mOrder.ZipCode;
                    efOrder.City = mOrder.City;
                    efOrder.State = mOrder.State;
                    efOrder.OrderDate = DateTime.Now;
                    efOrder.PaymentStatus = mOrder.PaymentStatus;
                    efOrder.ShippingStatus = mOrder.ShippingStatus;
                    efOrder.TotalAmount = mOrder.TotalAmount;
                    context.SaveChanges();
                    mOrder.Id = efOrder.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mOrder;
        }

        public Domain.Order Get(int id)
        {
            var mOrder = new Domain.Order();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efOrder = (from o in context.Orders
                                   where o.Id == id
                                   select new
                                   {
                                       Id = o.Id,
                                       FullName = o.FullName,
                                       Line1 = o.Line1,
                                       Line2 = o.Line2,
                                       Landmark = o.Landmark,
                                       ZipCode = o.ZipCode,
                                       City = o.City,
                                       State = o.State,
                                       OrderDate = o.OrderDate,
                                       TotalAmount = o.TotalAmount,
                                       PaymentStatus = o.PaymentStatus,
                                       ShippingStatus = o.ShippingStatus,
                                   }).AsEnumerable();

                    mOrder = JsonSerializer.Deserialize<Domain.Order>(JsonSerializer.Serialize(efOrder.FirstOrDefault()));
                }

            }
            catch (Exception)
            {
                throw;
            }

            return mOrder;
        }

        public void UpdateStatus(Domain.Order mOrder)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efOrder = context.Orders.Where(x => x.Id == mOrder.Id).FirstOrDefault();
                    if (efOrder != null && efOrder.Id > default(int))
                    {
                        efOrder.PaymentStatus = Enums.PaymentStatus.Pending.ToString();
                        efOrder.ShippingStatus = Enums.ShippingStatus.Not_Shipped.ToString();
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
