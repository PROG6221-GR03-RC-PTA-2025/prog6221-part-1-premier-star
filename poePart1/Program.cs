using System;
using System.Speech.Synthesis;
using System.Threading;

class CyberSecurityChatbot
{
    static void Main()
    {
        // Voice Greeting using Text-to-Speech
        SpeakGreeting();

        // Display ASCII Art Logo
        DisplayAsciiArt();

        // Greet and interact with user
        Console.Write("Enter your name: ");
        string userName = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(userName))
        {
            Console.WriteLine("Name cannot be empty. Please enter your name:");
            userName = Console.ReadLine();
        }

        Console.Clear();
        Console.WriteLine($"Hello, {userName}! I'm here to help you stay safe online.");
        Console.WriteLine("Type your question or type 'exit' to quit.");

        // Chat loop
        while (true)
        {
            Console.Write("\nYou: ");
            string userInput = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Chatbot: Please enter a valid question.");
                continue;
            }

            if (userInput == "exit")
            {
                Console.WriteLine("Chatbot: Stay safe online! Goodbye.");
                break;
            }

            // Get chatbot response
            string response = GetResponse(userInput);
            DisplayTypingEffect("Chatbot: " + response);
        }
    }

    // Function to speak greeting using SpeechSynthesizer
    static void SpeakGreeting()
    {
        try
        {
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                synth.SelectVoiceByHints(VoiceGender.Female);
                synth.Speak("Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: Text-to-speech not supported. " + ex.Message);
        }
    }

    // Function to display ASCII art
    static void DisplayAsciiArt()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
   ____            _       _    ____        _   
  / ___|___  _ __| |_    / \  |  _ \  __ _| |_ 
 | |   / _ \| '__| __|  / _ \ | | | |/ _` | __|
 | |__| (_) | |  | |_  / ___ \| |_| | (_| | |_ 
  \____\___/|_|   \__|/_/   \_\____/ \__,_|\__|
");
        Console.ResetColor();
    }

    // Function to get chatbot response
    static string GetResponse(string input)
    {
        if (input.Contains("phishing"))
            return "Phishing is an attempt to steal your information by pretending to be a trusted entity.";
        if (input.Contains("password"))
            return "Use a mix of uppercase, lowercase, numbers, and symbols for strong passwords.";
        if (input.Contains("safe browsing"))
            return "Always check the website URL before entering sensitive information.";
        if (input.Contains("how are you"))
            return "I'm just a bot, but I'm here to help!";
        if (input.Contains("what's your purpose"))
            return "I educate users about cybersecurity threats and best practices.";

        return "I didn't quite understand that. Could you rephrase?";
    }

    // Function to simulate typing effect
    static void DisplayTypingEffect(string message)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(30); // Simulate typing delay
        }
        Console.WriteLine();
    }
}
