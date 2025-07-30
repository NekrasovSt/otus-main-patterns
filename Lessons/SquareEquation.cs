using ArgumentException = System.ArgumentException;

namespace Lessons;

public class SquareEquation
{
    public const double Epsilon = 0.00001;

    private static void CheckInput(double input, string name)
    {
        if (double.IsNaN(input) || double.IsInfinity(input) || double.IsNegativeInfinity(input))
        {
            throw new ArgumentException(name);
        }
    }
    public static double[] Solve(double a, double b, double c)
    {
        if (Math.Abs(a) < Epsilon)
        {
            throw new ArgumentException(nameof(a));
        }
        CheckInput(a, nameof(a));
        CheckInput(b, nameof(b));
        CheckInput(c, nameof(c));
        var d = Math.Pow(b, 2) - 4 * a * c;
        // D = 0
        if (Math.Abs(d) < Epsilon)
        {
            var x = -b / (2 * a);
            return [-x, x];
        }

        // D < 0
        if (d < 0)
        {
            return [];
        }

        // D > 0
        var x1 = (-b + Math.Sqrt(d)) / (2 * a);
        var x2 = (-b - Math.Sqrt(d)) / (2 * a);
        return [x1, x2];
    }
}