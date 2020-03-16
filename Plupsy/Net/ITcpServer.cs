﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Plupsy.Net
{
    public interface ITcpServer : IDisposable, IAsyncDisposable
    {
        Task StartAsync(CancellationToken token = default);
        Task StopAsync(CancellationToken token = default);
    }
}
