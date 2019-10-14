
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Crayon.ExternalServices;

namespace Crayon.WebApi
{
    public class IoCConfig
    {
        public static IContainer Container;
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));

        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //WebAPI Settings
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterSource(new ViewRegistrationSource());
            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();
            builder.RegisterModelBinderProvider();


            builder.RegisterType<EcbService>().As<IEcbService>().InstancePerRequest();
            Container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));

            return Container;
        }

    }

}
