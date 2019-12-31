using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class BaseBLL
    {

        public class AjaxResult
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


        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public AjaxResult Success(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                code = 0,
                msg = msg,
                data = null
            };
            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public AjaxResult Success(object data)
        {
            AjaxResult res = new AjaxResult
            {
                code = 0,
                msg = "请求成功！",
                data = data
            };
            return res;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">返回的消息</param>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public AjaxResult Success(string msg, object data)
        {
            AjaxResult res = new AjaxResult
            {
                code = 0,
                msg = msg,
                data = data
            };
            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        public AjaxResult Error()
        {
            AjaxResult res = new AjaxResult
            {
                code = 1,
                msg = "请求失败！",
                data = null
            };
            return res;
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        public AjaxResult Error(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                code = 1,
                msg = msg,
                data = null
            };
            return res;
        }
    }
}
