using System;
using System.IO;
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
            //base.ConfigureApplicationContainer(container);
            //var allInterfaces = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes().Where(type => type.IsInterface));

            // TODO: Get by assembly name
            //var assemblyClasses = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass);
            var nancyioctestDll = "NancyIocTest.dll";
            RegisterFileInContainer(container, nancyioctestDll);
        }

        private static void RegisterFileInContainer(TinyIoCContainer container, string filename)
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (null == currentDirectory)
            {
                throw new Exception("Couldn't get current directory");
            }

            var fullPath = Path.Combine(currentDirectory, filename);
            var assembly = Assembly.LoadFrom(fullPath);
            var assemblyClasses = assembly.GetTypes().Where(type => type.IsClass);
            foreach (var assemblyClass in assemblyClasses)
            {
                var classInterfaceName = string.Format("I{0}", assemblyClass.Name);
                var interfaceMatch = assemblyClass.GetInterfaces().FirstOrDefault(type => type.Name.Equals(classInterfaceName));
                if (null == interfaceMatch)
                {
                    continue;
                }

                container.Register(interfaceMatch, assemblyClass).AsPerRequestSingleton();
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