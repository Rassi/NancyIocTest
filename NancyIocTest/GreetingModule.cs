using Nancy;

namespace NancyIocTest
{
    public interface IGreetingMessageService
    {
        string GetMessage(GreetingCommand command);
    }

    public class GreetingMessageService : IGreetingMessageService
    {
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

    public class GreetingsModule : NancyModule
    {
        private readonly IGreeter _greeter;

        public GreetingsModule(IGreeter greeter)
        {
            _greeter = greeter;
            Get["/"] = x => Greet();
        }

        private string Greet()
        {
            //in a real app this would likely be a this.BindAndValidate<GreetingCommand> call
            var command = new GreetingCommand { SourceUrl = Request.Url };

            //in a real app I would likely automap from the command to some internal model
            //I would not want to leak my public web contract (GreetingCommnad) into my application layer

            return _greeter.Greet(command); 
        }
    }

    public class GreetingCommand
    {
        public Url SourceUrl { get; set; }
    }
}