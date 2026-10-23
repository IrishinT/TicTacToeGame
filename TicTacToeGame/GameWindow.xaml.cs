using System;
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
        private readonly Random random = new Random();
        private readonly Button[] cells;

        private ComputerPlayer computer = new ComputerPlayer(Mark.Zero);
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
            computer = new ComputerPlayer(computerMark);
        }

        private void CellButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not int index)
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
            SetBoardEnabled(false);
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
                cells[index].Content = MarkToText(board.GetMark(index));
            }
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
