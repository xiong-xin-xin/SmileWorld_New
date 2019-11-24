using AspectCore.DynamicProxy;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Util
{
    public class ToolRedisCacheAOP : AbstractInterceptorAttribute
    {
        private IRedisCacheClient _redisCacheClient;
        public ToolRedisCacheAOP(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            IList<Tuple<string, string>> values = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("key1", "value1"),
                new Tuple<string, string>("key2", "value2"),
                new Tuple<string, string>("key3", "value3")
            };

            //bool added = await _redisCacheClient.Db15.AddAllAsync(values);
            await context.Invoke(next);
        }
    }
}
