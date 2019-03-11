using Autofac;
using Autofac.Integration.WebApi;
using JSONPlaceholderAPI.Models.Abstract;
using JSONPlaceholderAPI.Models.Infrastructure;
using JSONPlaceholderAPI.Models.ViewModel;
using System;
using System.Reflection;
using System.Web.Http;

namespace JSONPlaceholderAPI.Config
{
    public static class AutofacConfiguration
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<JsonPlaceholderUser>().As<IPlaceholderUser>();
            builder.RegisterType<JsonPlaceholderAlbum>().As<IPlaceholderAlbum>();
            builder.RegisterType<Pagination<UserViewModel>>().As<IPagination<UserViewModel>>();
            builder.RegisterType<Pagination<AlbumViewModel>>().As<IPagination<AlbumViewModel>>();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

    }
}