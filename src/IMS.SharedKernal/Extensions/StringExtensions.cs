namespace IMS.SharedKernal.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string value) =>
        string.IsNullOrEmpty(value);

    public static bool IsNotNullOrEmpty(this string value) =>
        !value.IsNullOrEmpty();

    public static bool IsNullOrWhiteSpace(this string value) =>
        string.IsNullOrWhiteSpace(value);

    public static bool IsNotNullOrWhiteSpace(this string value) =>
        !value.IsNullOrWhiteSpace();

    public static bool LengthIsBetweenMinAndMax(
        this string value,
        int minLength,
        int maxLength,
        bool inclusive = true) =>
        inclusive
            ? value?.Length >= minLength && value?.Length <= maxLength
            : value?.Length > minLength && value?.Length < maxLength;
}
