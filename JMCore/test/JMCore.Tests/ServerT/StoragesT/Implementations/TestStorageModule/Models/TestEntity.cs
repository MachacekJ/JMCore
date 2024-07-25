﻿using System.ComponentModel.DataAnnotations;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace JMCore.Tests.ServerT.StoragesT.Implementations.TestStorageModule.Models;

public class TestEntity
{
  public string UId { get; set; } = Guid.NewGuid().ToString();
  public int Id { get; set; }

  public string Name { get; set; } = null!;

  public DateTime Created { get; set; }
}