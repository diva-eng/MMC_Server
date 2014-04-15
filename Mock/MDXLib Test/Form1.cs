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
using MDXLib.Data;
using MDXLib.File;

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
            
            MDXFile motion = parser.GetFile(textBox1.Text);
            richTextBox1.Text = motion.ToString();
            pictureBox1.Load(motion.GetPreview().ToString());
            MDXFileUtil.Download(motion);
        }
    }
}
