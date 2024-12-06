namespace Sudoku;

public enum SudokuStatus
{
    ValidUnsolved,
    ValidSolved,
    Invalid
}

public enum SudokuDifficulty
{
    Easy, // 40% filled
    Medium, // 30% filled
    Hard // 20% filled
}

public class SudokuBoard
{
    /// <summary>
    /// Validates the current state of a Sudoku board.
    /// </summary>
    /// <param name="board">A 9x9 Sudoku board represented as a span of nullable bytes.</param>
    /// <returns>
    /// - ValidSolved: If the board is complete and valid
    /// - ValidUnsolved: If the board is valid but incomplete
    /// - Invalid: If the board violates Sudoku rules
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the board length is not 81.</exception>
    /// <exception cref="ArgumentException">Thrown when the board contains values outside the range [0-9].</exception>
    public SudokuStatus Validate(ReadOnlySpan<byte?> board)
    {
        ValidateBoard(board);

        bool isComplete = true;

        // Combined row and column check
        for (int i = 0; i < 9; i++)
        {
            var row = new HashSet<byte>();
            var column = new HashSet<byte>();
            for (int j = 0; j < 9; j++)
            {
                // Check row
                var rowCell = board[i * 9 + j];
                if (rowCell is not null)
                {
                    if (rowCell == 0 || !row.Add(rowCell.Value))
                    {
                        return SudokuStatus.Invalid;
                    }
                }
                else
                {
                    isComplete = false;
                }

                // Check column
                var columnCell = board[j * 9 + i];
                if (columnCell is not null)
                {
                    if (columnCell == 0 || !column.Add(columnCell.Value))
                    {
                        return SudokuStatus.Invalid;
                    }
                }
                else
                {
                    isComplete = false;
                }
            }
        }

        // Check 3x3 subgrids
        for (int block = 0; block < 9; block++)
        {
            var subgrid = new HashSet<byte>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var cell = board[block / 3 * 27 + block % 3 * 3 + i * 9 + j];
                    if (cell is not null)
                    {
                        if (cell == 0 || !subgrid.Add(cell.Value))
                        {
                            return SudokuStatus.Invalid;
                        }
                    }
                    else
                    {
                        isComplete = false;
                    }
                }
            }
        }

        return isComplete ? SudokuStatus.ValidSolved : SudokuStatus.ValidUnsolved;
    }

    /// <summary>
    /// Validates that a Sudoku board has the correct dimensions and value ranges.
    /// </summary>
    /// <param name="board">A 9x9 Sudoku board represented as a span of nullable bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the board length is not 81.</exception>
    /// <exception cref="ArgumentException">Thrown when the board contains values outside the range [0-9].</exception>
    private void ValidateBoard(ReadOnlySpan<byte?> board)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(board.Length, 81);

        foreach (var cell in board)
        {
            if (cell is not null && (cell < 0 || cell > 9))
            {
                throw new ArgumentException($"Board contains at least one invalid value");
            }
        }
    }

    /// <summary>
    /// Attempts to set a value at the specified position on the Sudoku board.
    /// </summary>
    /// <param name="board">A 9x9 Sudoku board represented as a span of nullable bytes.</param>
    /// <param name="row">The row index (0-8).</param>
    /// <param name="column">The column index (0-8).</param>
    /// <param name="value">The value to set (1-9).</param>
    /// <returns>
    /// true if the value was successfully set;
    /// false if the position is already filled or if setting the value would make the board invalid
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when:
    /// - The board length is not 81
    /// - Row index is greater than 8
    /// - Column index is greater than 8
    /// - Value is less than 1 or greater than 9
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when the board contains values outside the range [0-9].</exception>
    public bool TrySet(Span<byte?> board, byte row, byte column, byte value)
    {
        ValidateBoard(board);
        
        ArgumentOutOfRangeException.ThrowIfGreaterThan(row, 8);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(column, 8);
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 9);

        if (board[row * 9 + column] is not null)
        {
            return false;
        }

        Span<byte?> tempBoard = stackalloc byte?[81];
        board.CopyTo(tempBoard);
        
        int index = row * 9 + column;
        tempBoard[index] = value;

        if (Validate(tempBoard) == SudokuStatus.Invalid) 
        {
            return false;
        }

        board[index] = value;
        return true;
    }
}
