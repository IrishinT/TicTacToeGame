using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeGame
{
    /// <summary>
    /// Игровое поле 3x3 и правила игры в крестики-нолики.
    /// </summary>
    public class TicTacToeBoard
    {
        private const int Size = 3;

        private static readonly int[][] WinningLines =
        {
            new[] { 0, 1, 2 }, new[] { 3, 4, 5 }, new[] { 6, 7, 8 },
            new[] { 0, 3, 6 }, new[] { 1, 4, 7 }, new[] { 2, 5, 8 },
            new[] { 0, 4, 8 }, new[] { 2, 4, 6 }
        };

        private readonly Mark[] cells = new Mark[Size * Size];

        /// <summary>
        /// Возвращает метку в клетке с указанным индексом (0..8).
        /// </summary>
        public Mark this[int index] => cells[index];

        /// <summary>
        /// Индексы свободных клеток.
        /// </summary>
        public IEnumerable<int> EmptyCells => Enumerable.Range(0, cells.Length).Where(IsEmpty);

        /// <summary>
        /// Ставит метку в клетку. Возвращает false, если ход недопустим.
        /// </summary>
        public bool TryPlace(int index, Mark mark)
        {
            if (!IsValidIndex(index) || !IsEmpty(index))
            {
                return false;
            }

            cells[index] = mark;
            return true;
        }

        /// <summary>
        /// Возвращает победителя или <see cref="Mark.None"/>, если победителя нет.
        /// </summary>
        public Mark GetWinner()
        {
            foreach (int[] line in WinningLines)
            {
                Mark first = cells[line[0]];
                if (first != Mark.None && line.All(i => cells[i] == first))
                {
                    return first;
                }
            }

            return Mark.None;
        }

        /// <summary>
        /// Ничья: все клетки заняты, а победителя нет.
        /// </summary>
        public bool IsDraw() => GetWinner() == Mark.None && !EmptyCells.Any();

        /// <summary>
        /// Игра окончена, если есть победитель или ничья.
        /// </summary>
        public bool IsFinished() => GetWinner() != Mark.None || IsDraw();

        /// <summary>
        /// Создаёт копию поля, чтобы проверять ходы без изменения настоящего поля.
        /// </summary>
        public TicTacToeBoard Clone()
        {
            var copy = new TicTacToeBoard();
            Array.Copy(cells, copy.cells, cells.Length);
            return copy;
        }

        /// <summary>
        /// Очищает поле для новой партии.
        /// </summary>
        public void Reset()
        {
            Array.Fill(cells, Mark.None);
        }

        private bool IsEmpty(int index) => cells[index] == Mark.None;

        private static bool IsValidIndex(int index) => index >= 0 && index < Size * Size;
    }

    /// <summary>
    /// Метка клетки: пусто, крестик или нолик.
    /// </summary>
    public enum Mark
    {
        None,
        Cross,
        Nought
    }
}
