using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace JR.Stand.Abstracts.Web
{
    public interface ICompatibleRequest
    {
        string Method();
       string GetHeader(string key);
       string GetHost();

       string GetApplicationPath();
       string GetScheme();
       string GetPath();
       string GetQueryString();
       bool TryGetHeader(string key, out StringValues value);
       string UrlEncode(string url);
       string UrlDecode(string url);

       /// <summary>
       /// 获取查询参数
       /// </summary>
       /// <returns></returns>
       StringValues Query(string key);

       /// <summary>
       ///  获取表单参数
       /// </summary>
       /// <returns></returns>
       StringValues Form(string key);

       bool TryGetCookie(string member, out string o);
       IEnumerable<string> CookiesKeys();
       string GetEncodedUrl();
       IEnumerable<string> FormKeys();
       T ParseFormToEntity<T>();
       string GetParameter(string key);

       /// <summary>
       /// 获取上传的文件
       /// </summary>
       /// <param name="key"></param>
       /// <returns></returns>
       ICompatiblePostedFile File(string key);
       /// <summary>
       /// 按序号获取上传的文件
       /// </summary>
       /// <param name="i"></param>
       /// <returns></returns>
       ICompatiblePostedFile FileIndex(int i);
    }
}