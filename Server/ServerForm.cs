using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            int i;
            this.axVideoChatServer1.InitServer(1234, 300);
            this.axVideoChatServer1.UseRandomNumber = false;
            this.axVideoChatServer1.CreateConference();

            // Add defualt users
            this.listBox1.Items.Add(Globals.USER_1);
            this.listBox1.Items.Add(Globals.USER_2);
            this.listBox1.Items.Add(Globals.USER_3);
            i = this.axVideoChatServer1.AddUser(Globals.CONF_NO);
            i = this.axVideoChatServer1.AddUser(Globals.CONF_NO);
            i = this.axVideoChatServer1.AddUser(Globals.CONF_NO);
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.axVideoChatServer1.UnInitServer();
        }

        private void axVideoChatServer1_ClientConnected(object sender, AxVideoChatServerLib._DVideoChatServerEvents_ClientConnectedEvent e)
        {
            string strUserID;
            
            strUserID = e.iUserID.ToString();
            
            
            int index = listBox1.Items.IndexOf(strUserID);
            if (index != -1)
            {
                listBox1.Items.RemoveAt(index);
                listBox1.Items.Insert(index, strUserID+"***");
                listBox1.SelectedIndex = index;
            }

            this.listBox1.Refresh();
        }

        private void axVideoChatServer1_ClientDisConnected(object sender, AxVideoChatServerLib._DVideoChatServerEvents_ClientDisconnectedEvent e)
        {
            string strUserID;
            string strListID;
            strUserID = e.iUserID.ToString();

            for (int i = 0; i < this.listBox1.Items.Count; i++)
            {
                strListID = this.listBox1.Items[i].ToString();
                if (strListID == (strUserID + "***"))
                {
                    this.listBox1.Items.RemoveAt(i);
                    this.listBox1.Items.Insert(i, strUserID);
                }
            }

            this.listBox1.Refresh();
        }

        private void axVideoChatServer1_AllClientDisconnected(object sender, AxVideoChatServerLib._DVideoChatServerEvents_AllClientDisconnectedEvent e)
        {
            MessageBox.Show("All client disconnected");
            this.listBox1.Items.Clear();
        }
    }
}

