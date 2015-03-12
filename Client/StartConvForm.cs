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
        Dictionary<int, string> _loginUsers = new Dictionary<int, string>() { { 100, "בר גל" }, { 101, "בני בזומניק" }, { 102, "Leon" }, { 103, "Adi" } };
        private ChatForm frm = new ChatForm();
        List<User> users = new List<User>()
        {
            new User()
            {
                ID =int.Parse(Globals.USER_1),
                Name = "בני בזומניך",
                PictureURL = "http://images1.ynet.co.il/PicServer2/02022009/2078172/Spongbob.-Nickelodeon-chann&91;1&93;_wa.jpg"
            },
            new User()
            {
                ID =int.Parse(Globals.USER_2),
                Name = "אליה אבן צור",
                PictureURL = "http://www.themarker.com/polopoly_fs/1.1593408.1324041203!/image/3280629740.jpg_gen/derivatives/landscape_300/3280629740.jpg"
            },
            new User()
            {
                ID =int.Parse(Globals.USER_3),
                Name = "מיכאל שקסתא",
                PictureURL = "http://static1.squarespace.com/static/50067f0024ac21f35d8dc763/t/52776b91e4b0bef5deedfe53/1383558036774/10.png"
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
            listBox1.SelectedIndex = (listBox1.SelectedIndex - 1 + listBox1.Items.Count) % listBox1.Items.Count;
        }

        private void InstaceOnSwipeRightFired(PXCMHandData pxcmHandData)           
        {
            listBox1.SelectedIndex = (listBox1.SelectedIndex + 1) % listBox1.Items.Count;
        }

        private void InstaceOnTapFired(PXCMHandData pxcmHandData)
        {
            button1_Click(null, null);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pictureBox1.LoadAsync(((User)this.listBox1.SelectedItem).PictureURL);
        }
    }
}
