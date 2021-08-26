using Dapper;
using Api.LogDealer.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Api.LogDealer.Repository
{
    public class BaseService : IBaseService
    {
        private readonly IConfiguration _configuration;
        public BaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ResultModel<List<T>> DBQuery<T>(string sql)
        {
            ResultModel<List<T>> result = new ResultModel<List<T>>();
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            result.RetData= db.Query<T>(sql, null, commandType: CommandType.Text).ToList();
            result.RetValue = 200;
            result.RetMessageError = "";
            return result;
        }

        public ResultModel<T> DBExecute<T>(string command, DynamicParameters sp_params)
        {
            ResultModel<T> result = new ResultModel<T>();
            using (IDbConnection dbConnection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                if (dbConnection.State == ConnectionState.Closed)
                    dbConnection.Open();
                using var transaction = dbConnection.BeginTransaction();
                try
                {
                    result.RetData = dbConnection.Query<T>(command, sp_params, commandType: CommandType.StoredProcedure, transaction: transaction).FirstOrDefault();
                    result.RetValue= sp_params.Get<int>("retVal"); //get output parameter value
                    result.RetValue = 200;
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.RetValue = 500; //ex.HResult;
                    result.RetMessageError = ex.Message;
                }
            }
            return result;
        }

        public ResultModel<T> DBQuerySingleOfDefault<T>(string sql)
        {
            ResultModel<T> result = new ResultModel<T>();
            using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            result.RetData = db.QuerySingleOrDefault<T>(sql, null, commandType: CommandType.Text);
            result.RetValue = 200;
            result.RetMessageError = "";
            return result;
        }
    }
}
