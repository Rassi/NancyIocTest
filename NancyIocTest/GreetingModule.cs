using Nancy;

namespace NancyIocTest
{
    public interface IGreetingMessageService
    {
        string GetMessage();
    }

    public class GreetingMessageService : IGreetingMessageService
    {
        private readonly IRequestUrl _requestUrl;
        private static int _count = 0;
        public readonly int CountInstance;

        public GreetingMessageService(IRequestUrl requestUrl)
        {
            _requestUrl = requestUrl;
            CountInstance = _count++;
        }

        public string GetMessage()
        {
            return "Hi from GreetingMessageService " + CountInstance + " with url " + _requestUrl.Url;
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

    public interface IRequestUrl
    {
        Url Url { get; }
        NancyContext Context { get; set; }
    }

    public class RequestUrl : IRequestUrl
    {
        public NancyContext Context { get; set; }
        private static int _count = 0;
        public readonly int CountInstance;

        public RequestUrl()
        {
            CountInstance = _count++;
        }

        public Url Url { get { return Context.Request.Url; }}
    }

    public class GreetingsModule : NancyModule
    {
        public GreetingsModule(IGreeter greeter)
        {
            Get["/"] = x => greeter.Greet();
        }
    }
}