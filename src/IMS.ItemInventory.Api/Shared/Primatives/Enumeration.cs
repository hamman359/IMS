using System.Reflection;

namespace IMS.ItemInventory.Api.Shared.Primatives;

public abstract class Enumeration<TEnum>
    : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumeration();

    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public int Value { get; protected init; }
    public string Name { get; protected init; } = string.Empty;

    public static TEnum? FromValue(int value)
    {
        return Enumerations.TryGetValue(
            value,
            out TEnum? enumeration)
            ? enumeration
            : default;
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations
            .Values
            .SingleOrDefault(e => e.Name == name);
    }

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType()
            && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other
            && Equals(other);
    }

    public override int GetHashCode() =>
        Value.GetHashCode();

    private static Dictionary<int, TEnum> CreateEnumeration()
    {
        var enumerationType = typeof(TEnum);

        var fieldsForType = enumerationType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(fieldInfo =>
                enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo =>
                (TEnum)fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(x => x.Value);
    }
}
