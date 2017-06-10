﻿/*
* Copyright(C) 2010-2013 Z3Q.NET
* 
* File Name	: CmsEventRegister.cs
* publisher_id	: Newmin (new.min@msn.com)
* Create	: 2013/05/21 19:59:54
* Description	:
*
*/

using System.Linq;
using JR.Cms;
using JR.Cms.BLL;
using JR.Cms.CacheService;
using JR.Cms.Conf;
using JR.Cms.Domain.Interface.User;
using JR.Cms.Domain.Interface._old;
using JR.Cms.Resource;

namespace JR.Cms
{
    public class CmsEventRegister
    {
        /// <summary>
        /// CMS初始化
        /// </summary>
        public static void Init()
        {

            //读取站点
            if (Cms.Installed)
            {
                Cms.RegSites(SiteCacheManager.GetAllSites().ToArray());
            }

            //内嵌资源释放
            SiteResourceInit.Init();

            //设置可写权限
            Cms.Utility.SetDirCanWrite(CmsVariables.RESOURCE_PATH);
            Cms.Utility.SetDirCanWrite("templates/");
            Cms.Utility.SetDirCanWrite(CmsVariables.FRAMEWORK_PATH);
            Cms.Utility.SetDirCanWrite(CmsVariables.PLUGIN_PATH);
            Cms.Utility.SetDirCanWrite(CmsVariables.TEMP_PATH + "update");
            Cms.Utility.SetDirHidden("config");
            Cms.Utility.SetDirHidden("bin");
        }
    }
}