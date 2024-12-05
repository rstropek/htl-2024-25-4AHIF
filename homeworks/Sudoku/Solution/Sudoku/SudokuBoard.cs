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
