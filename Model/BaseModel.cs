using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    /// <summary>
    /// 数据库表基类
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreatedId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>
        public string UpdatedId { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdatedBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdatedDate { get; set; }


        public virtual void Create(string userId, string userName)
        {
            CreatedBy = userName;
            CreatedId = userId;
            CreatedDate = DateTime.Now;

            UpdatedBy = userName;
            UpdatedId = userId;
            UpdatedDate = DateTime.Now;
        }
        public virtual void Update(string userId, string userName)
        {
            this.UpdatedId = userId;
            this.UpdatedBy = userName;
            this.UpdatedDate = DateTime.Now;
        }

    }
}
