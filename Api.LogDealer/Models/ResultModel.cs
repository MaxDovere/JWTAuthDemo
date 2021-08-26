using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.LogDealer.Models
{
    public class ResultModel<T>
    {
        public string RetMessageError { get; set; }
        public T RetData { get; set; }
        public int RetValue { get; set; }
    }
}
