using Sudoku;

namespace Sudoku.Tests;

public class SudokuBoardTests
{
    private readonly SudokuBoard _board = new();

    [Fact]
    public void Validate_ValidCompletedBoard_ReturnsValidSolved()
    {
        ReadOnlySpan<byte?> board = stackalloc byte?[] {
            1,2,3, 4,5,6, 7,8,9,
            4,5,6, 7,8,9, 1,2,3,
            7,8,9, 1,2,3, 4,5,6,

            2,3,1, 5,6,4, 8,9,7,
            5,6,4, 8,9,7, 2,3,1,
            8,9,7, 2,3,1, 5,6,4,

            3,1,2, 6,4,5, 9,7,8,
            6,4,5, 9,7,8, 3,1,2,
            9,7,8, 3,1,2, 6,4,5
        };

        var result = _board.Validate(board);

        Assert.Equal(SudokuStatus.ValidSolved, result);
    }

    [Fact]
    public void Validate_ValidIncompleteBoard_ReturnsValidUnsolved()
    {
        ReadOnlySpan<byte?> board = stackalloc byte?[] {
            1,2,3, 4,5,6, 7,8,9,
            4,5,6, 7,8,9, 1,2,3,
            7,8,9, 1,2,3, 4,5,6,

            2,3,1, 5,6,4, 8,9,7,
            5,6,4, 8,9,7, 2,3,1,
            8,9,7, 2,3,1, 5,6,4,

            3,1,2, 6,4,5, 9,7,8,
            6,4,5, 9,7,8, 3,1,2,
            9,7,8, 3,1,null, 6,4,5
        };

        var result = _board.Validate(board);

        Assert.Equal(SudokuStatus.ValidUnsolved, result);
    }

    [Fact]
    public void Validate_InvalidRowDuplicate_ReturnsInvalid()
    {
        ReadOnlySpan<byte?> board = stackalloc byte?[] {
            1,2,3, 4,5,6, 7,8,1, // Duplicate 1 in row
            4,5,6, 7,8,9, 1,2,3,
            7,8,9, 1,2,3, 4,5,6,

            2,3,1, 5,6,4, 8,9,7,
            5,6,4, 8,9,7, 2,3,1,
            8,9,7, 2,3,1, 5,6,4,

            3,1,2, 6,4,5, 9,7,8,
            6,4,5, 9,7,8, 3,1,2,
            9,7,8, 3,1,2, 6,4,5
        };

        var result = _board.Validate(board);

        Assert.Equal(SudokuStatus.Invalid, result);
    }

    [Fact]
    public void Validate_InvalidColumnDuplicate_ReturnsInvalid()
    {
        ReadOnlySpan<byte?> board = stackalloc byte?[] {
            1,2,3, 4,5,6, 7,8,9,
            1,5,6, 7,8,9, 1,2,3, // Duplicate 1 in column
            7,8,9, 1,2,3, 4,5,6,

            2,3,1, 5,6,4, 8,9,7,
            5,6,4, 8,9,7, 2,3,1,
            8,9,7, 2,3,1, 5,6,4,

            3,1,2, 6,4,5, 9,7,8,
            6,4,5, 9,7,8, 3,1,2,
            9,7,8, 3,1,2, 6,4,5
        };

        var result = _board.Validate(board);

        Assert.Equal(SudokuStatus.Invalid, result);
    }

    [Fact]
    public void Validate_InvalidSubgridDuplicate_ReturnsInvalid()
    {
        ReadOnlySpan<byte?> board = stackalloc byte?[] {
            1,2,3, 4,5,6, 7,8,9,
            4,1,6, 7,8,9, 1,2,3, // Duplicate 1 in 3x3 grid
            7,8,9, 1,2,3, 4,5,6,

            2,3,1, 5,6,4, 8,9,7,
            5,6,4, 8,9,7, 2,3,1,
            8,9,7, 2,3,1, 5,6,4,

            3,1,2, 6,4,5, 9,7,8,
            6,4,5, 9,7,8, 3,1,2,
            9,7,8, 3,1,2, 6,4,5
        };

        var result = _board.Validate(board);

        Assert.Equal(SudokuStatus.Invalid, result);
    }

