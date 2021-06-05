using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApp
{
    public partial class MessageForm : Form
    {
        public MessageForm()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
        
                txtInfo.Text += $"Server :  { txtMessage.Text} {Environment.NewLine}";

                txtMessage.Text = string.Empty;
            }
        }
    }
}
