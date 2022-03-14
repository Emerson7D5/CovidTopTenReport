using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EntitiesLayer;
using Newtonsoft.Json;
using DomainLayer;
using CovidTopTenReport.Models;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace CovidTopTenReport.Controllers
{
    public class HomeController : Controller
    {
		/// <summary>
		/// Return the Top Ten Cases by Region
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			try
			{
				IEnumerable<totalRegions> totalByProvinces = new Provinces().GetTotals("2022-03-11");
				IEnumerable<totalRegions> totalByRegions = new Regions().GetTotals(totalByProvinces);
				IEnumerable<totalRegions> topByRegions = new Regions().GetTopTen(totalByRegions);
				var totalByProvincesSerialized = JsonConvert.SerializeObject(totalByProvinces);
				Session["_totalByProvinces"] = totalByProvincesSerialized;
				var topByRegionsSerialized = JsonConvert.SerializeObject(topByRegions);
				Session["_topByRegions"] = topByRegionsSerialized;

				ViewBag.TopByRegionsList = new DropDownListRegion().GetListRegion(topByRegions);
				return View(topByRegions);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Return the Top Ten Cases by Provices of one region
		/// </summary>
		/// <param name="RegionSelected"></param>
		/// <returns></returns>
		public ActionResult TopTenProvince(string RegionSelected)
		{
			try
			{
				if (Session["_totalByProvinces"] != null)
				{
					var totalByProvinces = Session["_totalByProvinces"];
					var topByRegions = Session["_topByRegions"];

					List<totalRegions> totalByProvincesList = JsonConvert.DeserializeObject<List<totalRegions>>(totalByProvinces.ToString());
					List<totalRegions> topByRegionsList = JsonConvert.DeserializeObject<List<totalRegions>>(topByRegions.ToString());

					IEnumerable<totalRegions> topByProvinces = new Provinces().GetTopTen(totalByProvincesList, RegionSelected);

					var topByProvincesSerialized = JsonConvert.SerializeObject(topByProvinces);
					Session["_topByProvinces"] = topByProvincesSerialized;

					ViewBag.TopByRegionsList = new DropDownListRegion().GetListRegion(topByRegionsList); ;

					return View(topByProvinces);
				}

				return View();
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		/// <summary>
		/// Export the top ten covid cases to JSON file
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public FileContentResult json()
		{
			try
			{
				var totalByProvinces = JsonConvert.SerializeObject(dataToExport());
				var fileName = "TopTenCovid.json";
				var mimeType = "text/plain";
				var fileBytes = Encoding.ASCII.GetBytes(totalByProvinces);
				return new FileContentResult(fileBytes, mimeType)
				{
					FileDownloadName = fileName
				};
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		/// <summary>
		/// Export the top ten covid cases to XML file
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public FileResult xml()
		{
			try
			{
				List<totalRegions> topByRegionsList = dataToExport();

				XElement identity = new XElement("cases");
				foreach (totalRegions province in topByRegionsList)
				{
					XElement elm = new XElement("row",
						  new XElement("region", province.iso),
						  new XElement("province", province.province),
						  new XElement("confirmed", province.confirmed),
						  new XElement("deaths", province.deaths));
					identity.Add(elm);
				}

				XElement xml = new XElement("xml", identity);

				MemoryStream output = new MemoryStream();
				StreamWriter xmlFile = new StreamWriter(output, Encoding.UTF8);
				xmlFile.WriteLine(xml);
				xmlFile.Flush();
				output.Position = 0;
				return File(output, "text/plain", "TopTenCovid.xml");
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		/// <summary>
		/// Export the top ten covid cases to CSV file
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public FileResult csv()
		{
			try
			{
				List<totalRegions> topByRegionsList = dataToExport();
				MemoryStream output = new MemoryStream();
				StreamWriter writer = new StreamWriter(output, Encoding.UTF8);
				writer.WriteLine("Region,Province,Confirmed,Deaths");
				foreach (totalRegions province in topByRegionsList)
				{
					writer.WriteLine(string.Format("{0},{1},{2},{3}", province.iso, province.province, province.confirmed, province.deaths));
				}
				writer.Flush();
				output.Position = 0;
				return File(output, "text/comma-separated-values", "TopTenCovid.csv");
			}
			catch (Exception ex)
			{
				throw;
			}


		}

		/// <summary>
		/// Get the data to export, two case: top ten region or top ten provices
		/// </summary>
		/// <returns></returns>
		public List<totalRegions> dataToExport()
		{
			var totalByProvinces = "";
			if (Session["_topByProvinces"] != null)
			{
				totalByProvinces = Session["_topByProvinces"].ToString();
			}
			else
			{
				totalByProvinces = Session["_topByRegions"].ToString();
			}
			return JsonConvert.DeserializeObject<List<totalRegions>>(totalByProvinces.ToString());
		}
	}
}