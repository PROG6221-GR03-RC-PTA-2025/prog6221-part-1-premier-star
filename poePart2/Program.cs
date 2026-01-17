using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Threading;

class CyberSecurityChatbot
{
    static string userName = "";
    static string userInterest = "";
    static SpeechSynthesizer synth = new SpeechSynthesizer();
    static Random random = new Random();

    // Keyword and response mappings
    static Dictionary<string, string[]> keywordResponses = new Dictionary<string, string[]>()
    {
        { "password", new string[] {
            "Use strong, unique passwords for every account.",
            "Avoid using personal information like birthdays in your passwords.",
            "Consider using a password manager to generate and store secure passwords."
        }},
        { "phishing", new string[] {
            "Be cautious of emails asking for personal information.",
            "Check email addresses carefully; scammers often use lookalike domains.",
            "Never click suspicious links; hover over them to preview the URL."
        }},
        { "privacy", new string[] {
            "Review privacy settings on your social media and online accounts.",
            "Be mindful of the personal information you share online.",
            "Use secure and private browsers to protect your digital footprint."
        }},
        { "scam", new string[] {
            "Scammers often pressure victims into making quick decisions.",
            "Verify calls or emails by contacting the organization directly.",
            "Remember, if it sounds too good to be true, it probably is."
        }}
    };

    // Sentiment keywords
    static Dictionary<string, string> sentimentResponses = new Dictionary<string, string>()
    {
        { "worried", "It's completely understandable to feel that way. Scammers can be tricky. Let me share some tips to help you stay safe." },
        { "frustrated", "Cybersecurity can feel overwhelming, but small steps make a big difference. I'm here to assist you." },
        { "curious", "Curiosity is a great start! Let's explore some cybersecurity tips together." }
    };

    static void Main()
    {
        SpeakGreeting();
        DisplayAsciiArt();

        Console.Write("Enter your name: ");
        userName = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(userName))
        {
            Console.WriteLine("Name cannot be empty. Please enter your name:");
            userName = Console.ReadLine();
        }

        Console.Clear();
        Console.WriteLine($"Hello, {userName}! I'm here to help you stay safe online.");
        Console.WriteLine("You can ask about passwords, phishing, privacy, scams, or share your concerns.");
        Console.WriteLine("Type 'exit' to quit at any time.");

        while (true)
        {
            Console.Write("\nYou: ");
            string userInput = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Chatbot: Please enter a valid message.");
                continue;
            }

            if (userInput == "exit")
            {
                Console.WriteLine("Chatbot: Stay safe, " + userName + "! Goodbye.");
                break;
            }

            ProcessUserInput(userInput);
        }
    }

    static void ProcessUserInput(string input)
    {
        bool responded = false;

        // Sentiment detection
        foreach (var sentiment in sentimentResponses)
        {
            if (input.Contains(sentiment.Key))
            {
                DisplayTypingEffect("Chatbot: " + sentiment.Value);
                responded = true;
                break;
            }
        }

        // Keyword recognition
        foreach (var keyword in keywordResponses)
        {
            if (input.Contains(keyword.Key))
            {
                string response = keyword.Value[random.Next(keyword.Value.Length)];
                DisplayTypingEffect($"Chatbot: {response}");
                // Remember interest if applicable
                if (keyword.Key == "privacy" || keyword.Key == "phishing" || keyword.Key == "password")
                {
                    userInterest = keyword.Key;
                    DisplayTypingEffect($"Chatbot: I'll remember that you're interested in {keyword.Key}.");
                }
                responded = true;
                break;
            }
        }

        // Recall user interest if available and user asks for more info
        if (!responded && input.Contains("more") && !string.IsNullOrEmpty(userInterest))
        {
            DisplayTypingEffect($"Chatbot: Since you're interested in {userInterest}, remember to stay updated with the latest {userInterest} practices!");
            responded = true;
        }

        // Default response
        if (!responded)
        {
            DisplayTypingEffect("Chatbot: I'm not sure I understand. Could you try rephrasing?");
        }
    }

    static void SpeakGreeting()
    {
        try
        {
            synth.SelectVoiceByHints(VoiceGender.Female);
            synth.Speak("Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error with text-to-speech: " + ex.Message);
        }
    }

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

    static void DisplayTypingEffect(string message)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(30);
        }
        Console.WriteLine();
    }
}
