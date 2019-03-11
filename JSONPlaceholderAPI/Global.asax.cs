using AutoMapper;
using JSONPlaceholderAPI.Config;
using JSONPlaceholderAPI.Models.Mappings;
using JSONPlaceholderAPI.Models;
using JSONPlaceholderAPI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Xml.Serialization;

namespace JSONPlaceholderAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutofacConfiguration.ConfigureContainer();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Mapper.Initialize(cfg => cfg.CreateMap<User, UserViewModel>());
        }
    }
}
