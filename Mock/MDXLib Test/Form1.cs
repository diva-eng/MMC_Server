using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MDXLib.API;

namespace MDXLib_Test
{
    public partial class Form1 : Form
    {
        private MDXParser parser;
        public Form1()
        {
            InitializeComponent();
            parser = new MDXParser("dasdas");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = parser.GetFile(textBox1.Text);
        }
    }
}
