using EmployeesStructure.Data;
using EmployeesStructure.Data.Repositories;
using EmployeesStructure.Services;
using System;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace EmployeesStructure
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();

              RegisterTypes(container);

              //Web API
              GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        /// 
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<DataBaseContext, DataBaseContext>(new HierarchicalLifetimeManager());
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            container.RegisterType(typeof(IEmployeeRepository), typeof(EmployeeRepository));
            container.RegisterType(typeof(IVacationRepository), typeof(VacationRepository));
            container.RegisterType(typeof(IEmployeeHierarchyService), typeof(EmployeeHierarchyService));
            container.RegisterType(typeof(IEmployeeVacationService), typeof(EmployeeVacationService));
        }
    }
}