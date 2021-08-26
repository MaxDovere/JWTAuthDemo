using Dapper;
using Api.LogDealer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.LogDealer.Repository
{
    public interface IBaseService
    {
        ResultModel<T> DBExecute<T>(string query, DynamicParameters sp_params);
        ResultModel<List<T>> DBQuery<T>(string sql);
        ResultModel<T> DBQuerySingleOfDefault<T>(string sql);
    }
}
