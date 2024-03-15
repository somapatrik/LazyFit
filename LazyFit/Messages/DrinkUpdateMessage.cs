using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.Drinks;

namespace LazyFit.Messages
{
    public class DrinkUpdateMessage : ValueChangedMessage<Drink>
    {
        public DrinkUpdateMessage(Drink value) : base(value)
        {
        }
    }
}
