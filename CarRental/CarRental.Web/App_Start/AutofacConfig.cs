using System.Data.Entity;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using CarRental.Data.App_Data;
using CarRental.Data.Infastructure;
using CarRental.Data.Repositories;
using CarRental.Service;
using CarRental.Service.Interfaces;

namespace CarRental.Web
{
    public class AutofacConfig
    {
        public static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<CarRentalDBEntities>()
                .As<DbContext>()
                .InstancePerRequest();

            builder.RegisterType<DbFactory>()
                .As<IDbFactory>()
                .InstancePerRequest();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(BaseRepository<>))
                   .As(typeof(IBaseRepository<>))
                   .InstancePerRequest();

            //services
            builder.RegisterType<EncryptionService>()
                .As<IEncryptionService>()
                .InstancePerRequest();

            builder.RegisterType<MembershipService>()
                .As<IMembershipService>()
                .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}