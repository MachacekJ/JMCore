﻿namespace JMCore.Server.Modules.AuditModule.UserProvider;

public interface IAuditUserProvider
{
    (string userId, string userName) GetUser();
}