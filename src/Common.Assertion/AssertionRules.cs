namespace Goodtocode.Assertion;

public static class AssertionRules
{
    public static void Should<T>(this T? obj, string message = "Expected value to be not null")
    {
        if (obj is null) throw new AssertionFailedException(message);
    }

    public static void ShouldNot<T>(this T? obj, string message = "Expected value to be null")
    {
        if (obj is not null) throw new AssertionFailedException(message);
    }

    public static void ShouldBe<T>(this T actual, T expected, string? message = null)
    {
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
        {
            throw new AssertionFailedException(message ?? $"Expected value to be '{expected}', but was '{actual}'.");
        }
    }

    public static void ShouldBeTrue(this bool condition, string message = "Expected condition to be true")
    {
        if (!condition) throw new AssertionFailedException(message);
    }

    public static void ShouldBeFalse(this bool condition, string message = "Expected condition to be false")
    {
        if (condition) throw new AssertionFailedException(message);
    }

    public static void ShouldBeNull<T>(this T? obj, string message = "Expected value to be null")
    {
        if (obj is not null) throw new AssertionFailedException(message);
    }

    public static void ShouldNotBeNull<T>(this T? obj, string message = "Expected value to be not null")
    {
        if (obj is null) throw new AssertionFailedException(message);
    }

    public static void ShouldBeEmpty<T>(this T value, string message = "Expected value to be empty") where T : struct
    {
        if (!EqualityComparer<T>.Default.Equals(value, default))
            throw new AssertionFailedException(message);
    }

    public static void ShouldNotBeEmpty<T>(this T value, string message = "Expected value to not be empty") where T : struct
    {
        if (EqualityComparer<T>.Default.Equals(value, default))
            throw new AssertionFailedException(message);
    }
}

