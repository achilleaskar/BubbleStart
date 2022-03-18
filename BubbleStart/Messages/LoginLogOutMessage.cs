namespace BubbleStart.Messages
{
    public class LoginLogOutMessage
    {
        public LoginLogOutMessage(bool login)
        {
            Login = login;
        }

        public bool Login { get; }
        public bool Force { get; }
    }
}