using Autofac;
using SportPlanner.Models;
using SportPlanner.Repository;
using SportPlanner.Repository.Interfaces;
using SportPlanner.Services;
using System;
using System.Diagnostics;

namespace SportPlanner.Bootstrap
{
    public static class Appcontiner
    {
        private static IContainer _container;

        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //Services
            builder.RegisterType<UserLoginService>().As<IUserLoginService>();

            //DataStore
            builder.RegisterType<EventDataStore>().As<IEventDataStore>();
            builder.RegisterType<UserDataStore>().As<IDataStore<User>>();

            //Repositories
            builder.RegisterType<GenericRepository>().As<IGenericRepository>();
            builder.RegisterType<LocalRepository<User>>().As<ILocalRepository<User>>();

            _container = builder.Build();
        }

        public static object Resolve(Type typeName)
        {
            return _container.Resolve(typeName);
        }

        public static T Resolve<T>()
        {
            try
            {
                return _container.Resolve<T>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }
    }
}
