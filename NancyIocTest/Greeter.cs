using NancyIocTest.Web;

namespace NancyIocTest.Application
{
    //In a real app this would be in it's own dll and would not reference Nancy
    //Or likely any web specific things at all
    public interface IGreetingMessageService
    {
        string GetMessage(GreetingCommand command);
    }

    public class GreetingMessageService : IGreetingMessageService
    {
        //By eliminating the bootstrapper you only pay the cose of autoregister once at startup
        //You also only ever instantiate a single instance of your components
        //if you need per request components (should be very few) you can override in the bootstrapper as you had been doing
        private static int _count = 0;
        private readonly int _countInstance;

        public GreetingMessageService()
        {
            _countInstance = _count++;
        }

        public string GetMessage(GreetingCommand command)
        {
            return "Hi from GreetingMessageService " + _countInstance + " with url " + command.SourceUrl;
        }
    }

    public interface IGreeter
    {
        string Greet(GreetingCommand command);
    }

    public class Greeter : IGreeter
    {
        private readonly IGreetingMessageService _service;
        private readonly string _message;
        private static int _count = 0;

        public Greeter(IGreetingMessageService service)
        {
            _service = service;
            _message = "Hi from Greeter " + _count++;
        }

        public string Greet(GreetingCommand command)
        {
            return _message + "<br>" + _service.GetMessage(command);
        }
    }
}