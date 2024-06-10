using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace $safeprojectname$
{
    public class HttpRequestFactory
    {
        private readonly string method = "GET";

        public HttpRequestFactory()
        { }

        public HttpRequestFactory(string httpMethod)
        {
            method = httpMethod;
        }

        public HttpRequest CreateHttpRequest(string queryStringKey, string queryStringValue)
        {
            IQueryFeature queryFeature = new QueryFeature(new QueryCollection(LocalConverter.ToDictionary(queryStringKey, queryStringValue)));
            IFeatureCollection features = new FeatureCollection();
            features.Set(queryFeature);
            var context = new DefaultHttpContext(features);
            return context.Request;
        }

        public HttpRequest CreateHttpRequest(string queryStringKey, string queryStringValue, object content)
        {
            IHttpRequestFeature feature = new HttpRequestFeature
            {
                QueryString = $"?{queryStringKey}={queryStringValue}",
                Method = method,
                Body = JsonConvert.SerializeObject(content).ToStream()
            };
            IFeatureCollection features = new FeatureCollection();
            features.Set(feature);
            var context = new DefaultHttpContext(features);
            return context.Request;
        }

        public HttpRequest CreateHttpRequest(object content)
        {
            IHttpRequestFeature feature = new HttpRequestFeature
            {
                Method = method,
                Body = JsonConvert.SerializeObject(content).ToStream()
            };
            IFeatureCollection features = new FeatureCollection();
            features.Set(feature);
            var context = new DefaultHttpContext(features);
            return context.Request;
        }
    }

    public static partial class LocalExtensions
    {
        public static Stream ToStream(this string item)
        {
            var returnValue = new MemoryStream();

            if (String.IsNullOrEmpty(item) == false)
            {
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(item);
                    returnValue = new MemoryStream(bytes);
                }
                catch
                {
                    returnValue = new MemoryStream();
                }
            }

            return returnValue;
        }
    }

    public class LocalConverter
    { 
        public static Dictionary<string, StringValues> ToDictionary(string key, string value)
        {
            var qs = new Dictionary<string, StringValues>
            {
                { key, value }
            };
            return qs;
        }
    }
}