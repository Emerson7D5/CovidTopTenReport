using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLayer
{
    public class totalProvincias
    {
		public int confirmed { get; set; }
		public int deaths { get; set; }
		public int recovered { get; set; }
		public int confirmed_diff { get; set; }
		public int deaths_diff { get; set; }
		public int recovered_diff { get; set; }
		public int active { get; set; }
		public decimal fatality_rate { get; set; }
		public regions region { get; set; }
	}
}
