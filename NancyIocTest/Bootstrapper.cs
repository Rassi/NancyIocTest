using Nancy;
using Nancy.TinyIoc;

namespace NancyIocTest
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            container.Register<IGreeter, Greeter>();
            base.ConfigureRequestContainer(container, context);
            //container.Register<IGreetingMessageService, GreetingMessageService>();
        }
    }
}