using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models;

namespace LazyFit.Messages
{
    public class MoodDeleteMessage : ValueChangedMessage<Mood>
    {
        public MoodDeleteMessage(Mood value) : base(value)
        {
        }
    }
}
