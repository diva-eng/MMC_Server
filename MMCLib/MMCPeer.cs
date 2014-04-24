using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

using Lidgren.Network;
using Newtonsoft.Json;

namespace MMC
{
    public class MMCPeer : NetPeer
    {
        public MMCPeerConfig pConfig { get; set; }
        public PeerState pState { get; set; }
        public HashSet<ClientValue> DiscoveredClients { get; set; }
        private Action<NetIncomingMessage> UserCallback;
        public MMCPeer(MMCPeerConfig config) : base (config.npConfig)
        {
            DiscoveredClients = new HashSet<ClientValue>();
            pConfig = config;
            RegisterReceivedCallback(new SendOrPostCallback(MessageCallback));
        }
        public void Discover(int port)
        {
            this.DiscoverLocalPeers(port);
        }
        public void RespondDiscover(NetIncomingMessage message)
        {
            NetOutgoingMessage response = this.CreateMessage();
            response.Write(this.pConfig.type.ToString() + "@" + this.pConfig.name);

            // Send the response to the sender of the request
            this.SendDiscoveryResponse(response, message.SenderEndPoint);
        }
        public void RespondDiscoverResponse(NetIncomingMessage message)
        {
            string[] id = message.ReadString().Split('@');
            switch (id[0])
            {
                case "CHARACTER":
                    break;
                case "SFX":
                    break;
                case "LIGHT":
                    break;
                case "CONTROLLER":
                    break;
            }
            DiscoveredClients.Add(new ClientValue(message.SenderEndPoint, id));
        }
        public void RegisterCallback(Action<NetIncomingMessage> callback)
        {
            UserCallback = callback;
        }
        public void SendMessage(MMCMessage message, NetConnection connection)
        {
            NetOutgoingMessage msg = this.CreateMessage();
            msg.Write(JsonConvert.SerializeObject(message));
            this.SendMessage(msg, connection, NetDeliveryMethod.ReliableOrdered);
        }
        public ClientValue GetConnectionInfo(NetConnection connection)
        {
            foreach (ClientValue c in DiscoveredClients)
            {
                if (c.Key == connection.RemoteEndPoint)
                    return c;
            }
            return new ClientValue(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0), new string [] {"UNKNOWN", "CLIENT"});
        }
        //Should it be a problem, filtering message based on the client's state?
        private NetIncomingMessage FilterMessage(NetIncomingMessage msg)
        {
            return msg;
        }
        private void MessageCallback(object peer)
        {
            //Pass the message to user defined method first then run defined functions
            //Filter message based on the mode client is in
            NetIncomingMessage msg = this.ReadMessage();
            if (UserCallback == null)
                throw new NotImplementedException();
            else
            {
                UserCallback(FilterMessage(msg));
            }
            switch (msg.MessageType)
            {
                case NetIncomingMessageType.DiscoveryResponse:
                    RespondDiscoverResponse(msg);
                    break;
                case NetIncomingMessageType.DiscoveryRequest:
                    RespondDiscover(msg);
                    break;
                case NetIncomingMessageType.Data:
                    MMCMessage message = JsonConvert.DeserializeObject<MMCMessage>(msg.ReadString());
                    if (message.Type.Contains(DataType.STATE))
                    {
                        this.pState = message.StateChange;
                    }
                    break;
            }
        }
    }
    public enum PeerState
    {
        LOCK,
        READY,
        RECIEVE,
        COMMAND,
    }
    public enum PeerType
    {
        CHARACTER,
        SFX,
        LIGHT,
        CONTROLLER
    }
    public class ClientValue : Object
    {
        public KeyValuePair<IPEndPoint, string[]> internalValue;
        public IPEndPoint Key
        {
            get
            {
                return internalValue.Key;
            }
        }
        public string[] Value
        {
            get
            {
                return internalValue.Value;
            }
        }
        public ClientValue(IPEndPoint ip, string[] val)
        {
            this.internalValue = new KeyValuePair<IPEndPoint, string[]>(ip, val);
        }
        public override int GetHashCode()
        {
            return internalValue.Key.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is ClientValue)
            {
                ClientValue val = (ClientValue)obj;
                if ((this.internalValue.Key.Address.ToString() == val.Key.Address.ToString()) &&
                    (this.internalValue.Key.Port == val.Key.Port))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
    public class MMCPeerConfig
    {
        public NetPeerConfiguration npConfig { get; set; }
        public PeerType type { get; set; }
        public string name { get; set; }
        public MMCPeerConfig(NetPeerConfiguration config)
        {
            npConfig = config;
        }
        public MMCPeerConfig(NetPeerConfiguration config, PeerType tp, string n)
        {
            npConfig = config;
            type = tp;
            name = n;
        }
    }
}
