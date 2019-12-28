using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Pagination
    {
        public string sql { get; set; }
        public object where { get; set; }

        /// <summary>
        /// 每页行数
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int pageIndex { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        public string sidx { get; set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public string sord { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int records { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int total
        {
            get
            {
                if (records > 0)
                {
                    return records % this.pageSize == 0 ? records / this.pageSize : records / this.pageSize + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
