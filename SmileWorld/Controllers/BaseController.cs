using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Util;

namespace SmileWorld.Controllers
{
    /// <summary>
    /// 基类
    /// </summary>
    [Route("admin/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "admin")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        protected virtual object ToJsonResult(object data)
        {
            return Content(data.ToJson());
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        protected virtual object Success(object data = null)
        {
            return Content(new { code = 0, msg = "响应成功", data = data }.ToJson());
        }

        protected virtual object Content(ResponeInfo responeInfo)
        {
            return base.Content(responeInfo.ToJson());
        }
        protected async Task<string> UpLoad(IHostingEnvironment env, IFormFile file, string alltype, string type)
        {
            string webRootPath = env.WebRootPath;
            string contentRootPath = env.ContentRootPath;

            if (file.Length > 0)
            {
                var savePath = Path.Combine(contentRootPath, "upload", alltype, type);
                if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

                var saveUrl = Path.Combine(savePath, DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(file.FileName));

                using (var stream = new FileStream(saveUrl, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return saveUrl;
            }
            return null;
        }
    }
}