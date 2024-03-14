using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.Drinks;

namespace LazyFit.Messages
{
    public class DrinkRefreshMessage : ValueChangedMessage<Drink>
    {
        public DrinkRefreshMessage(Drink value) : base(value)
        {
        }
    }
}
