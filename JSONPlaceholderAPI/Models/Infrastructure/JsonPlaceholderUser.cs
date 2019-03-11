using JSONPlaceholderAPI.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JSONPlaceholderAPI.Models.Infrastructure
{
    public class JsonPlaceholderUser : IPlaceholderUser
    {
        private string _url = @"http://jsonplaceholder.typicode.com/users";

        public string GetLink()
        {
            return _url;
        }
    }
}