//Sauraav Jayrajh
//sauraavjayrajh@gmail.com
using Microsoft.Win32;
using System.Text.Json;
using System.IO;
using System.Windows;

namespace aiChatterBox
{
    public partial class inputBox : Window
    {
        //Get Directory Path on Device
        private string configDirectory = Directory.GetCurrentDirectory() + "\\configs\\";

        public inputBox()
        {
            InitializeComponent();
        }

        //Confirmation
        private void button_ConfirmChange_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.localhost = textBox_InputNewHost.Text;
            MessageBox.Show("Host has been set!");
            this.Close();
        }

        //Take Content From Textbox and Save it in Config File
        private void button_SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = configDirectory;
            saveFileDialog.Filter = "JSON Files (*.json)|*.json";
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.FileName = "default_address.json";

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string configFile = saveFileDialog.FileName;
                string configContent = JsonSerializer.Serialize(textBox_InputNewHost.Text);
                File.WriteAllText(configFile, configContent);
            }
        }

        //Take Content From Config File and Place it in Textbox
        private void button_LoadConfig_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = configDirectory;
            openFileDialog.Filter = "JSON Files (*.json)|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                string configContent = File.ReadAllText(openFileDialog.FileName);
                textBox_InputNewHost.Text = JsonSerializer.Deserialize<string>(configContent);
            }
        }
    }
}
