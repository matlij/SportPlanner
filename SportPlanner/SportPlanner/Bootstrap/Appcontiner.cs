using Autofac;
using SportPlanner.Models;
using SportPlanner.Repository;
using SportPlanner.Services;
using System;
using System.Diagnostics;

namespace SportPlanner.Bootstrap
{
    public class Appcontiner
    {
        private static IContainer _container;

        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //DataStore
            builder.RegisterType<EventDataStore>().As<IEventDataStore>();
            builder.RegisterType<UserDataStore>().As<IDataStore<User>>();

            //Repositories
            builder.RegisterType<GenericRepository>().As<IGenericRepository>();

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
