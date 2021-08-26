using Api.LogDealer.Models;
using Api.LogDealer.Repository;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Api.LogDealer.GeoManager
{
    public class GeoLocation : BaseService, IGeoLocation
    {
        private readonly IConfiguration _configuration;

        public GeoLocation(IConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }
        public ResultModel<T> SingleInfoObjectServerName<T>(string servername)
        {
            string sql = $"SELECT " +
                $"LocId " +
                $", Brand " +
                $", Dealer " +
                $", City " +
                $", Street " +
                $", Cap " +
                $", Description " +
                $", Province " +
                $", ServerName " +
                $", IPName " +
                $", Latitudine " +
                $", Longitudine " +
                $"FROM dbo.T_ServerName_Locations " +
                $"WHERE servername = '{servername}'";
            return DBQuerySingleOfDefault<T>(sql);

        }
        public ResultModel<T> SingleLatitudine<T>(string servername)
        {
            string sql = $"SELECT " +
                $"LocId " +
                $", Dealer " +
                $", Servername " +
                $", Latitudine " +
                $", 0 as Longitudine " +
                $"FROM dbo.T_ServerName_Locations " +
                $"WHERE servername = '{servername}'";
            return DBQuerySingleOfDefault<T>(sql);
        }

        public ResultModel<T> SingleLongitudine<T>(string servername)
        {
            string sql = $"SELECT " +
                $"LocId " +
                $", Dealer " +
                $", Servername " +
                $", 0 as Latitudine " +
                $", Longitudine " +
                $"FROM dbo.T_ServerName_Locations " +
                $"WHERE servername = '{servername}'";
            return DBQuerySingleOfDefault<T>(sql);
        }
        public ResultModel<T> SingleLatitudineLongitudine<T>(string servername)
        {
            string sql = $"SELECT " +
                $"LocId " +
                $", Dealer " +
                $", Servername " +
                $", Latitudine " +
                $", Longitudine " +
                $"FROM dbo.T_ServerName_Locations " +
                $"WHERE servername = '{servername}'";
            return DBQuerySingleOfDefault<T>(sql);
        }
        public ResultModel<List<T>> LatitudineLongitudine<T>(string dealer)
        {
            string sql = $"SELECT " +
                $"LocId " +
                $", Dealer " +
                $", Servername " +
                $", Latitudine " +
                $", Longitudine " +
                $"FROM dbo.T_ServerName_Locations " +
                $"WHERE Dealer = '{dealer}'";

            return DBQuery<T>(sql);
        }
        public ResultModel<List<T>> InfoDealers<T>()
        {
            string sql = $"SELECT " +
                $"LocId " +
                $", Dealer " +
                $", Servername " +
                $", 0 Latitudine " +
                $", 0 Longitudine " +
                $"FROM dbo.T_ServerName_Locations";

            return DBQuery<T>(sql);
        }
    }
}
