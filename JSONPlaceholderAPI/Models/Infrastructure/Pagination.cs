using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace JSONPlaceholderAPI.Models.Infrastructure
{
    public class Pagination<T> : IPagination<T>
    {
        public IEnumerable<T> GetPaging(IEnumerable<T> list, int pageSize, int page)
        {
            if ((pageSize > 0 || page > 0)
                &&  pageSize * page <= list.Count())
            {
                return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }

            return list;
        }
    }
}