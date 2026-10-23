using System.Linq;

namespace TicTacToeGame
{
    /// <summary>
    /// Компьютер-соперник: выбирает ход по простым правилам.
    /// </summary>
    public class ComputerPlayer
    {
        private const int NoMove = -1;
        private const int CenterCell = 4;
        private static readonly int[] CornerCells = { 0, 2, 6, 8 };

        private readonly Mark mark;
        private readonly Mark opponentMark;

        public ComputerPlayer(Mark mark)
        {
            this.mark = mark;
            opponentMark = mark == Mark.Cross ? Mark.Nought : Mark.Cross;
        }

        /// <summary>
        /// Выбирает индекс клетки для хода. Поле не должно быть заполнено полностью.
        /// </summary>
        public int ChooseMove(TicTacToeBoard board)
        {
            int move = FindWinningMove(board, mark);
            if (move != NoMove)
            {
                return move;
            }

            move = FindWinningMove(board, opponentMark);
            if (move != NoMove)
            {
                return move;
            }

            return FindPreferredFreeCell(board);
        }

        private static int FindWinningMove(TicTacToeBoard board, Mark markToWin)
        {
            foreach (int index in board.EmptyCells)
            {
                TicTacToeBoard copy = board.Clone();
                copy.TryPlace(index, markToWin);
                if (copy.GetWinner() == markToWin)
                {
                    return index;
                }
            }

            return NoMove;
        }

        private int FindPreferredFreeCell(TicTacToeBoard board)
        {
            if (board.EmptyCells.Contains(CenterCell))
            {
                return CenterCell;
            }

            int corner = CornerCells.FirstOrDefault(c => board.EmptyCells.Contains(c), NoMove);
            if (corner != NoMove)
            {
                return corner;
            }

            return board.EmptyCells.First();
        }
    }
}
