using Nancy;

namespace NancyIocTest
{
    public interface IGreetingMessageService
    {
        string GetMessage();
    }

    public class GreetingMessageService : IGreetingMessageService
    {
        private static int _count = 0;
        private readonly string _message;

        public GreetingMessageService()
        {
            _message = "Hi from GreetingMessageService " + _count++;
        }

        public string GetMessage()
        {
            return _message;
        }
    }

    public interface IGreeter
    {
        string Greet();
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

        public string Greet()
        {
            return _message + "<br>" + _service.GetMessage();
        }
    }

    public class GreetingsModule : NancyModule
    {
        public GreetingsModule(IGreeter greeter)
        {
            Get["/"] = x => greeter.Greet();
        }
    }
}