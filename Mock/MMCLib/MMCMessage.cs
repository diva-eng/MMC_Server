using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Lidgren.Network;
using Newtonsoft.Json;

namespace MMC
{
    public class MMCMessage
    {
        public ReturnStatus Status;
        public DataType[] Type;
        public string StringData;
        public byte[] ByteData;
        public MMCControl ControlData;
        public MMCSong SongData;
        public MMCCharacter CharacterData;
    }
    public class MMCControl
    {
        public ControlType Type;
        public BasicControl BasicControl;
    }
    public class MMCSong
    {
        public string Name;
        public string SongAPI;
        public List<string> Characters;
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class MMCCharacter
    {
        [JsonProperty]
        public string Name;
        [JsonProperty]
        public string ModelAPI;
        [JsonProperty]
        public string MotionAPI;
        [JsonProperty]
        public bool UseClientModel;
        [JsonProperty]
        public bool UseClientMotion;
        [JsonProperty]
        public bool UseClientRender;
        [JsonProperty]
        private string ID;
        public string Finalize()
        {
            if (ID == null)
                ID = Base64Encode((Guid.NewGuid().ToString() + ("-" + Name + "-" + this.GetHashCode())));
            else
                throw new MMCException("cannot finalize after ID is set.");
            return ID;
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
    public enum ControlType
    {
        BASIC,
        COMPLEX,
        DATA
    }
    public enum BasicControl
    {
        //Playback control
        PLAY,
        PAUSE,
        STOP,
        //Util
        CALIB,
        LOCK,
        UNLOCK,
        //Send recieve
        BEGIN_SEND,
        END_SEND
    }
}
