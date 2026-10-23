using System;

namespace TicTacToeGame
{
    /// <summary>
    /// Компьютер-соперник: выбирает ход перебором вариантов (минимакс) на заданную глубину.
    /// </summary>
    public class ComputerPlayer
    {
        private readonly Mark mark;
        private readonly Mark opponentMark;
        private readonly int searchDepth;

        public ComputerPlayer(Mark mark, int searchDepth)
        {
            this.mark = mark;
            this.searchDepth = searchDepth;
            opponentMark = mark == Mark.Cross ? Mark.Zero : Mark.Cross;
        }

        /// <summary>
        /// Выбирает индекс клетки для хода. Поле не должно быть заполнено полностью.
        /// </summary>
        public int ChooseMove(TicTacToeBoard board)
        {
            int bestMove = -1;
            int bestScore = int.MinValue;

            foreach (int move in board.GetEmptyCells())
            {
                TicTacToeBoard next = board.Clone();
                next.TryPlace(move, mark);
                int score = Minimax(next, opponentMark, searchDepth - 1);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            return bestMove;
        }

        private int Minimax(TicTacToeBoard board, Mark turn, int depthLeft)
        {
            if (board.IsFinished())
            {
                return ScoreFinished(board);
            }

            if (depthLeft == 0)
            {
                return 0;
            }

            bool isOurTurn = turn == mark;
            int best = isOurTurn ? int.MinValue : int.MaxValue;

            foreach (int move in board.GetEmptyCells())
            {
                TicTacToeBoard next = board.Clone();
                next.TryPlace(move, turn);
                int score = Minimax(next, isOurTurn ? opponentMark : mark, depthLeft - 1);

                if (isOurTurn)
                {
                    best = Math.Max(best, score);
                }
                else
                {
                    best = Math.Min(best, score);
                }
            }

            return best;
        }

        private int ScoreFinished(TicTacToeBoard board)
        {
            Mark winner = board.GetWinner();
            if (winner == mark)
            {
                return 1;
            }

            if (winner == opponentMark)
            {
                return -1;
            }

            return 0;
        }
    }
}