    [Fact]
    public void Validate_InvalidBoardSize_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            ReadOnlySpan<byte?> board = stackalloc byte?[80]; // Wrong size
            _board.Validate(board);
        });
    }

    [Fact]
    public void Validate_InvalidValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            Span<byte?> board = stackalloc byte?[81];
            board[0] = 10; // Invalid value > 9
            _board.Validate(board);
        });
    }

    [Fact]
    public void TrySet_ValidMove_ReturnsTrue()
    {
        Span<byte?> board = stackalloc byte?[] {
            1,2,3, 4,5,6, 7,8,9,
            4,5,6, 7,8,9, 1,2,3,
            7,8,9, 1,2,3, 4,5,6,

            2,3,1, 5,6,4, 8,9,7,
            5,6,4, 8,9,7, 2,3,1,
            8,9,7, 2,3,1, 5,6,4,

            3,1,2, 6,4,5, 9,7,8,
            6,4,5, 9,7,8, 3,1,2,
            9,7,8, 3,1,null, 6,4,5  // Empty cell at (8,5)
        };

        var result = _board.TrySet(board, 8, 5, 2);

        Assert.True(result);
        Assert.Equal((byte)2, board[8 * 9 + 5]);
    }

    [Fact]
    public void TrySet_InvalidMove_ReturnsFalse()
    {
        Span<byte?> board = stackalloc byte?[] {
            1,2,3, 4,5,6, 7,8,9,
            4,5,6, 7,8,9, 1,2,3,
            7,8,9, 1,2,3, 4,5,6,

            2,3,1, 5,6,4, 8,9,7,
            5,6,4, 8,9,7, 2,3,1,
            8,9,7, 2,3,1, 5,6,4,

            3,1,2, 6,4,5, 9,7,8,
            6,4,5, 9,7,8, 3,1,2,
            9,7,8, 3,1,null, 6,4,5  // Empty cell at (8,5)
        };

        var result = _board.TrySet(board, 8, 5, 1);  // 1 already exists in column

        Assert.False(result);
        Assert.Null(board[8 * 9 + 5]);
    }

    [Fact]
    public void TrySet_CellAlreadyFilled_ReturnsFalse()
    {
        Span<byte?> board = stackalloc byte?[] {
            1,2,3, 4,5,6, 7,8,9,
            4,5,6, 7,8,9, 1,2,3,
            7,8,9, 1,2,3, 4,5,6,

            2,3,1, 5,6,4, 8,9,7,
            5,6,4, 8,9,7, 2,3,1,
            8,9,7, 2,3,1, 5,6,4,

            3,1,2, 6,4,5, 9,7,8,
            6,4,5, 9,7,8, 3,1,2,
            9,7,8, 3,1,2, 6,4,5
        };

        var result = _board.TrySet(board, 0, 0, 5);  // Cell (0,0) is already filled

        Assert.False(result);
        Assert.Equal((byte)1, board[0 * 9 + 0]);
    }

    [Theory]
    [InlineData(9, 0, 1)]  // Invalid row
    [InlineData(0, 9, 1)]  // Invalid column
    [InlineData(0, 0, 0)]  // Invalid value (too low)
    [InlineData(0, 0, 10)] // Invalid value (too high)
    public void TrySet_InvalidParameters_ThrowsArgumentOutOfRangeException(byte row, byte column, byte value)
    {

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Span<byte?> board = stackalloc byte?[81];
            _board.TrySet(board, row, column, value);
        });
    }

    [Fact]
    public void TrySet_InvalidBoardSize_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            Span<byte?> board = stackalloc byte?[80]; // Wrong size
            _board.TrySet(board, 0, 0, 1);
        });
    }
}
