using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JSONPlaceholderAPI.Models.Infrastructure.Hateoas
{
    public static class CreatorHateoas
    {
        public static LinkHATEOAS CreateHateoas(string rel, string href, string reqMethod)
        {
            return new LinkHATEOAS(rel, href, reqMethod);
        }
    }
}