using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestConsoleApplication
{
    public partial class TextEditor : Form
    {
        public TextEditor()
        {
            InitializeComponent();
        }

        private void txtAutoComplete_TextChanged(object sender, EventArgs e)
        {
            string text = txtAutoComplete.Text;
            txtAutoComplete.AutoCompleteMode = AutoCompleteMode.Append;
            txtAutoComplete.AutoCompleteSource = AutoCompleteSource.AllSystemSources;
            
        }

        //private readonly Dictionary<int, string> AutoCompleteString()
        //{
        //    Dictionary<int, string> dic = new Dictionary<int, string>();
        //    dic.Add(0,"int");
        //    return dic;
        //}
    }
}
