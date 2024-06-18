# Glowing Spoon Chatter Box - Frontend for Llama 3

## Application Version: 1.1
## Developed by: Sauraav Jayrajh

---

## Developer Contact Details: 

- **Phone:** 071 924 4175
- **Email:** sauraavjayrajh@gmail.com

---

## Description:

Glowing Spoon Chatter Box is a frontend GUI application designed for interaction with the Llama 3 AI model running locally on your machine. The application provides a user-friendly interface to send prompts and receive responses from the AI and manage conversation histories.

---

## Getting Started:

### Software Requirements:
- .NET Framework
- Ollama Llama 3
- Windows Operating System

### Hardware Requirements:
- 8GB of RAM (For Llama 3 Base Model)
- 25MB of Free Storage

---

## Installation Instructions:

1. Clone this repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution and run the application.

---

## Usage:

Make sure Ollama is running on your machine when using this application. 
Upon running the application, you will see the main interface which includes the following components:

- **Chat Log Area:** Displays your active chat session.
- **Prompt Input Area:** Where you can type and submit your prompts to the AI.
- **Past Chats List:** Maintains a list of previous chat sessions.
- **Path Configuration Button:** Allows you to change the address of the Llama AI server.

### Basic Workflow:

1. **Starting a Chat:**
   - Enter your prompt in the input area and either click "Submit" or press "Enter".
   - The prompt and response from the Llama AI will appear in the chat log area.

2. **Managing Chats:**
   - New chat sessions can be started by clicking the "New Chat" button.
   - Previous chat sessions can be selected from the "Past Chats" list to review the interactions.
   - Use the "Clear Chats" button to delete all previous chat history.

3. **Changing Server Address:**
   - Click the button to open the configuration dialog where you can enter a new server address.
   - You can also save, load, and set default configurations for the server address.

4. **Visual Cues:**
   - User messages are displayed in teal and aligned to the right.
   - AI messages are displayed in dark blue and aligned to the left.
   - Error messages are displayed in crimson red.

---

## FAQ:

### General

**Q: How do I change the AI server address?**  
**A:** Click on the "Path Configuration" button, enter the new address, and confirm. You can also save this configuration or set it as the default address.

**Q: How do I save and load server configurations?**  
**A:** Use the "Save Configuration" button to save the current address and the "Load Configuration" button to load a saved address.

**Q: What happens if the AI server is unreachable?**  
**A:** If the server is unreachable, the application will display an error message in the chat log.

### Chats

**Q: How do I start a new chat session?**  
**A:** Click the "New Chat" button to clear the current session and start a new one.

**Q: How do I clear all chat history?**  
**A:** Use the "Clear Chats" button to remove all previous chat sessions.

**Q: Can I review past chat sessions?**  
**A:** Yes, select any session from the "Past Chats" list to review the chat. You can even continue the chat.

---

## Plugins and Frameworks Used:

- **System.Text.Json**
- **System.Net.Http**

---

## Attributions/References:

- **Llama AI:** [https://github.com/ollama/ollama/tree/main/docs]
- **.NET Framework Documentation:** [https://docs.microsoft.com/dotnet]

---

## Demos:

[Demo of Initial Attempt](https://github.com/Saupernova13/glowing-spoon/blob/main/docs/Sauraav_Jayrajh_Derivco_MagicSpoon_Demo.mp4)

[Demo of Updated and Requested Features](https://github.com/Saupernova13/glowing-spoon/blob/main/docs/Sauraav_Jayrajh_Derivco_MagicSpoon_Demo_2_Requested_Changes.mp4)

---
