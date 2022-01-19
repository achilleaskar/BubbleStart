using BubbleStart.Helpers;
using BubbleStart.ViewModels;

namespace BubbleStart.Messages
{
    public class OpenPopupUpMessage
    {
        public OpenPopupUpMessage(Hour hour, RoomEnum room)
        {
            Hour = hour;
            Room = room;
        }

        public Hour Hour { get; }
        public RoomEnum Room { get; }
    }
}