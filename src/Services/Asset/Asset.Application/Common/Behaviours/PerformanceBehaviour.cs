﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Asset.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehaviour(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds <= 5000) return response;

            var requestName = typeof(TRequest).Name;
            _logger.LogWarning("Application Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                requestName, elapsedMilliseconds, request);

            return response;
        }
    }
}
