namespace BubbleStart.Messages
{
    public class ShowExceptionMessage_Message
    {
        public ShowExceptionMessage_Message(string message)
        {
            Message = message;
        }

        #region properties

        /// <summary>
        /// Just some text that comes from the sender.
        /// </summary>
        public string Message { get; private set; }

        #endregion properties
    }
}