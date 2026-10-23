using System;

namespace TicTacToeGame
{
    /// <summary>
    /// Компьютер-соперник: выбирает лучший ход перебором всех вариантов (минимакс).
    /// </summary>
    public class ComputerPlayer
    {
        private readonly Mark mark;
        private readonly Mark opponentMark;

        public ComputerPlayer(Mark mark)
        {
            this.mark = mark;
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
                int score = Minimax(next, opponentMark);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            return bestMove;
        }

        private int Minimax(TicTacToeBoard board, Mark turn)
        {
            if (board.IsFinished())
            {
                return ScoreFinished(board);
            }

            bool isOurTurn = turn == mark;
            int best = isOurTurn ? int.MinValue : int.MaxValue;

            foreach (int move in board.GetEmptyCells())
            {
                TicTacToeBoard next = board.Clone();
                next.TryPlace(move, turn);
                int score = Minimax(next, isOurTurn ? opponentMark : mark);

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
