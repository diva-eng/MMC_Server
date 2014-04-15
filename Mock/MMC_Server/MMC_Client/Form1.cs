using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

using Lidgren.Network;
using SamplesCommon;
using MMC;
using MDXLib.API;
using MDXLib.Data;
using Newtonsoft.Json;

namespace MMC_Client
{
    public partial class Form1 : Form
    {
        private static MMCPeer s_server;
        private static MDXParser parser;
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            parser = new MDXParser("api");
        }

        private void Output(string text)
        {
            NativeMethods.AppendText(richTextBox1, text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("MMC");
            config.MaximumConnections = 100;
            config.Port = (int)numericUpDown1.Value;
            // Enable DiscoveryResponse messages
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.AcceptIncomingConnections = true;

            MMCPeerConfig pConfig = new MMCPeerConfig(config, (PeerType)comboBox1.SelectedIndex, textBox1.Text);

            s_server = new MMCPeer(pConfig);
            Output("listening on " + config.Port.ToString());
            s_server.RegisterCallback(new Action<NetIncomingMessage>(GotMessage));
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
                case NetIncomingMessageType.WarningMessage:
                    string text = msg.ReadString();
                    Output(text);
                   break;
                case NetIncomingMessageType.StatusChanged:
                   NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                   string reason = msg.ReadString();
                   Output(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " " + status + ": " + reason);
                   break;
                case NetIncomingMessageType.Data:
                   MMCMessage message = JsonConvert.DeserializeObject<MMCMessage>(msg.ReadString());
                   if (message.Type.Contains(DataType.CHARACTER))
                   {
                       MMCCharacter character = message.CharacterData;
                       MDXFile model = parser.GetFile(character.ModelAPI);
                       pictureBox1.Load(model.GetPreview().ToString());
                   }
                   break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            s_server.Shutdown("bye");
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }
    }
}
