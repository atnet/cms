﻿/**
 * Copyright (C) 2007-2015 Z3Q.NET,All rights reseved.
 * Get more infromation of this software,please visit site http://k3f.net/cms
 * 
 * name : Helper.cs
 * author : newmin (new.min@msn.com)
 * date : 2012/12/01 23:00:00
 * description : 
 * history : 
 */

using System.Data;
using System.Web;
using J6.DevFw.Framework.Extensions;
using Newtonsoft.Json;
using J6.Cms;

namespace com.plugin.sso.Core
{
    internal class Helper
    {
        private static string _domain;


        public  static void PagerJson(HttpResponse rsp,DataTable rows, string pager)
        {
            const string fmt = "{'pager':'%pager%','rows':%html%}";
            rsp.Write(fmt.Template(
               pager.Replace("'", "\\'"),
                JsonConvert.SerializeObject(rows)
               ));
            rsp.ContentType = "application/json";
        }

        public static string GetResouceDomain()
        {
            return _domain ??(_domain =Cms.Context.ResourceDomain);
        }
    }
}
