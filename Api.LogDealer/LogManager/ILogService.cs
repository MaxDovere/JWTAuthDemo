using Api.LogDealer.Models;
using System;
using System.Collections.Generic;

namespace Api.LogDealer.LogManager
{
    public interface ILogService
    {
        ResultModel<T> AddLog<T>(LogMessageModel log, int userId);
        ResultModel<T> UpodateLog<T>(int id, LogMessageModel log, int userId);
        ResultModel<T> DeleteLog<T>(int id, int userId);
        ResultModel<List<T>> LogsList<T>(DateTime creationdate);
        ResultModel<T> SingleLog<T>(int id, int userId);
    }
}
