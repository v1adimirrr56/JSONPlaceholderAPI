using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONPlaceholderAPI.Models.Infrastructure
{
    public interface IPagination<T>
    {
         IEnumerable<T> GetPaging(IEnumerable<T> list, int pageSize, int page);
    }
}
