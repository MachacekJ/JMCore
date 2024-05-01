using JMCore.Server.DB.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Server.DB;



public class DBB(Type type)
{
   public string Name { get; } = type.Name;
   public Type Type { get; } = type;
}