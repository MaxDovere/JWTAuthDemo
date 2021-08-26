using Api.LogDealer.Models;
using System.Collections.Generic;

namespace Api.LogDealer.GeoManager
{
    public interface IGeoLocation
    {
        ResultModel<T> SingleLatitudine<T>(string servername);
        ResultModel<T> SingleLatitudineLongitudine<T>(string servername);
        ResultModel<List<T>> LatitudineLongitudine<T>(string dealer);
        ResultModel<T> SingleLongitudine<T>(string servername);
        ResultModel<T> SingleInfoObjectServerName<T>(string servername);
        ResultModel<List<T>> InfoDealers<T>();
    }
}