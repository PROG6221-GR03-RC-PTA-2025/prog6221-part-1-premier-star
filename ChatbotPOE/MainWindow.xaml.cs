using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CyberSecurityChatbot
{
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer synth = new SpeechSynthesizer();
        private List<TaskItem> tasks = new List<TaskItem>();
        private List<string> activityLog = new List<string>();
        private int quizIndex = 0;
        private int score = 0;

        // Conversation stages to manage chat flow
        private enum ConversationStage
        {
            StartGreeting,
            WaitingForUserGreeting,
            AskPhishing,
            WaitPhishingResponse,
            AskPassword,
            WaitPasswordResponse,
            ReadyForCommands
        }
        private ConversationStage conversationStage = ConversationStage.StartGreeting;

        private List<QuizQuestion> quizQuestions = new List<QuizQuestion>
        {
            new QuizQuestion("What is phishing?", new List<string> {
                "A scam to steal personal info", "A game", "A security software", "An antivirus" }, 0),
            new QuizQuestion("True or False: Sharing passwords is safe.", new List<string> {
                "True", "False" }, 1),
            new QuizQuestion("What is two-factor authentication?", new List<string> {
                "Using two passwords", "Extra verification step", "A type of virus", "Firewall type" }, 1),
            new QuizQuestion("Best way to create a strong password?", new List<string> {
                "Use pet's name", "Short and simple", "123456", "Long with symbols & numbers" }, 3),
            new QuizQuestion("Which is a sign of a secure website?", new List<string> {
                "Starts with http", "No lock icon", "Starts with https", "It’s fast" }, 2),
            new QuizQuestion("True or False: Public Wi-Fi is always safe.", new List<string> {
                "True", "False" }, 1),
            new QuizQuestion("What should you do if you get a suspicious email?", new List<string> {
                "Reply to it", "Click all links", "Report as phishing", "Ignore and delete" }, 2),
            new QuizQuestion("What is malware?", new List<string> {
                "Good software", "Malicious software", "Firewall", "Antivirus" }, 1),
            new QuizQuestion("True or False: Antivirus updates are unnecessary.", new List<string> {
                "True", "False" }, 1),
            new QuizQuestion("Which is a good security practice?", new List<string> {
                "Reuse passwords", "Update devices regularly", "Share login with friends", "Ignore security alerts" }, 1)
        };

        public MainWindow()
        {
            InitializeComponent();

            // Start conversation when window loads
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartConversation();
        }

        private async void StartConversation()
        {
            BotSendMessage("Hello! I'm your Cybersecurity Awareness Bot.");
            await Task.Delay(1000);
            BotSendMessage("How are you doing today?");
            conversationStage = ConversationStage.WaitingForUserGreeting;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string userInput = txtUserInput.Text.Trim();
            if (string.IsNullOrEmpty(userInput)) return;

            lstChat.Items.Add("You: " + userInput);
            ProcessUserInput(userInput);
            txtUserInput.Clear();
        }

        private void ProcessUserInput(string input)
        {
            input = input.ToLower();

            switch (conversationStage)
            {
                case ConversationStage.WaitingForUserGreeting:
                    if (input.Contains("good") || input.Contains("fine") || input.Contains("well") || input.Contains("great"))
                    {
                        BotSendMessage("Glad to hear that!");
                    }
                    else if (input.Contains("bad") || input.Contains("not well") || input.Contains("sad"))
                    {
                        BotSendMessage("I'm sorry to hear that. Hope I can make your day better!");
                    }
                    else
                    {
                        BotSendMessage("Thanks for sharing!");
                    }
                    BotSendMessage("Let's start with a question: Do you know what phishing is?");
                    conversationStage = ConversationStage.WaitPhishingResponse;
                    break;

                case ConversationStage.WaitPhishingResponse:
                    if (input.Contains("yes") || input.Contains("know"))
                    {
                        BotSendMessage("Great! Phishing scams try to trick you into giving personal info.");
                    }
                    else if (input.Contains("no") || input.Contains("not"))
                    {
                        BotSendMessage("No worries! Phishing is when attackers pretend to be someone you trust to steal info.");
                    }
                    else
                    {
                        BotSendMessage("Phishing is a scam to steal your info through fake messages.");
                    }
                    BotSendMessage("How about passwords? Do you think your passwords are strong?");
                    conversationStage = ConversationStage.WaitPasswordResponse;
                    break;

                case ConversationStage.WaitPasswordResponse:
                    if (input.Contains("strong"))
                    {
                        BotSendMessage("Awesome! Strong passwords are your first line of defense.");
                    }
                    else if (input.Contains("weak"))
                    {
                        BotSendMessage("Try using longer passwords with symbols and numbers for better security.");
                    }
                    else
                    {
                        BotSendMessage("A strong password is long, unique, and hard to guess.");
                    }
                    BotSendMessage("You can ask me to add tasks, set reminders, start a quiz, or check your activity anytime.");
                    conversationStage = ConversationStage.ReadyForCommands;
                    break;

                case ConversationStage.ReadyForCommands:
                    if (input.Contains("add task") || input.Contains("set reminder") || input.Contains("remind me"))
                    {
                        lstChat.Items.Add("Bot: Please enter the task title, description, and reminder days in the Tasks tab.");
                    }
                    else if (input.Contains("quiz"))
                    {
                        quizIndex = 0;  // reset quiz progress
                        score = 0;
                        tabControl.SelectedIndex = 2;  // Switch to Quiz tab
                        lstChat.Items.Add("Bot: Starting the quiz! Go to the Quiz tab.");
                        AddToLog("Quiz started.");
                        LoadNextQuizQuestion();  // Load first question
                    }
                    else if (input.Contains("show activity") || input.Contains("what have you done"))
                    {
                        tabControl.SelectedIndex = 3;
                        lstChat.Items.Add("Bot: Showing your activity log.");
                    }
                    else if (input.Contains("hello") || input.Contains("hi") || input.Contains("hey"))
                    {
                        BotSendMessage("Hello again! How can I help you today?");
                    }
                    else
                    {
                        BotSendMessage("I'm here to help with tasks, reminders, quizzes, or activity logs. What would you like to do?");
                    }
                    break;

                default:
                    BotSendMessage("Let's get started! How are you today?");
                    conversationStage = ConversationStage.WaitingForUserGreeting;
                    break;
            }
        }

        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTaskTitle.Text.Trim();
            string description = txtTaskDescription.Text.Trim();
            int days;

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) ||
                !int.TryParse(txtReminderDays.Text.Trim(), out days))
            {
                MessageBox.Show("Please enter valid task details and reminder (number of days).");
                return;
            }

            TaskItem task = new TaskItem
            {
                Title = title,
                Description = description,
                ReminderDate = DateTime.Now.AddDays(days)
            };

            tasks.Add(task);
            lstTasks.Items.Add($"{task.Title} - {task.Description} (Remind in {days} days)");
            lstChat.Items.Add($"Bot: Task added: '{task.Title}'. Reminder set for {task.ReminderDate.ToShortDateString()}");
            AddToLog($"Task added: '{task.Title}' (Reminder in {days} days)");
            ClearTaskInputs();
        }

        private void ClearTaskInputs()
        {
            txtTaskTitle.Clear();
            txtTaskDescription.Clear();
            txtReminderDays.Clear();
        }

        private void LoadNextQuizQuestion()
        {
            if (quizIndex >= quizQuestions.Count)
            {
                lstAnswers.Items.Clear();
                txtQuestion.Text = $"Quiz completed! You scored {score}/{quizQuestions.Count}.";
                txtQuizFeedback.Text = score >= 7 ? "Great job! You're a cybersecurity pro!" : "Keep learning to stay safe online!";
                AddToLog($"Quiz completed. Final score: {score}");
                return;
            }

            QuizQuestion question = quizQuestions[quizIndex];
            txtQuestion.Text = question.QuestionText;
            lstAnswers.Items.Clear();
            foreach (var option in question.Options)
                lstAnswers.Items.Add(option);
        }

        private void btnSubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (lstAnswers.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an answer.");
                return;
            }

            QuizQuestion question = quizQuestions[quizIndex];
            if (lstAnswers.SelectedIndex == question.CorrectAnswerIndex)
            {
                txtQuizFeedback.Text = "Correct!";
                score++;
                AddToLog($"Quiz Q{quizIndex + 1}: Correct");
            }
            else
            {
                txtQuizFeedback.Text = $"Wrong! Correct answer: {question.Options[question.CorrectAnswerIndex]}";
                AddToLog($"Quiz Q{quizIndex + 1}: Incorrect");
            }

            quizIndex++;
            LoadNextQuizQuestion();
        }

        private void AddToLog(string message)
        {
            string logEntry = $"{DateTime.Now:HH:mm:ss} - {message}";
            activityLog.Add(logEntry);
            if (activityLog.Count > 10)
                activityLog.RemoveAt(0);

            lstActivityLog.Items.Clear();
            foreach (var entry in activityLog)
                lstActivityLog.Items.Add(entry);
        }

        private void BotSendMessage(string message)
        {
            lstChat.Items.Add("Bot: " + message);
            try
            {
                synth.SpeakAsync(message);
            }
            catch
            {
                // Ignore speech synthesis errors
            }
            AddToLog("Bot: " + message);
        }
    }

    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReminderDate { get; set; }
    }

    public class QuizQuestion
    {
        public string QuestionText { get; }
        public List<string> Options { get; }
        public int CorrectAnswerIndex { get; }

        public QuizQuestion(string question, List<string> options, int correctIndex)
        {
            QuestionText = question;
            Options = options;
            CorrectAnswerIndex = correctIndex;
        }
    }
}
