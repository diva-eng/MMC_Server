using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Lidgren.Network;

namespace MMC
{
    public class MMCMessage
    {
        public IPEndPoint sender;
        public IPEndPoint reciever;
        public ReturnStatus status;
        public DataType[] type;
        public string stringData;
        public byte[] byteData;
        public MMCControl controlData;
        public MMCSong songData;
        public MMCCharacter characterData;
    }
    public class MMCControl
    {
    }
    public class MMCSong
    {
    }
    public class MMCCharacter
    {
    }
    public enum ReturnStatus
    {
        OK,
        ERROR,
        RECIEVED
    }
    public enum DataType
    {
        STRING,
        BYTE,
        CONTROL,
        SONG,
        CHARACTER
    }
}
