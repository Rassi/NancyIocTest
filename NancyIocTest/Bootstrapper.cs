using Nancy;
using Nancy.TinyIoc;

namespace NancyIocTest
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            //container.Register<IRequestUrl>((cContainer, overloads) => new RequestUrl(context));
            container.Register<NancyContext>((cContainer, overloads) => context);
            container.Register<IRequestUrl, RequestUrl>();
            container.Register<IGreeter, Greeter>();
            container.Register<IGreetingMessageService, GreetingMessageService>();
        }
    }
}