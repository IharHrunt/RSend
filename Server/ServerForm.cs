using System;
using System.Runtime.Remoting;
using System.Windows.Forms;
using RSend;

namespace Server
{
    public partial class ServerForm : Form
    {
        public ServerObject ServerObj = null;
        
        public ServerForm()
        {
            InitializeComponent();
        }
        public ServerForm(ServerObject ServerObj)
        {
            InitializeComponent();
            this.ServerObj = ServerObj;
        }        
        
        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Do you want to shutdown Rsend Server?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
