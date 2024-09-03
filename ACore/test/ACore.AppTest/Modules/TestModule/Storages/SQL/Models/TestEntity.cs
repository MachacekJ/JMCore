using System.ComponentModel.DataAnnotations;
using ACore.AppTest.Modules.TestModule.CQRS.Test.Models;
using ACore.Extensions;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Models.PK;
using Microsoft.AspNetCore.Http.HttpResults;

// ReSharper disable PropertyCanBeMadeInitOnly.Global


namespace ACore.AppTest.Modules.TestModule.Storages.SQL.Models;

internal class TestEntity : PKIntEntity
{
  [MaxLength(200)]
  public string Name { get; set; } = string.Empty;

  public DateTime Created { get; set; }

  public static TestEntity Create(TestData data)
  {
    var en = new TestEntity();
    en.CopyPropertiesFrom(data);
    return en;
  }
}

internal static class TestEntityExtensions
{
  public static TestData ToData(this TestEntity entity)
  {
    var data = new TestData();
    data.CopyPropertiesFrom(entity);
    return data;
  }
}