using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using $datanamespace$;
using $modelnamespace$;
using WebApplicationQOSI.Libraries;
using WebApplicationQOSI.Libraries.Exceptions;
using VtiUnitConversion.Entities;
using VtiUnitConversion.Helpers;
using VtiUnitConversion.Services;
using Newtonsoft.Json;

namespace $rootnamespace$
{
	[RoutePrefix("$routeprefix$")]
	public class $safeitemrootname$ : Base
	{
		[HttpGet]
		[Route("")]
		public HttpResponseMessage Get()
		{
			List<$valuetype$> results = new List<$valuetype$>();
			int filteredCount = 0;
			int totalCount = 0;

			try
			{
				List<string> filters = new List<string>();

				DateTime? start;
				DateTime? end;

				List<string> dates = new List<string>() { $datetimeproperties$ };
				if (dates.Count > 0)
				{
					foreach (string date in dates)
					{
						start = GetQueryStringDate($"{date}Start");
						end = GetQueryStringDate($"{date}End");

						if (start != null)
						{
							filters.Add($"x.{date} >= '{start:d} 00:00:00'");
						}
						if (end != null)
						{
							filters.Add($"x.{date} <= '{end:d} 23:59:59'");
						}
					}
				}

				// versionFilter?

				int? length = GetQueryStringInt("length");
				int? startRow = GetQueryStringInt("start");

				if (!length.HasValue)
					length = 50;
				else if (length == -1)
				{
					startRow = 0;
					length = 10000;
				}

				if (!startRow.HasValue)
					startRow = 0;

				string textSearch = GetQueryStringValue("search.value");
				if (!string.IsNullOrEmpty(textSearch))
				{
					textSearch = textSearch.Replace("'", ""); // strip single quotes
					filters.Add($"$textsearchfilter$");
				}

				string order = "$orderby$";

				string orderFilter = GetQueryStringValue("order[0].column");
				if (!string.IsNullOrEmpty(orderFilter))
				{
					string orderDir = GetQueryStringValue("order[0].dir");
					string orderDirection = " ASC";
					if (!string.IsNullOrEmpty(orderDir) && orderDir == "desc")
					{
						orderDirection = " DESC";
					}

					switch (orderFilter)
					{
						case "0":
							order = $"$keyname$ {orderDirection}";
							break;
						default:
							break;
					}
				}
				filters.Add("Deleted = 0");

				results = $valuetype$DB.Query(filters, order, length.Value, startRow.Value).Values.ToList<VGMS>();
				totalCount = $valuetype$DB.GetTotal();
				filteredCount = $valuetype$DB.GetFlteredCount(filters);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex);
				return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
			}

			int? draw = GetQueryStringInt("draw");
			if(!draw.HasValue)
			{
				draw = 1;
			}

			return this.Request.CreateResponse(HttpStatusCode.OK, new
			{
				data = results,
				draw = draw,
				recordsTotal = totalCount,
				recordsFiltered = filteredCount
			});
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

				return this.Request.CreateResponse <$valuetype$> (HttpStatusCode.OK, res);
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
				input.$keyname$ = newId;
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
