//Sauraav Jayrajh
//sauraavjayrajh@gmail.com
using System.Windows;
namespace aiChatterBox
{
    public partial class inputBox : Window
    {
        public inputBox()
        {
            InitializeComponent();
        }

        private void button_ConfirmChange_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.localhost = textBox_InputNewHost.Text;
            MessageBox.Show("Host changed");
            this.Close();
        }
    }
}
