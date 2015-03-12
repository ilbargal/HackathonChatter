using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClientWPF.Utiity;

namespace ClientWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.lstUsers.Items.Add("Roy Atsmonn");
            this.lstUsers.Items.Add("Benny Bazumnik");
            this.lstUsers.Items.Add("Adi Chernitsky");

            RealSenseHandler.Instace.Start();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
