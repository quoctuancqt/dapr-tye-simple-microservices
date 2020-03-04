﻿using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SimpleStore.ProductCatalog.Infrastructure.EfCore.ValidationModel
{
    public class RequestValidationBehavior<TRequest,TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest, IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _validator;
        private readonly ILogger<RequestValidationBehavior<TRequest, TResponse>> _logger;

        public RequestValidationBehavior(IValidator<TRequest> validator, ILogger<RequestValidationBehavior<TRequest, TResponse>> logger)
        {
            this._validator = validator;
            this._logger = logger;
        }

        #region Implementation of IPipelineBehavior<in TRequest,TResponse>

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation($"Doing validation for {nameof(request)}");
            await _validator.HandleValidation(request);
            return await next();
        }

        #endregion
    }
}