﻿//
// Copyright (C) 2007-2008 TO2.NET,All rights reseved.
// 
// Project: jr.Cms.Manager
// FileName : Ajax.cs
// Author : PC-CWLIU (new.min@msn.com)
// Create : 2011/10/15 21:16:56
// Description :
//
// Get infromation of this software,please visit our site http://to2.net/cms
//
//

using System;
using T2.Cms.Conf;
using T2.Cms.WebManager;
using JR.DevFw.Framework.Extensions;
using JR.DevFw.Framework.Web.UI;

namespace T2.Cms.Web.WebManager.Handle
{
    public class UploadC:BasePage
    {

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void UploadImage_POST()
        {
            string uploadfor = base.Request["for"];
            string id = base.Request["upload.id"];
            DateTime dt = DateTime.Now;
            string dir = string.Format("/{0}s{1}/image/{2:yyyyMM}/", CmsVariables.RESOURCE_PATH, base.CurrentSite.SiteId.ToString(), dt);
            string name = String.Format("{0}{1:ddHHss}{2}",
                String.IsNullOrEmpty(uploadfor) ? "" : uploadfor + "_",
                dt, String.Empty.RandomLetters(4));

            string file = new FileUpload(dir, name).Upload();
            Response.Write("{" + String.Format("url:'{0}'", file) + "}");
        }

        public void UploadFile_POST()
        {
            string uploadfor = base.Request["for"];
            string id = base.Request["upload.id"];
            DateTime dt = DateTime.Now;
            string dir = string.Format("/{0}s{1}/attachment/{2:yyyyMM}/",
                CmsVariables.RESOURCE_PATH,
                base.CurrentSite.SiteId.ToString(), dt);
            string name = String.Format("{0:ddHHss}{1}", dt, String.Empty.RandomLetters(4));
            string file = new FileUpload(dir, name).Upload();
            Response.Write("{"+String.Format("url:'{0}'",file)+"}");
        }
    }
}