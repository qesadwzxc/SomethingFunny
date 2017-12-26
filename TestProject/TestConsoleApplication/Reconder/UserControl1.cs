using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;


namespace TestConsoleApplication.Reconder
{
    public partial class record : UserControl
    {
        public record()
        {
            InitializeComponent();
        }
        SoundRecord recorder;
        string path1 = @"\mchinese\";
        string wavname = "rec.wav";
        private string path;
        SmSound ss = new SmSound();

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
                if (path == null) {
                    path = path1 + wavname;
                }
                return path;
            }
            set
            {
                
               path=value;
            }
        }

        
        private byte[] playsound=null;

        /// <summary>
        ///声音byte数组
        /// </summary>
        /// <value>声音byte数组</value>
        [Description("声音byte数组")]
        [DisplayName("PlaySound")]
    
        public byte[] PlaySound {
            get {
                return playsound;
            }
            set {
                playsound =value ;
            }
        }


        private void start_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.ToString()))
            {
                Directory.CreateDirectory(path1);
                File.Open(wavname,FileMode.Create);
                 
            }
     
            play.Enabled = true;
            button1.Enabled = true;
            stop.Enabled = true;
            start.Enabled = false;
            recorder = new SoundRecord();
            recorder.SetFileName(Path.ToString());
            recorder.RecStart();
            
        }

        private void stop_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.ToString()))
            {
                play.Enabled = true;
                button1.Enabled = true;
                stop.Enabled = false;
                start.Enabled = true;
                if (recorder != null)
                {
                    recorder.RecStop();
                    recorder = null;
                }  
            }
            
        }

        private void play_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.ToString()))
            {
                if (recorder != null) {
                    recorder.RecStop();
                    recorder = null;
                }                
                playwav.Play(Path.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {      
            if (File.Exists(Path.ToString()))
            {
                if (recorder != null)
                {
                    recorder.RecStop();
                    recorder = null;
                }  
                               
                ss.Play(PlaySound,Path.ToString());
           
            }
          
        }


    }
}