using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Communication
{
    public enum PacketType
    {
        NameDenied, // [DeniedName] - Server only
        Map, // [Map] - Server only
        Message, // [SenderName, Message]
        Quit, // [Client Id]
        Registration, // [Username]
        Tick, // [Tick Id] - Server only
        Welcome // [Client Id, Username] - Server only
    }
}
