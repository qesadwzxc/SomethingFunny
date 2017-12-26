using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestConsoleApplication.Reconder
{
    public partial class Reconder : Form
    {
        public Reconder()
        {
            InitializeComponent();
        }

        SoundRecord recorder;
        string path1 = @"\mchinese\";
        string wavname = "rec.wav";
        private string path;

        /// <summary>
        ///录音文件路径
        /// </summary>
        /// <value>录音文件路径</value>
        [Description("录音文件路径")]
        [DisplayName("Path")]
        public string Path
        {
            get
            {
                if (path == null)
                {
                    path = path1 + wavname;
                }
                return path;
            }
            set
            {

                path = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.ToString()))
            {
                Directory.CreateDirectory(path1);
                File.Open(wavname, FileMode.Create);

            }
            
            button1.Enabled = false;
            button2.Enabled = true;
            recorder = new SoundRecord();
            recorder.SetFileName(Path.ToString());
            recorder.RecStart();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.ToString()))
            {
                button1.Enabled = true;
                button2.Enabled = false;
                if (recorder != null)
                {
                    recorder.RecStop();
                    recorder = null;
                }
            }
        }

        private void rec_Load(object sender, EventArgs e)
        {

        }
    }
}
