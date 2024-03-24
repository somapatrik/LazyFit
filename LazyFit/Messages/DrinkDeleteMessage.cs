using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.Drinks;


namespace LazyFit.Messages
{
    internal class DrinkDeleteMessage : ValueChangedMessage<Drink>
    {
        public DrinkDeleteMessage(Drink value) : base(value)
        {
        }
    }
}
