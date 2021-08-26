using Dapper;
using Api.LogDealer.AccountManager.Model;
using Api.LogDealer.Models;
using Api.LogDealer.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace Api.LogDealer.LogManager
{
    public class LogService : BaseService, ILogService
    {
        private readonly IConfiguration _configuration;

        public LogService(IConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }
        public ResultModel<T> AddLog<T>(LogMessageModel log, int userId)
        {
            ResultModel<T> result = new ResultModel<T>();
            DynamicParameters dp_param = new DynamicParameters();
            dp_param.Add("Data", log.Data, DbType.String);
            dp_param.Add("TypeMessage", log.TypeMessage, DbType.Int32);
            dp_param.Add("Message", log.Message, DbType.String);
            dp_param.Add("ErrorMessage", log.ErrorMessage, DbType.String);
            dp_param.Add("ApplicationName", log.ApplicationName, DbType.String);
            dp_param.Add("MethodName", log.MethodName, DbType.String);
            dp_param.Add("IPName", log.IPName, DbType.String);
            dp_param.Add("ServerName", log.ServerName, DbType.String);
            dp_param.Add("IdRemoteUser", userId > 0 ? userId : log.IdRemoteUser, DbType.Int32);

            dp_param.Add("retVal", DbType.String, direction: ParameterDirection.Output);

            result = DBExecute<T>("sp_addlog", dp_param);
            if (result.RetValue == 200)
            {
                return result;
            }
            return result;
        }

        public ResultModel<T> UpodateLog<T>(int id, LogMessageModel log, int userId)
        {
            ResultModel<T> result = new ResultModel<T>();
            DynamicParameters dp_param = new DynamicParameters();
            dp_param.Add("Logid", id, DbType.Int32);
            dp_param.Add("Data", log.Data, DbType.String);
            dp_param.Add("TypeMessage", log.TypeMessage, DbType.Int32);
            dp_param.Add("Message", log.Message, DbType.String);
            dp_param.Add("ErrorMessage", log.ErrorMessage, DbType.String);
            dp_param.Add("ApplicationName", log.ApplicationName, DbType.String);
            dp_param.Add("MethodName", log.MethodName, DbType.String);
            dp_param.Add("IPName", log.IPName, DbType.String);
            dp_param.Add("ServerName", log.ServerName, DbType.String);
            dp_param.Add("IdRemoteUser", userId > 0 ? userId : log.IdRemoteUser, DbType.Int32);

            dp_param.Add("retVal", DbType.String, direction: ParameterDirection.Output);

            result = DBExecute<T>("sp_updatelog", dp_param);
            if (result.RetValue == 200)
            {
                return result;
            }
            return result;
        }

        public ResultModel<T> DeleteLog<T>(int id, int userId)
        {
            ResultModel<T> result = new ResultModel<T>();
            DynamicParameters dp_param = new DynamicParameters();
            dp_param.Add("Logid", id, DbType.Int32);
            dp_param.Add("IdRemoteUser", userId, DbType.Int32);

            dp_param.Add("retVal", DbType.String, direction: ParameterDirection.Output);

            result = DBExecute<T>("sp_deletelog", dp_param);
            if (result.RetValue == 200)
            {
                return result;
            }
            return result;
        }

        public ResultModel<T> SingleLog<T>(int id, int userId)
        {

            string sql = $"SELECT " +
                $"LogId " +
                $", Data " +
                $", HasErrors " +
                $", TypeMessage " +
                $", Message " +
                $", ErrorMessage " +
                $", ApplicationName " +
                $", MethodName " +
                $", IPName " +
                $", ServerName " +
                $", IdRemoteUser " +
                $", CreationDate " +
                $", ModifierDate " +
                $"FROM dbo.T_UsersLog " +
                $"WHERE idRemoteUser = {userId} " +
                $"AND LogId = {id}";
            return DBQuerySingleOfDefault<T>(sql);
        }
        public ResultModel<List<T>> LogsList<T>(DateTime creationdate)
        {
            string sql = $"SELECT " +
                $"LogId " +
                $", Data " +
                $", HasErrors " +
                $", TypeMessage " +
                $", Message " +
                $", ErrorMessage " +
                $", ApplicationName " +
                $", MethodName " +
                $", IPName " +
                $", ServerName " +
                $", IdRemoteUser " +
                $", CreationDate " +
                $", ModifierDate " +
                $"FROM dbo.T_UsersLog " +
                $"WHERE CreationDate > CONVERT(DATETIME2, '{creationdate}',105)";
            return DBQuery<T>(sql);
        }
    }
}
