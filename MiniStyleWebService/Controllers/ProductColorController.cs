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
    [RoutePrefix("api/ProductColor")]
    public class ProductColorController : ApiController
    {
        private readonly IProductColorService _service;

        public ProductColorController(IProductColorService service)
        {
            _service = service;
        }

        [Authorize, HttpPut]
        public HttpResponseMessage Upsert(List<Domain.ProductColor> mDetail)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                _service.Upsert(mDetail);
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


        [Authorize, HttpGet, Route("ProductId/{productId:int}")]
        public HttpResponseMessage Get(int productId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.Get(productId));
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
