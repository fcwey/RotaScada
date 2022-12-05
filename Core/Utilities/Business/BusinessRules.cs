using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        public static IResult Run(params IResult[] logics)
        {
            foreach (var result in logics)
            {
                if (!result.IsSuccess) //logic çalışmazsa return result diyoruz
                {
                    return result;
                }
            }
            return null; //bütün iş kurallarını geçtiyse 
        }
    }
}
