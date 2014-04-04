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
using SamplesCommon;

namespace MMC_Server
{
    public partial class Form1 : Form
    {
        private static NetPeer s_server;
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

            s_server = new NetPeer(config);
            Output("listening on " + config.Port.ToString());
            s_server.Start();
            

        }


        private void Application_Idle(object sender, EventArgs e)
        {
            if(s_server != null)
            while (NativeMethods.AppStillIdle)
            {
                NetIncomingMessage im;
                while ((im = s_server.ReadMessage()) != null)
                {
                    // handle incoming message
                    switch (im.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryResponse:
                            Output("Found client!");
                            string[] id = im.ReadString().Split('@');
                            switch (id[0])
                            {
                                case "character":
                                    Output("Found Character Client: " + id[1]);
                                    break;
                            }
                            break;
                        default:
                            Output("Unhandled type: " + im.MessageType + " " + im.LengthBytes + " bytes " + im.DeliveryMethod + "|" + im.SequenceChannel);
                            break;
                    }
                }
            }
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
    }
}
