using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lidgren.Network;

namespace MMC
{
    class MMCPeer : NetPeer
    {
        public MMCPeerConfig pConfig { get; set; }
        public MMCPeer(MMCPeerConfig config) : base (config.npConfig)
        {
            pConfig = config;
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
    }
    enum PeerType
    {
        CHARACTER,
        SFX,
        LIGHT,
        CONTROLLER
    }
    class MMCPeerConfig
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
