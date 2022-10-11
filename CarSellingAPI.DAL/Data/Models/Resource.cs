#nullable disable
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSellingAPI.DAL.Data.Models
{
    public abstract class Resource
    {
        [JsonProperty(Order =-2)]
        public string Href { get; set; }

    }
}
