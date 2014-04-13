using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

using Lidgren.Network;

namespace MMC
{
    public class MMCPeer : NetPeer
    {
        public MMCPeerConfig pConfig { get; set; }
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
        private void MessageCallback(object peer)
        {
            NetIncomingMessage msg = this.ReadMessage();
            if (UserCallback == null)
                throw new NotImplementedException();
            else
                UserCallback(msg);
            switch (msg.MessageType)
            {
                case NetIncomingMessageType.DiscoveryResponse:
                    RespondDiscoverResponse(msg);
                    break;
                case NetIncomingMessageType.DiscoveryRequest:
                    RespondDiscover(msg);
                    break;
            }
        }
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
