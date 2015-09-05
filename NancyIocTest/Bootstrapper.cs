using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using Nancy.Hosting.Aspnet;
using Nancy.TinyIoc;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Web.Common;

namespace NancyIocTest
{
    public class Bootstrapper : NinjectNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Ninject.IKernel existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);
            existingContainer.Bind(x => x.FromThisAssembly()
                                         .SelectAllClasses()
                                         .BindDefaultInterface()
                                         .Configure(config => config.InRequestScope()));
        }

        //protected override void RequestStartup(Ninject.IKernel container, IPipelines pipelines, NancyContext context)
        //{
        //    base.RequestStartup(container, pipelines, context);
        //    pipelines.BeforeRequest += ctx =>
        //    {
        //        var testUrl = container.Get<ITestUrl>();
        //        testUrl.Context = context;
        //        return null;
        //    };

        //    //var testUrl = container.Get<ITestUrl>();
        //    //testUrl.Context = context;
        //}

        //protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        //{
        //    base.ConfigureRequestContainer(container, context);
        //    //container.Register<IRequestUrl2>((cContainer, overloads) => new RequestUrl2(context));
        //    container.Register<NancyContext>((cContainer, overloads) => context);
        //    container.Register<IRequestUrl2, RequestUrl2>();
        //    container.Register<IGreeter, Greeter>();
        //    container.Register<IGreetingMessageService, GreetingMessageService>();
        //}
    }

    public class Startup : IRequestStartup
    {
        private readonly ITestUrl _testUrl;

        public Startup(ITestUrl testUrl)
        {
            _testUrl = testUrl;
        }

        public void Initialize(IPipelines pipelines, NancyContext context)
        {
            _testUrl.Context = context;
        }
    }
}