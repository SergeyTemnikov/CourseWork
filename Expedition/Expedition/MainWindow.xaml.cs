using Expedition.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;

namespace Expedition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameModel gameModel;

        private bool IsGameUpload;

        private string textHistory;

        public MainWindow()
        {
            InitializeComponent();

            StartApp();
            OutputTextBox.ScrollToEnd();
        }

        private void StartApp()
        {
            string helloWords = "Здравствуйте, вы можете ввести:\n - Новая игра\n ";

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "savegame.json");
            if (File.Exists(filePath))
            {
                helloWords += "- Загрузить игру\n ";
            }

            helloWords += "- Выйти\n";

            OutputTextBox.AppendText(helloWords);
        }

        private void UpdateOutput(string message)
        {
            if (IsGameUpload)
            {
                GameUploadOutput(message);
            }
            else
            {
                NonGameUploadOutput(message);
            }
            OutputTextBox.ScrollToEnd();
        }

        private void GameUploadOutput(string message)
        {
            string response = gameModel.ProcessCommand(message);
            if (response.StartsWith("Вы умерли!"))
            {
                textHistory = response;
                OutputTextBox.AppendText(response);
            } 
            else
            {
                var textToPlayer = $"Вы: {message}\n\n{response}\n";
                OutputTextBox.AppendText(textToPlayer);
                textHistory += textToPlayer;
            }
            txtPlayesStats.Text = gameModel.Player.CurrentStats;
            if (response.EndsWith("Игра окончена!"))
            {
                string helloWords = "Вы можете ввести:\n - Новая игра\n ";

                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "savegame.json");
                if (File.Exists(filePath))
                {
                    helloWords += "- Загрузить игру\n ";
                }

                helloWords += "- Выйти\n";

                OutputTextBox.AppendText(helloWords);

                IsGameUpload = false;
            }
        }

        private void NonGameUploadOutput(string message)
        {
            string command = message.ToLower();
            if (command == "новая игра")
            {
                gameModel = new InitializeGame().CreateExpeditionGame("Дэй");
                OutputTextBox.AppendText($"Добро пожаловать в игру: \"Экспедиция\"!\n\n");
                var response = gameModel.CurrentQuest.StartQuest() + "\n\n";
                textHistory = "";
                textHistory += response;
                OutputTextBox.AppendText(response);
                IsGameUpload = true;

                txtPlayesStats.Text = gameModel.Player.CurrentStats;
                btnSave.Visibility = Visibility.Visible;
            }
            else if (command == "загрузить игру")
            {
                gameModel = new GameModel();
                textHistory = gameModel.LoadGame();
                OutputTextBox.AppendText(textHistory + "\n\n");
                OutputTextBox.AppendText(gameModel.CurrentQuest.GetCurrentStepDescription() + "\n\n");
                IsGameUpload = true;

                txtPlayesStats.Text = gameModel.Player.CurrentStats;
                btnSave.Visibility = Visibility.Visible;
            } 
            else if (command == "выйти")
            {
                Application.Current.Shutdown();
            } 
            else
            {
                OutputTextBox.AppendText("Неизвестная команда.\n");
            }
        }


        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string input = InputTextBox.Text.Trim();
                InputTextBox.Clear();
                UpdateOutput(input);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void appExit(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void appMinimize(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            gameModel.SaveGame(textHistory);
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text.Trim();
            InputTextBox.Clear();
            UpdateOutput(input);
        }
    }
}