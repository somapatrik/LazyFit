using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Messages
{
    class ReloadActionsMessage : ValueChangedMessage<int>
    {
        public ReloadActionsMessage(int value) : base(value)
        {
        }
    }
}
