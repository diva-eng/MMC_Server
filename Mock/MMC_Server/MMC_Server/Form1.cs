using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using Lidgren.Network;
using System.Net;
using SamplesCommon;
using MMC;

namespace MMC_Server
{
    public partial class Form1 : Form
    {
        private static MMCPeer s_server;
        public Form1()
        {
            InitializeComponent();
            Application.Idle += new EventHandler(Application_Idle);
        }
        private void Output(string text)
        {
            NativeMethods.AppendText(richTextBox1, text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("MMC");
            config.MaximumConnections = 100;
            config.Port = 3939;
            // Enable DiscoveryResponse messages
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

            MMCPeerConfig pConfig = new MMCPeerConfig(config, PeerType.CONTROLLER, "Controller1");

            s_server = new MMCPeer(pConfig);
            s_server.RegisterCallback(new Action<NetIncomingMessage>(GotMessage));
            Output("listening on " + config.Port.ToString());
            s_server.Start();
            

        }

        public void GotMessage(NetIncomingMessage msg)
        {
            //NetIncomingMessage msg = s_server.ReadMessage();
            if (msg == null)
                return;
            switch (msg.MessageType)
            {
                case NetIncomingMessageType.VerboseDebugMessage:
                    string text = msg.ReadString();
                    Output(text);
                   break;
            }
        }


        private void Application_Idle(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Emit a discovery signal
            Output("discovering clients");
            s_server.DiscoverLocalPeers(39393);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            s_server.Shutdown("bye");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Output("====Discovered Clients====");
            foreach (ClientValue i in s_server.DiscoveredClients)
            {
                Output("Client: " + i.Key);
                Output("Value: " + i.Value[0] + "@" + i.Value[1]);
                Output("Hashcode: " + i.GetHashCode());
            }
            Output("====End of the list====");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NetOutgoingMessage hail = s_server.CreateMessage("This is the hail message");
            foreach (ClientValue v in s_server.DiscoveredClients)
            {
                s_server.Connect(v.Key, hail);
                Output("Connected " + v.Key.Address + ":" + v.Key.Port + " " + v.Value[1]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Output("====Connections====");
            foreach (NetConnection c in s_server.Connections)
            {
                Output("Connection: " + NetUtility.ToHexString(c.RemoteUniqueIdentifier));
            }
            Output("====End Connections====");
        }
    }
}
