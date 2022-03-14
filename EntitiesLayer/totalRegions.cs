using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLayer
{
    public class totalRegions : regions
    {
        [DisplayFormat(DataFormatString = "{0:N}")]
        public int confirmed { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}")]
        public int deaths { get; set; }
    }
}
