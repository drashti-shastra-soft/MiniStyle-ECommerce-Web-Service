using MiniStyleWebService.Infrastructure.CustomException;
using MiniStyleWebService.Infrastructure.IService;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System;

namespace MiniStyleWebService.Controllers
{
    [RoutePrefix("api/SubCategory")]
    public class SubCategoryController : ApiController
    {

        private readonly ISubCategoryService _service;

        public SubCategoryController(ISubCategoryService service)
        {
            _service = service;
        }

        [Authorize, HttpPost, Route("GetAll")]
        public HttpResponseMessage GetAll(Domain.SubCategoryLister mLister)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.GetAll(mLister));
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }
                
        [Authorize, HttpPost]
        public HttpResponseMessage Upsert(Domain.SubCategory mSubCategory)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.Upsert(mSubCategory));
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }
                
        [Authorize, HttpGet, Route("{id:int}")]
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.Get(id));
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }
                
        [Authorize, HttpDelete, Route("{id:int}/LoginUser/{loginUserId:int}")]
        public HttpResponseMessage Delete(int id, int loginUserId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                _service.Delete(id, loginUserId);
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


        [Authorize, HttpGet]
        public HttpResponseMessage GetAllActiveSubCategories()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.GetAllActiveSubCategories());
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

        [AllowAnonymous, HttpGet, Route("GetAllActiveSubCategoriesBy/CategoryId/{categoryId:int}")]
        public HttpResponseMessage GetAllActiveSubCategoriesBy(int categoryId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.GetAllActiveSubCategoriesBy(categoryId));
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
