using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ClientWPF.Utiity;
using FrameWork;
using videochatsample.Utiity;

namespace videochatsample
{
    public partial class StartConvForm : Form
    {
        Dictionary<int, string> _loginUsers = new Dictionary<int, string>() { { 100, "רועי עצמון" }, { 101, "בני בזומניק" }, { 102, "עדי צ'רניצקי" }, { 103, "Adi" } };

        private ChatForm frm = new ChatForm();
        List<User> users = new List<User>()
        {
            new User()
            {
                ID =int.Parse(Globals.USER_1),
                Name = "בני בזומניך",
                PictureURL = @"C:\Users\Hackathon-IDF\Desktop\Reps\HackathonChatter\Client\Pics\Benny.jpg"
            },
            new User()
            {
                ID =int.Parse(Globals.USER_2),
                Name = "אליה אבן צור",
                PictureURL = @"C:\Users\Hackathon-IDF\Desktop\Reps\HackathonChatter\Client\Pics\Eliya.jpg"
            },
            new User()
            {
                ID =int.Parse(Globals.USER_3),
                Name = "מיכאל שקסתא",
                PictureURL = @"C:\Users\Hackathon-IDF\Desktop\Reps\HackathonChatter\Client\Pics\Michael.jpg"
            },
        };

        public StartConvForm(int userID)
        {
            InitializeComponent();
            listBox1.Items.AddRange(users.ToArray());
            listBox1.DisplayMember =  "Name";
            RealSenseHandler.Instace.Start();
            this.Text = "שלום " + _loginUsers[userID];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            RealSenseHandler.Instace.SwipeLeftFired -= InstaceOnSwipeLeftFired;
            RealSenseHandler.Instace.SwipeRightFired -= InstaceOnSwipeRightFired;
            RealSenseHandler.Instace.TapFired -= InstaceOnTapFired;
            frm.textUserID.Text = ((User)listBox1.SelectedItem).ID.ToString();
            frm.Show();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
            RealSenseHandler.Instace.SwipeLeftFired += InstaceOnSwipeLeftFired;
            RealSenseHandler.Instace.SwipeRightFired += InstaceOnSwipeRightFired;
            RealSenseHandler.Instace.TapFired += InstaceOnTapFired;
        }

        private void InstaceOnSwipeLeftFired(PXCMHandData pxcmHandData)
        {
            this.BeginInvoke((Action)(()=>
            listBox1.SelectedIndex = (listBox1.SelectedIndex - 1 + listBox1.Items.Count) % listBox1.Items.Count));
        }

        private void InstaceOnSwipeRightFired(PXCMHandData pxcmHandData)           
        {
            this.BeginInvoke((Action)(()=>
            listBox1.SelectedIndex = (listBox1.SelectedIndex + 1) % listBox1.Items.Count));
        }

        private void InstaceOnTapFired(PXCMHandData pxcmHandData)
        {
            this.BeginInvoke((Action)(()=>
            button1_Click(null, null)));
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pictureBox1.ImageLocation = (listBox1.SelectedItem as User).PictureURL;
        }
    }
}
