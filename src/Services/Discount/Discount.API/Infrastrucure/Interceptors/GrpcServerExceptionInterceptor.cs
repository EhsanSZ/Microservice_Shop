﻿
namespace eShop.Services.Discount.DiscountAPI.Infrastrucure.Interceptors;


public class GrpcServerExceptionInterceptor : Interceptor
{
    private readonly ILogger<GrpcServerExceptionInterceptor> _logger;
    private readonly IErrorHandler _errorHandler;
    public GrpcServerExceptionInterceptor(ILogger<GrpcServerExceptionInterceptor> logger,
        IErrorHandler errorHandler
        )
    {
        _logger = logger;
        _errorHandler = errorHandler;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }

        catch (Exception exception)
        {
            var httpContext = context.GetHttpContext();
            var res = _errorHandler.GetError(exception);

        
            httpContext.Response.StatusCode = res.StatusCode;
            throw new RpcException(new Status(StatusCode.Internal, JsonSerializer.Serialize(res)));



           
        }
    }
}