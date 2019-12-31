using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{
    /// <summary>
    /// 工作单元
    /// 仅用来做特性标记 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class UnitOfWorkAttribute : Attribute
    {

    }
}
