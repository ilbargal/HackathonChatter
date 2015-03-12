using System.Collections.Generic;
using System.Windows.Forms;

namespace videochatsample.FaceRecognition
{
    public partial class NextWindow : Form
    {
        Dictionary<int,string> users = new Dictionary<int,string>(){{100,"michael"},{101,"Bar"},{102,"Leon"},{103,"Adi"}};


        public NextWindow()
        {
            InitializeComponent();
        }
        public NextWindow(int id)
        {
            InitializeComponent();
            label1.Text = users[id];
        }
    }
}
