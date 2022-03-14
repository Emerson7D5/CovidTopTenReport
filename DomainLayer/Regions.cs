using System;
using System.Collections.Generic;
using System.Linq; 
using EntitiesLayer;

namespace DomainLayer
{
    public class Regions
    {
		/// <summary>
		/// Sum and orders the cases by region
		/// </summary>
		/// <param name="totalByProvinces">Data to sum</param>
		/// <returns></returns>
		public IEnumerable<totalRegions> GetTotals(IEnumerable<totalRegions> totalByProvinces)
		{
			IEnumerable<totalRegions> totalByRegions = (from tbp in totalByProvinces
														select tbp).GroupBy(s => new { s.iso, s.name })
									  .Select(g => new totalRegions
									  {
										  iso = g.Key.iso,
										  name = g.Key.name,
										  confirmed = g.Sum(x => x.confirmed),
										  deaths = g.Sum(x => x.deaths),
									  }
								);
			return totalByRegions;
		}

		/// <summary>
		/// Return the top ten of cases by region
		/// </summary>
		/// <param name="totalByRegions">Dataset to get the top ten</param>
		/// <returns></returns>
		public IEnumerable<totalRegions> GetTopTen(IEnumerable<totalRegions> totalByRegions)
		{
			IEnumerable<totalRegions> topByRegions = (from tbr in totalByRegions
													  orderby tbr.confirmed descending
													  select new totalRegions
													  {
														  iso = tbr.iso,
														  name = tbr.name,
														  confirmed = tbr.confirmed,
														  deaths = tbr.deaths
													  }).Take(10);
			return topByRegions;
		}

	}
}
