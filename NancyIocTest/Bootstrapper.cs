using System;
using System.Linq;
using System.Reflection;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Hosting.Aspnet;
using Nancy.TinyIoc;

namespace NancyIocTest
{
    public class Bootstrapper : DefaultNancyAspNetBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            //var allInterfaces = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes().Where(type => type.IsInterface));
            var assemblyClasses = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass);
            foreach (var assemblyClass in assemblyClasses)
            {
                var interfaces = assemblyClass.GetInterfaces();
                if (interfaces.Count() == 1)
                {
                    container.Register(interfaces[0], assemblyClass).AsPerRequestSingleton();
                }
            }
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            var requestUrl = container.Resolve<IRequestUrl>();
            requestUrl.Context = context;
        }
    }
}