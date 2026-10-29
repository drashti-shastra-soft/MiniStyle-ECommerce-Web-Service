using MiniStyleWebService.Infrastructure.CustomException;
using MiniStyleWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MiniStyleWebService.Controllers
{
    [RoutePrefix("api/ProductSizeChart")]
    public class ProductSizeChartController : ApiController
    {
        private readonly IProductSizeChartService _service;

        public ProductSizeChartController(IProductSizeChartService service)
        {
            _service = service;
        }


        [Authorize, HttpPost]
        public HttpResponseMessage Upsert(Domain.ProductSizeChart mProductSizeChart)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.Upsert(mProductSizeChart));
            }
            catch (APIRequestFailedException ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Unexpected Error: " + ex.Message);
            }
            return response;
        }

        [HttpGet, Route("ProductId/{productId:int}/UnitType/{unitType}")]
        public HttpResponseMessage GetAllActive(int productId, string unitType)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.GetAllActive(productId, unitType));
            }
            catch (APIRequestFailedException ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [HttpGet, Route("{id:int}")]
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.Get(id));
            }
            catch (APIRequestFailedException ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }

        [HttpDelete, Route("{id:int}/UserId/{userId:int}/LoginUser/{loginUserId:int}")]
        public HttpResponseMessage Delete(int id, int userId, int loginUserId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                _service.Delete(id, userId, loginUserId);
                response = Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (APIRequestFailedException ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }


    }
}
