using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.LogDealer.Models
{
	//T_ServerName_Locations
	public class ServerNameLocation
    {
		public int LocId { get; set; }
		public string Brand { get; set; }
		public string Dealer { get; set; }
		public string City { get; set; }
		public string Street { get; set; }
		public string Cap { get; set; }
		public string Description { get; set; }
		public string Province { get; set; }
		public string ServerName { get; set; }
		public string IPName { get; set; }
		public string Latitudine { get; set; }
		public string Longitudine { get; set; }
    }
}
