using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.LogDealer.Models
{
    //public class LatLongModel
    //{
    //    public LocationModel location { get; set; }
    //    public int accuracy { get; set; }
    //}

    public class LocationModel
    {
        public int LocId { get; set; }
        public string Dealer { get; set; }
        public string ServerName { get; set; }
        public string Latitudine {get; set;}
        public string Longitudine { get; set; }
    }
}
