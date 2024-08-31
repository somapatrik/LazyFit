using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.Moods;

namespace LazyFit.Messages
{
    public class MoodDeleteMessage : ValueChangedMessage<Mood>
    {
        public MoodDeleteMessage(Mood value) : base(value)
        {
        }
    }
}
