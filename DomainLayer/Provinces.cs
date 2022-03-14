using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesLayer;
using EntitiesLayer;
using System.Net.Http;
using Newtonsoft.Json;

namespace DomainLayer
{
    public class Provinces
    {
		/// <summary>
		/// Get the covid-19 cases from the API
		/// </summary>
		/// <param name="date">Parameter to request in the API</param>
		/// <returns></returns>
		public IEnumerable<totalRegions> GetTotals(string date)
		{

			ServicesLayer.HttpServices serviceObj = new HttpServices();
			HttpResponseMessage response = serviceObj.GetResponse($"reports?date={date}"); //&iso=USA
			response.EnsureSuccessStatusCode();
			var apiResult = response.Content.ReadAsStringAsync().Result;

			var dataFromApi = JsonConvert.DeserializeAnonymousType(apiResult, new Dictionary<string, object>());

			List<totalProvincias> totalByProvinceList = JsonConvert.DeserializeObject<List<totalProvincias>>(dataFromApi["data"].ToString());

			IEnumerable<totalRegions> totalByProvinces = (from tbp in totalByProvinceList
														  orderby tbp.confirmed descending
														  select new totalRegions
														  {
															  iso = tbp.region.iso,
															  name = tbp.region.name,
															  province = tbp.region.province,
															  confirmed = tbp.confirmed,
															  deaths = tbp.deaths
														  });

			return totalByProvinces;
		}

		/// <summary>
		/// Return the top ten of cases by provinces in one region.
		/// </summary>
		/// <param name="totalByProvincesList">Dataset to filter by region </param>
		/// <param name="ddlRegions">Parameter of region selected in the screen</param>
		/// <returns></returns>
		public IEnumerable<totalRegions> GetTopTen(List<totalRegions> totalByProvincesList, string ddlRegions)
		{
			IEnumerable<totalRegions> topByProvinces = (from tbp in totalByProvincesList
														where tbp.iso == ddlRegions
														orderby tbp.confirmed descending
														select new totalRegions
														{
															iso = tbp.iso,
															name = tbp.name,
															province = tbp.province,
															confirmed = tbp.confirmed,
															deaths = tbp.deaths
														}).Take(10);
			return topByProvinces;
		}
	}
}
