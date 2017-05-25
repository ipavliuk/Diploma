using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace BackendsServer.HttpHelper
{
	public static class HttpRequestMessageHelper
	{

		public static string GetValueFromHeaders(this HttpRequestMessage request, string key)
		{
			IEnumerable<string> headerValues;
			if (request.Headers.TryGetValues(key, out headerValues))
			{
				return headerValues.FirstOrDefault();
			}
			else
			{
				return null;
			}
		}

		public static string GetValueFromURL(this HttpRequestMessage request, string key)
		{
			NameValueCollection queryParams = HttpUtility.ParseQueryString(request.RequestUri.Query);
			return queryParams[key];
		}
	}
}