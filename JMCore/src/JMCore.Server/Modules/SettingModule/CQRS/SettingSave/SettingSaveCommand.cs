﻿using JMCore.Server.Storages.Models;
using MediatR;

namespace JMCore.Server.Modules.SettingModule.CQRS.SettingSave;

public class SettingSaveCommand(StorageTypeEnum storageType, string key, string value, bool isSystem = false) : IRequest
{
  public StorageTypeEnum StorageType { get; } = storageType;
  public string Key { get; } = key;
  public string Value { get; } = value;
  public bool IsSystem { get; } = isSystem;

  public SettingSaveCommand(string key, string value, bool isSystem = false) : this(StorageTypeEnum.AllRegistered, key, value, isSystem)
  {
  }
}