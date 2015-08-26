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
        private readonly int _countInstance;

        public GreetingMessageService(IRequestUrl requestUrl)
        {
            _requestUrl = requestUrl;
            _countInstance = _count++;
        }

        public string GetMessage()
        {
            return "Hi from GreetingMessageService " + _countInstance + " with url " + _requestUrl.Url;
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
    }

    public class RequestUrl : IRequestUrl
    {
        private readonly NancyContext _context;

        public RequestUrl(NancyContext context)
        {
            _context = context;
        }

        public Url Url { get { return _context.Request.Url; }}
    }

    public class GreetingsModule : NancyModule
    {
        public GreetingsModule(IGreeter greeter)
        {
            Get["/"] = x => greeter.Greet();
        }
    }
}