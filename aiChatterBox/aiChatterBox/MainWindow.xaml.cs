//Sauraav Jayrajh
//sauraavjayrajh@gmail.com
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace aiChatterBox
{
    public partial class MainWindow : Window
    {
        //Client to Handle API Interactions
        private static readonly HttpClient client = new HttpClient();
        //Adress of Ollama Running On Machine, Change if Needed
        public static string localhost = "http://localhost:11434";
        //Store Chats
        private List<List<string>> chats = new List<List<string>>();
        //Store current chat index
        private int currentChatIndex = -1;
        //To Indicate Start/Stop of Loading Animation
        private CancellationTokenSource currentCancellationTokenSource = null;

        //Colours for Chat Messages
        public Brush userMessageColour = Brushes.Teal;
        public Brush llamaMessageColour = Brushes.DarkBlue;
        public Brush errorMessageColour = Brushes.Crimson;

        //File to Store Chats
        private string chatsFilePath = "pastChats.json";

        public MainWindow()
        {
            InitializeComponent();
            LoadChatsFromFile();
        }

        //Reuseable Method to Handle Submission of Prompt
        public async void submitPrompt()
        {

            //Get and Validate Input
            string inputPrompt = textBox_PromptInput.Text;
            if (string.IsNullOrWhiteSpace(inputPrompt))
            {
                MessageBox.Show("Please enter a prompt");
                return;
            }

            //Ensure current chat exists
            if (currentChatIndex == -1)
            {
                currentChatIndex = chats.Count;
                chats.Add(new List<string>());
                listView_PastChats.Items.Add($"             Chat 1");
            }

            //Add User Input to Chat
            chats[currentChatIndex].Add($"ME: {inputPrompt}");
            addMessageToListView("ME: " + inputPrompt, userMessageColour, HorizontalAlignment.Right);
            textBox_PromptInput.Text = "";

            // Start Loading Animation
            if (currentCancellationTokenSource != null)
            {
                currentCancellationTokenSource.Cancel();
            }
            currentCancellationTokenSource = new CancellationTokenSource();
            var token = currentCancellationTokenSource.Token;
            showLoadingAnimation(token);
            string aiOutput = "Error: An error has occured";
            //Get Response From Ollama, or Show Error
            try
            {
                aiOutput = await promptOllamaAsync(inputPrompt);
                chats[currentChatIndex].Add($"AI: {aiOutput}");
                addMessageToListView("AI: " + aiOutput, llamaMessageColour, HorizontalAlignment.Left);
            }
            catch (Exception ex)
            {
                chats[currentChatIndex].Add($"Error: {ex.Message}");
                addMessageToListView($"Error: {ex.Message}", errorMessageColour, HorizontalAlignment.Left);
            }
            finally
            {
                //Reset Values so Loading Animation can be Reused
                currentCancellationTokenSource.Cancel();
                currentCancellationTokenSource = null;

                //Save chats to file after each interaction
                SaveChatsToFile();
            }
        }

        //Button Click to Submit Prompt
        private async void button_PromptSubmit_Click(object sender, RoutedEventArgs e)
        {
            submitPrompt();
        }

        // Save chats to file
        private void SaveChatsToFile()
        {
            string chatsJson = JsonConvert.SerializeObject(chats);
            File.WriteAllText(chatsFilePath, chatsJson);
        }

        // Load chats from file
        private void LoadChatsFromFile()
        {
            if (File.Exists(chatsFilePath))
            {
                string chatsJson = File.ReadAllText(chatsFilePath);
                chats = JsonConvert.DeserializeObject<List<List<string>>>(chatsJson);
                for (int i = 0; i < chats.Count; i++)
                {
                    listView_PastChats.Items.Add($"             Chat {i + 1}");
                }
            }
        }

        //Calling Ollama
        private async Task<string> promptOllamaAsync(string userPrompt)
        {
            //Operation to Attach to Ollama host
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

        //Add Message Block to Chat ListView
        private void addMessageToListView(string message, Brush background, HorizontalAlignment alignment)
        //Declare New Message Block and Add To ListView
        {
            TextBlock textBlock = new TextBlock
            {
                Text = message,
                Width = 450,
                Background = background,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                HorizontalAlignment = alignment
            };
            listView_currentChat.Items.Add(textBlock);
        }

        //Display Loading Animation
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

        //Open Window to Change Ollama Path
        private void button_changePath_Click(object sender, RoutedEventArgs e)
        {
            inputBox ib = new inputBox();
            ib.ShowDialog();
        }

        private void listView_PastChats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listView_PastChats.SelectedItem == null)
            {
                return;
            }
            //Set current chat index and refresh chat view
            currentChatIndex = listView_PastChats.SelectedIndex;
            listView_currentChat.Items.Clear();
            foreach (var message in chats[currentChatIndex])
            {
                switch (message)
                {
                    //Populate User Message
                    case var _ when message.StartsWith("ME:"):
                        addMessageToListView(message, userMessageColour, HorizontalAlignment.Right);
                        break;
                    //Populate AI Message
                    case var _ when message.StartsWith("AI:"):
                        addMessageToListView(message, llamaMessageColour, HorizontalAlignment.Left);
                        break;
                    //Populate Error Message
                    case var _ when message.StartsWith("Error:"):
                        addMessageToListView(message, errorMessageColour, HorizontalAlignment.Left);
                        break;
                }
            }
        }

        private void button_NewChat_Click(object sender, RoutedEventArgs e)
        {
            //Create a New Chat and Clear the Current Chat View
            currentChatIndex = chats.Count;
            chats.Add(new List<string>());

            //Add the new chat to the past chats list and select it
            int numChats = listView_PastChats.Items.Count + 1;
            string newChatLabel = $"             Chat {numChats}";
            listView_PastChats.Items.Add(newChatLabel);
            listView_PastChats.SelectedItem = newChatLabel;
        }

        //Clear All Chat History
        private void button_ClearChats_Click(object sender, RoutedEventArgs e)
        {
            chats.Clear();
            listView_currentChat.Items.Clear();
            listView_PastChats.Items.Clear(); SaveChatsToFile();
            currentChatIndex = -1;
        }

        //Clear Searchbox
        private void textBox_PromptInput_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            textBox_PromptInput.Clear();
        }

        //If Enter is Pressed, Submit Text
        private void textBox_PromptInput_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                submitPrompt();
            }
        }
    }
}