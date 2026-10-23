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
            [ 0, 1, 2 ], [ 3, 4, 5 ], [ 6, 7, 8 ],
            [0, 3, 6], [1, 4, 7], [2, 5, 8],
            [0, 4, 8], [2, 4, 6]
        };

        private readonly Mark[] cells = new Mark[Size * Size];

        /// <summary>
        /// Возвращает метку в клетке с указанным индексом (0..8).
        /// </summary>
        public Mark GetMark(int index)
        {
            return cells[index];
        }

        /// <summary>
        /// Возвращает индексы свободных клеток.
        /// </summary>
        public List<int> GetEmptyCells()
        {
            var empty = new List<int>();
            for (int index = 0; index < Size * Size; index++)
            {
                if (IsEmpty(index))
                {
                    empty.Add(index);
                }
            }

            return empty;
        }

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
        public bool IsDraw()
        {
            return GetWinner() == Mark.None && GetEmptyCells().Count == 0;
        }

        /// <summary>
        /// Игра окончена, если есть победитель или ничья.
        /// </summary>
        public bool IsFinished()
        {
            return GetWinner() != Mark.None || IsDraw();
        }

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

        private bool IsEmpty(int index)
        {
            return cells[index] == Mark.None;
        }

        private static bool IsValidIndex(int index)
        {
            return index >= 0 && index < Size * Size;
        }
    }

    /// <summary>
    /// Метка клетки: пусто, крестик или нолик.
    /// </summary>
    public enum Mark
    {
        None,
        Cross,
        Zero
    }
}
