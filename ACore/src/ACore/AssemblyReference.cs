using System.Reflection;

namespace ACore;

public static class AssemblyReference
{
  public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}