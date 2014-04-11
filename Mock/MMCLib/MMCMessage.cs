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
        public string Name;
        public string SongAPI;
        public List<string> Characters;
    }
    public class MMCCharacter
    {
        public string Name;
        public string ModelAPI;
        public string MotionAPI;
        public bool UseClientModel;
        public bool UseClientMotion;
        public bool UseClientRender;
        private string ID;
        public void Finalize()
        {
            if (ID != null)
                ID = Base64Encode((Guid.NewGuid().ToString() + ("-" + Name + "-" + this.GetHashCode())));
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
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
