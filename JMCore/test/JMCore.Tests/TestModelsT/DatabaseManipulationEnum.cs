﻿namespace JMCore.Tests.TestModelsT;

[Flags]
public enum DatabaseManipulationEnum
{
  Create = 1 << 0,
  Drop = 1 << 1,
  Default = Create | Drop  
}