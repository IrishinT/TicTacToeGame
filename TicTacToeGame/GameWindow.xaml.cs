using System.Windows;
using System.Windows.Controls;

namespace TicTacToeGame
{
    /// <summary>
    /// Главное окно игры в крестики-нолики.
    /// </summary>
    public partial class GameWindow : Window
    {
        private const string NewGameButtonText = "Новая игра";

        private readonly TicTacToeBoard board = new TicTacToeBoard();
        private readonly ComputerPlayer computer = new ComputerPlayer(Mark.Nought);
        private Button[] cells;

        public GameWindow()
        {
            InitializeComponent();
            cells = [Cell0, Cell1, Cell2, Cell3, Cell4, Cell5, Cell6, Cell7, Cell8];
            SubscribeCells();
        }

        private void SubscribeCells()
        {
            for (int index = 0; index < cells.Length; index++)
            {
                cells[index].Tag = index;
                cells[index].Click += CellButton_Click;
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            board.Reset();
            NewGameButton.Content = NewGameButtonText;
            SetBoardEnabled(true);
            RefreshBoard();
            StatusText.Text = "Ваш ход";
        }

        private void CellButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not int index)
            {
                return;
            }

            if (!board.TryPlace(index, Mark.Cross))
            {
                return;
            }

            if (!board.IsFinished())
            {
                board.TryPlace(computer.ChooseMove(board), Mark.Nought);
            }

            RefreshBoard();
            ShowTurnResult();
        }

        private void ShowTurnResult()
        {
            if (!board.IsFinished())
            {
                StatusText.Text = "Ваш ход";
                return;
            }

            StatusText.Text = GetResultText();
            SetBoardEnabled(false);
        }

        private string GetResultText()
        {
            Mark winner = board.GetWinner();
            if (winner == Mark.Cross)
            {
                return "Вы победили";
            }

            if (winner == Mark.Nought)
            {
                return "Победил компьютер";
            }

            return "Ничья";
        }

        private void RefreshBoard()
        {
            for (int index = 0; index < cells.Length; index++)
            {
                cells[index].Content = MarkToText(board[index]);
            }
        }

        private static string MarkToText(Mark mark)
        {
            if (mark == Mark.Cross)
            {
                return "X";
            }

            if (mark == Mark.Nought)
            {
                return "O";
            }

            return string.Empty;
        }

        private void SetBoardEnabled(bool isEnabled)
        {
            foreach (Button cell in cells)
            {
                cell.IsEnabled = isEnabled;
            }
        }
    }
}
