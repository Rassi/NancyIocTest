using Nancy;

namespace NancyIocTest
{
    public interface IGreetingMessageService
    {
        string GetMessage();
    }

    public class GreetingMessageService : IGreetingMessageService
    {
        private readonly IRequestUrl2 _requestUrl;
        private readonly ITestUrl _testUrl;
        private static int _count = 0;
        private readonly int _countInstance;

        public GreetingMessageService(IRequestUrl2 requestUrl, ITestUrl testUrl)
        {
            _requestUrl = requestUrl;
            _testUrl = testUrl;
            _countInstance = _count++;
        }

        public string GetMessage()
        {
            return "Hi from GreetingMessageService " + _countInstance + GetUrl();
        }

        private string GetUrl()
        {
            return " with url " + (_testUrl.Context.Request.Url == null ? "null" : _testUrl.Context.Request.Url.ToString());
            //return " with url " + (_requestUrl.Url == null ? "null" : _requestUrl.Url.ToString());
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

    public interface IRequestUrl2
    {
        NancyContext Context { get; set; }
        Url Url { get; }
    }

    public class RequestUrl2 : IRequestUrl2
    {
        public RequestUrl2()
        {
        }

        public NancyContext Context { get; set; }
        public Url Url { get { return Context.Request == null ? null : Context.Request.Url; }}
    }

    public interface ITestUrl
    {
        string TestMethod();
        NancyContext Context { get; set; }
    }

    public class TestUrl : ITestUrl
    {
        public TestUrl()
        {
            
        }

        public string TestMethod()
        {
            return "test";
        }

        public NancyContext Context { get; set; }
    }

    //public class ModuleBase : NancyModule
    //{
    //    public ModuleBase(IRequestUrl2 requestUrl)
    //    {
    //        requestUrl.Context = Context;
    //    }
    //}

    public class GreetingsModule : NancyModule
    {
        public GreetingsModule(IRequestUrl2 requestUrl, IGreeter greeter, ITestUrl testUrl)// : base(requestUrl)
        {
            Get["/"] = x =>
            {
                //testUrl.TestMethod();
                //requestUrl.Context = Context;
                //testUrl.Context = Context;
                return greeter.Greet();
            };
        }
    }
}