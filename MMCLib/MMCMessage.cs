using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Lidgren.Network;
using Newtonsoft.Json;

using MDXLib.API;
using MDXLib.Data;

namespace MMC
{
    /*In one message, it can contain one or more types of messages
     * the client will extract the message from the MMCMessage and parse
     * them. The entire concert will run on songs, and each song can reference
     * characters based on their UID of the characters. The effects and lighting
     * will be random since it is not set how the SFX station will work
     */

    /* How the messages work
     * 
     * After user clicked "finalize" controller will lock all stations and turn them into recieve mode
     * 1. Controller will call finalize() on all character defined.
     * 2. Sending all character to audio station
     * 2.5 Wait for ACK from audio station (When the character is saved - audio station doesnt download model)
     * 2.75 Send all song to audio station
     * 2.9 Wait for ACK from audio station (When songs are finished downloading)
     * 3. Sending all character to character station
     * 3.5 Waiting for ACK from character station (When all model is downloaded and file list are created)
     * 4. Unlock all station and turn off recieve mode, turn on command mode in all stations
     * 5. Wait for concert to start
     */
    public class MMCMessage
    {
        public ReturnStatus Status;
        public DataType[] Type;
        public string StringData;
        public byte[] ByteData;
        public MMCControl ControlData;
        public MMCSong SongData;
        public MMCCharacter CharacterData;
        public PeerState StateChange;
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
        [JsonIgnore]
        protected Uri serverAddress = new Uri("http://motiondex.com/resources");
        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                if (String.IsNullOrWhiteSpace(Name))
                    return false;
                if (String.IsNullOrWhiteSpace(ModelAPI))
                    return false;
                if (String.IsNullOrWhiteSpace(MotionAPI))
                    return false;
                return true;
            }
        }
        [JsonIgnore]
        public MDXParser parser = new MDXParser("API");
        [JsonIgnore]
        public MDXFile CurrentFile;
        [JsonIgnore]
        public Uri PreviewImage
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(ModelAPI))
                {
                    CurrentFile = parser.GetFile(ModelAPI);
                    return CurrentFile.GetPreview();
                }
                return new Uri(serverAddress, "/default/preview.png");
            }
        }
        public string Finalize()
        {
            if (ID == null && IsValid)
                ID = Base64Encode((Guid.NewGuid().ToString() + ("-" + Name + "-" + this.GetHashCode())));
            else
                throw new MMCException("cannot finalize, it could be finalized or no ready for finalize");
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
        CHARACTER,
        STATE
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
