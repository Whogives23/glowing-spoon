//Sauraav Jayrajh
//sauraavjayrajh@gmail.com
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace aiChatterBox
{
    public partial class MainWindow : Window
    {
        //Client to Handle API Interactions
        private static readonly HttpClient client = new HttpClient();
        //Adress of Ollama Running On Machine, Change If Needed
        public static string localhost = "http://localhost:11434";
        //Store Messages Of Current Chat As List
        public List<string> currentChat = new List<string>();
        //To Indicate Start/Stop of Loading Animation
        private CancellationTokenSource currentCancellationTokenSource = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void button_PromptSubmit_Click(object sender, RoutedEventArgs e)
        {
            //Get and Validate Input
            string inputPrompt = textBox_PromptInput.Text;
            if (string.IsNullOrWhiteSpace(inputPrompt))
            {
                MessageBox.Show("Please enter a prompt");
                return;
            }
            //Add User Input to Chat
            currentChat.Add($"ME: {inputPrompt}");
            addMessageToListView(inputPrompt, Brushes.Pink, HorizontalAlignment.Right);
            textBox_PromptInput.Text = "";

            //Start Loading Animation
            if (currentCancellationTokenSource != null)
            {
                currentCancellationTokenSource.Cancel();
            }
            currentCancellationTokenSource = new CancellationTokenSource();
            var token = currentCancellationTokenSource.Token;
            showLoadingAnimation(token);

            //Get Response From Ollama, or Show Error
            try
            {
                string aiOutput = await promptOllamaAsync(inputPrompt);
                currentChat.Add($"AI: {aiOutput}");
                addMessageToListView(aiOutput, Brushes.LightBlue, HorizontalAlignment.Left);
            }
            catch (Exception ex)
            {
                addMessageToListView($"Error: {ex.Message}", Brushes.Red, HorizontalAlignment.Left);
            }
            finally
            {
                //Reset Values so Loading Animation can be Reused
                currentCancellationTokenSource.Cancel();
                currentCancellationTokenSource = null;
            }
        }

        //Calling Ollama
        private async Task<string> promptOllamaAsync(string userPrompt)
        {
            //Operation to Attatch to Ollama host
            string path = "/api/generate";
            string apiUrl = localhost + path;

            //Structure Request
            var requestBody = new
            {
                model = "llama3",
                prompt = userPrompt,
                stream = false
            };

            //Format and Send Request
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();

            //Return Response
            string responseString = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
            return jsonResponse.response;
        }

        private void addMessageToListView(string message, Brush background, HorizontalAlignment alignment)
        {
            //Declare New Message Block and Add To ListView
            TextBlock textBlock = new TextBlock
            {
                Text = message,
                Width = 450,
                Background = background,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                HorizontalAlignment = alignment
            };
            listView_currentChat.Items.Add(textBlock);
        }

        private async void showLoadingAnimation(CancellationToken token)
        {
            //Store Animation Frames
            string[] loadingStages = new string[] { ".", "..", "...", "...." };
            int stage = 0;
            //Declare Loading Animation and Add to ListView
            TextBlock loadingTextBlock = new TextBlock
            {
                Text = loadingStages[stage],
                Background = Brushes.Transparent,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            listView_currentChat.Items.Add(loadingTextBlock);

            //Play Animation
            while (!token.IsCancellationRequested)
            {
                foreach (var frame in loadingStages)
                {
                    loadingTextBlock.Text = frame;
                    await Task.Delay(500);
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }

            //Remove Animation When Done
            listView_currentChat.Items.Remove(loadingTextBlock);
        }

        private void button_changePath_Click(object sender, RoutedEventArgs e)
        {
            inputBox ib = new inputBox();
            ib.ShowDialog();
        }
    }
}