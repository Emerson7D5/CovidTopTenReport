using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer
{
    public class HttpServices
    {
        public HttpClient Client { get; set; }

        public HttpServices()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://covid-19-statistics.p.rapidapi.com/");
            Client.DefaultRequestHeaders.Add("x-rapidapi-host", "covid-19-statistics.p.rapidapi.com");
            Client.DefaultRequestHeaders.Add("x-rapidapi-key", "2d79403189msh20fb215b5eeb7e2p19be78jsndf19ec6089f2");
        }

        public HttpResponseMessage GetResponse(string url)
        {
            return Client.GetAsync(url).Result;
        }
    }
}
