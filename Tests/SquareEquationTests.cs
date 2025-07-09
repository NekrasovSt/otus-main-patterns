using Lesson4;
using Xunit;

namespace Tests;

public class SquareEquationTests
{
    private const int precision = 3;
    [Fact]
    public void NegativeDiscriminant()
    {
        // Arrange
        double a = 1, b = 0, c = 1;
 
        // Act
        var result = SquareEquation.Solve(a, b, c);
 
        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public void PositiveDiscriminant()
    {
        // Arrange
        double a = 1, b = 0, c = -1;
 
        // Act
        var result = SquareEquation.Solve(a, b, c);
 
        // Assert
        Array.Sort(result);
        Assert.Equal(-1, result[0], precision);
        Assert.Equal(1, result[1], precision);
    }

    [Fact]
    public void ZeroDiscriminant()
    {
        // Arrange
        double a = 1, b = 2, c = 1;

        // Act
        var result = SquareEquation.Solve(a, b, c);

        var t = Math.Pow(b, 2) / 4 * a;
        // Assert
        Array.Sort(result);
        Assert.Equal(-1, result[0], precision);
        Assert.Equal(1, result[1], precision);
    }
    
    [Fact]
    public void ZeroDiscriminantInEpsilon()
    {
        // Arrange
        double a = 1, b = 1.98, c = 0.98009999999999997;
 
        // Act
        var result = SquareEquation.Solve(a, b, c);

        var t = Math.Pow(b, 2) / 4 * a;
        // Assert
        Array.Sort(result);
        Assert.Equal(-0.98999, result[0], precision);
        Assert.Equal(0.98999, result[1], precision);
    }

    [Fact]
    public void AZero()
    {
        Assert.Throws<ArgumentException>(()=>SquareEquation.Solve(0.000001, 2, 3));
    }
    
    [Theory]
    [InlineData(double.NaN, 3, 5)]
    [InlineData(-2, double.NaN, -5)]
    [InlineData(-2, 8, double.NaN)]
    [InlineData(double.PositiveInfinity, 3, 5)]
    [InlineData(-2, double.NegativeInfinity, -5)]
    [InlineData(-2, 8, double.NegativeInfinity)]
    public void WrongInput(double a, double b, double c)
    {
        Assert.Throws<ArgumentException>(()=>SquareEquation.Solve(a, b, c));
    }
}