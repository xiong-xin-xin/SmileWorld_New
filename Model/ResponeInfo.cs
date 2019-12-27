using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    /// <summary>
    /// 返回内容模型
    /// </summary>
    public class ResponeInfo
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public object data { get; set; } = "";
        /// <summary>
        /// 返回消息
        /// </summary>
        public string msg { get; set; } = "响应成功";
        /// <summary>
        /// 0正确 -1错误
        /// </summary>
        public int code { get; set; } = 0;

    }
}
