using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.Moods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Messages
{
    public class MoodUpdateMessage : ValueChangedMessage<Mood>
    {
        public MoodUpdateMessage(Mood value) : base(value)
        {
        }
    }
}
