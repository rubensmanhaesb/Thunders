using Azure.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaApp.Tests.Helpers
{

    public static class TestHelper
    {

        public static StringContent CreateContent<T>(T obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj),
                                Encoding.UTF8, "application/json");
        }

 
        public static HttpClient CreateClient()
        {
            return new WebApplicationFactory<Program>().CreateClient();
        }


        public static T ReadResponse<T>(HttpResponseMessage response)
        {
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(jsonResult);
        }
    }
}
