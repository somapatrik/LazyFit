using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.Drinks;

namespace LazyFit.Messages
{
    public class DrinkNewMessage : ValueChangedMessage<Drink>
    {
        public DrinkNewMessage(Drink value) : base(value)
        {
        }
    }
}
