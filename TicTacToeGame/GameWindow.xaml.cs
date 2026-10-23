using System.Windows;
using System.Windows.Controls;

namespace TicTacToeGame
{
    /// <summary>
    /// Главное окно игры в крестики-нолики.
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            NewGameButton.Content = "Новая игра";
            StatusText.Text = "Ваш ход";
            SetBoardEnabled(true);
        }

        private void SetBoardEnabled(bool isEnabled)
        {
            foreach (Button cell in BoardGrid.Children)
            {
                cell.IsEnabled = isEnabled;
            }
        }
    }
}
