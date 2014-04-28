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
using MDXLib.FileUtil;

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
            s_server.RegisterCallback(new Action<MMCMessage>(GotMessage));
            s_server.Start();
        }

        public void GotMessage(MMCMessage message)
        {
            //NetIncomingMessage msg = s_server.ReadMessage();
            if (message == null)
                return;
            Output(message.ToString());
            if (message.Type.Contains(DataType.CHARACTER) && s_server.pConfig.type == PeerType.CHARACTER)
            {
                MMCCharacter character = message.CharacterData;
                pictureBox1.Load(character.PreviewImage.ToString());
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
