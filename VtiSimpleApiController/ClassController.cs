using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using $datanamespace$;
using $modelnamespace$;
using WebApplicationQOSI.Libraries;
using WebApplicationQOSI.Libraries.Exceptions;

namespace $rootnamespace$
{
    [RoutePrefix("$routeprefix$")] // replaced
    public class $safeitemrootname$ : Base // replaced
    {
        // GET $routeprefix$
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            // list all programs
            List<$valuetype$> results = new List<$valuetype$>(); // replacing 

            try
            {
                results = $valuetype$DB.Cached().Values.ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, results);
        }

        // GET $routeprefix$/123
        [HttpGet]
        [Route("{id:$keytype$}")]
        public HttpResponseMessage Get($keytype$ id)
        {
            try
            {
                $valuetype$ res = $valuetype$DB.Get(id);
                if (res == null)
                    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "No pay date with the supplied ID found.");

                return this.Request.CreateResponse<$valuetype$>(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST $routeprefix$
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post([FromBody] $valuetype$ input)
        {
            try
    {
                $keytype$ newId = $valuetype$DB.Create(input);
                input.$keyName$ = newId;
            }
            catch (PermissionDeniedException ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.Forbidden, ex.Message);
            }
            catch (MissingRequiredFieldsException ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (DuplicateKeyException ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return this.Request.CreateResponse(HttpStatusCode.OK, input);
        }

        // PUT $routeprefix$/123
        [HttpPut]
        [Route("{id:$keytype$}")]
        public HttpResponseMessage Put($keytype$ id, [FromBody] $valuetype$ input)
        {
            try
            {
                $valuetype$DB.Update(id, input);
            }
            catch (RecordNotFoundException ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (PermissionDeniedException ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.Forbidden, ex.Message);
            }
            catch (MissingRequiredFieldsException ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (DuplicateKeyException ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE $routeprefix$/123
        [HttpDelete]
        [Route("{id:$keytype$}")]
        public HttpResponseMessage Delete($keytype$ id)
        {
            try
            {
                $valuetype$DB.Delete(id);
            }
            catch (RecordNotFoundException ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (PermissionDeniedException ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.Forbidden, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}