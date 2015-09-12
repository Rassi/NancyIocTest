using Nancy;
using NancyIocTest.Application;

namespace NancyIocTest.Web
{
    //In a real app this would be in it's own web project and would reference Nancy

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

            //IMO moving data to and from HTTP format in the module does a lot for SRP and OCP
            //access headers/urls/query params etc. throughout the application code means
            //your application code needs to understand your HTTP/Web formats and needs to change
            //when details of your HTTP/Web interface change

            return _greeter.Greet(command); 
        }
    }

    public class GreetingCommand
    {
        public Url SourceUrl { get; set; }
    }
}