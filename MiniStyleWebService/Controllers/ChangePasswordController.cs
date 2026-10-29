using MiniStyleWebService.Infrastructure.CustomException;
using MiniStyleWebService.Infrastructure.IService;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System;

namespace MiniStyleWebService.Controllers
{
    [Authorize, RoutePrefix("api/ChangePassword")]
    public class ChangePasswordController : ApiController
    {
        private readonly IChangePasswordService _service;

        public ChangePasswordController(IChangePasswordService service)                        
        {
            _service = service;
        }

        [HttpPut]
        public HttpResponseMessage Upsert(Domain.ChangePassword mChangePassword)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                _service.Upsert(mChangePassword);
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
