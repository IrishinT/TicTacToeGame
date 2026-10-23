using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToeGame
{
    /// <summary>
    /// Главное окно игры в крестики-нолики.
    /// </summary>
    public partial class GameWindow : Window
    {
        private const string NewGameButtonText = "Новая игра";
        private const int FullSearchDepth = 9;

        private readonly TicTacToeBoard board = new TicTacToeBoard();
        private readonly Random random = new Random();
        private readonly Button[] cells;

        private ComputerPlayer computer = new ComputerPlayer(Mark.Zero, FullSearchDepth);
        private Mark playerMark;
        private Mark computerMark;

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
            ChooseMarks();
            NewGameButton.Content = NewGameButtonText;
            DifficultyBox.IsEnabled = false;
            NewGameButton.IsEnabled = false;
            SetBoardEnabled(true);

            if (computerMark == Mark.Cross)
            {
                MakeComputerMove();
            }

            RefreshBoard();
            ShowTurnResult();
        }

        private void ChooseMarks()
        {
            playerMark = random.Next(2) == 0 ? Mark.Cross : Mark.Zero;
            computerMark = playerMark == Mark.Cross ? Mark.Zero : Mark.Cross;
            computer = new ComputerPlayer(computerMark, GetSearchDepth());
        }

        private int GetSearchDepth()
        {
            if (DifficultyBox.SelectedItem is ComboBoxItem item && item.Tag is string tag && int.TryParse(tag, out int depth))
            {
                return depth;
            }

            return FullSearchDepth;
        }

        private void CellButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not int index)
            {
                return;
            }

            if (board.IsFinished())
            {
                return;
            }

            if (!board.TryPlace(index, playerMark))
            {
                return;
            }

            if (!board.IsFinished())
            {
                MakeComputerMove();
            }

            RefreshBoard();
            ShowTurnResult();
        }

        private void MakeComputerMove()
        {
            board.TryPlace(computer.ChooseMove(board), computerMark);
        }

        private void ShowTurnResult()
        {
            if (!board.IsFinished())
            {
                StatusText.Text = $"Вы - \"{MarkToText(playerMark)}\". Ваш ход";
                return;
            }

            StatusText.Text = GetResultText();
            HighlightWinningLine();
            DifficultyBox.IsEnabled = true;
            NewGameButton.IsEnabled = true;
        }

        private string GetResultText()
        {
            Mark winner = board.GetWinner();
            if (winner == playerMark)
            {
                return "Вы победили";
            }

            if (winner == computerMark)
            {
                return "Победил робот";
            }

            return "Ничья";
        }

        private void RefreshBoard()
        {
            for (int index = 0; index < cells.Length; index++)
            {
                Mark mark = board.GetMark(index);
                cells[index].Content = MarkToText(mark);
                cells[index].Foreground = MarkToBrush(mark);
                cells[index].ClearValue(Button.BackgroundProperty);
            }
        }

        private void HighlightWinningLine()
        {
            int[]? line = board.GetWinningLine();
            if (line == null)
            {
                return;
            }

            Brush highlight = board.GetWinner() == playerMark ? Brushes.LightGreen : Brushes.LightCoral;
            foreach (int index in line)
            {
                cells[index].Background = highlight;
            }
        }

        private static Brush MarkToBrush(Mark mark)
        {
            if (mark == Mark.Cross)
            {
                return Brushes.SteelBlue;
            }

            if (mark == Mark.Zero)
            {
                return Brushes.Crimson;
            }

            return Brushes.Black;
        }

        private static string MarkToText(Mark mark)
        {
            if (mark == Mark.Cross)
            {
                return "Х";
            }

            if (mark == Mark.Zero)
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
