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
        throw new NotImplementedException();
    }

    /// <summary>
    /// Validates that a Sudoku board has the correct dimensions and value ranges.
    /// </summary>
    /// <param name="board">A 9x9 Sudoku board represented as a span of nullable bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the board length is not 81.</exception>
    /// <exception cref="ArgumentException">Thrown when the board contains values outside the range [0-9].</exception>
    private void ValidateBoard(ReadOnlySpan<byte?> board)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}
