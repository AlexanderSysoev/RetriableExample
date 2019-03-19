using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Polly;

namespace RetriableExample
{
    public class RetriableReadAsyncInterceptor : IAsyncInterceptor
    {
        public void InterceptSynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptSync(invocation);
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            throw new NotImplementedException();
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsync<TResult>(invocation);
        }

        private IEnumerable<TimeSpan> RetryIntervals =>
            new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(15)
            };

        private object InternalInterceptSync(IInvocation invocation)
        {
            return Policy
                .Handle<DatabaseException>()
                .WaitAndRetry(RetryIntervals, (exception, timeSpan) =>
                {
                    Console.WriteLine($"Exception {timeSpan}");
                })
                .Execute(() =>
                {
                    invocation.Proceed();
                    return invocation.ReturnValue;
                });
        }

        private async Task<TResult> InternalInterceptAsync<TResult>(IInvocation invocation)
        {
            return await Policy
                .Handle<DatabaseException>()
                .WaitAndRetryAsync(RetryIntervals, (exception, timeSpan) =>
                {
                    Console.WriteLine($"Exception {timeSpan}");
                })
                .ExecuteAsync(async () =>
                {
                    invocation.Proceed();
                    var task = (Task<TResult>)invocation.ReturnValue;
                    return await task;
                });
        }
    }
}
