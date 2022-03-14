using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntitiesLayer;

namespace CovidTopTenReport.Models
{
    public class DropDownListRegion
    {
        public List<SelectListItem> GetListRegion(IEnumerable<totalRegions> topByRegions)
        {
            List<SelectListItem> listRegion = new List<SelectListItem>();
            foreach (totalRegions region in topByRegions)
            {
                SelectListItem newOption = new SelectListItem
                {
                    Text = region.iso,
                    Value = region.iso
                };
                listRegion.Add(newOption);
            }
            return listRegion;
        }
    }
}