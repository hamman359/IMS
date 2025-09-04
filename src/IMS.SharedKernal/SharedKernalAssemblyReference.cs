using System.Reflection;

namespace IMS.SharedKernal;

/// <summary>
/// Provides strongly typed reference to this assembly
/// </summary>
public static class SharedKernalAssemblyReference
{
    public static readonly Assembly Assembly = typeof(SharedKernalAssemblyReference).Assembly;
}
