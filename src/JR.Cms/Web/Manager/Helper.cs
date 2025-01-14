﻿/*
* Copyright(C) 2010-2012 fze.NET
* 
* File Name	: PagerLinkHelper
* Author	: Newmin (new.min@msn.com)
* Create	: 2012/10/10 22:30:43
* Description	:
*
*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using JR.Cms.Conf;
using JR.Cms.Library.CacheService;
using JR.Cms.Library.DataAccess.BLL;
using JR.Stand.Core;
using JR.Stand.Core.Framework;
using JR.Stand.Core.Framework.Web.UI;
using JR.Stand.Core.Utils;

namespace JR.Cms.Web.Manager
{
    internal class Helper
    {
        internal static string BuildPagerInfo(string format, int pageIndex, int recordCount, int pages)
        {
            return UrlPaging.PagerHtml(format, format, pageIndex, recordCount, pages);
            /*
            UrlPager p = new UrlPager(pageIndex, pages);
            p.LinkCount = 5;
            p.RecordCount = recordCount;
            p.FirstPageLink = format;
            p.LinkFormat = format;
            p.SelectPageText = "&nbsp;跳页：";
            p.EnableSelect = true;
            p.PagerTotal = String.Empty;
            p.NextPagerLinkText = "..";
            p.PreviousPagerLinkText = "..";
            p.NextPageText = " > ";
            p.PreviousPageText = " < ";
            p.Style = PagerStyle.Blue;
            return p.ToString();
             */
        }

        internal static string BuildJsonPagerInfo(string firstLinkFormat, string linkFormat, int pageIndex,
            int recordCount, int pages)
        {
            var pagingGetter = new CustomPagingGetter(
                firstLinkFormat, linkFormat,
                 "<<", ">>");
            var pg = UrlPaging.NewPager(pageIndex, pages, pagingGetter);
            pg.RecordCount = recordCount;
            pg.LinkCount = 10;
            pg.PagerTotal = "共{2}条";
            return pg.Pager();
        }


        /// <summary>
        /// 单层树结构
        /// </summary>
        /// <param name="title"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string SingleTree(string title, TreeItem[] array)
        {
            var sb = new StringBuilder();
            sb.Append(
                    "<div class=\"tree\" id=\"single_tree\"><dl><dt class=\"tree-title\"><img src=\"/public/mui/css/old/sys_themes/default/icon_trans.png\" class=\"tree-title\" width=\"24\" height=\"24\"/>")
                .Append(title).Append("</dt>");
            var i = 0;
            foreach (var t in array)
                sb.Append("<dd treeid=\"").Append(t.ID.ToString()).Append("\"><img class=\"")
                    .Append(++i == array.Length ? "tree-item-last" : "tree-item")
                    .Append(
                        "\" src=\"/public/mui/css/old/sys_themes/default/icon_trans.png\" width=\"24\" height=\"24\"/><span class=\"txt\"><a class=\"namelink\" href=\"javascript:;\">")
                    .Append(t.Name)
                    .Append("</a></span></dd>");
            sb.Append("</div>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> GetTemplates()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            var tplRootPath = $"{EnvUtil.GetBaseDirectory()}/templates/";
            var dir = new DirectoryInfo(tplRootPath);

            var dirs = dir.GetDirectories();
            var tpls = new string[dirs.Length];
            if (dir.Exists)
            {
                var i = -1;
                foreach (var d in dirs)
                    if ((d.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                        tpls[++i] = d.Name;
            }

            SettingFile sf;
            string tplConfigFile,
                tplName;

            foreach (var key in tpls)
            {
                tplName = key;

                tplConfigFile = string.Format("{0}{1}/tpl.conf", tplRootPath, key);
                if (File.Exists(tplConfigFile))
                {
                    sf = new SettingFile(tplConfigFile);
                    if (sf.Contains("name")) tplName = sf["name"];
                    //if (sf.Contains("thumbnail"))
                    //{
                    //    tplThumbnail = sf["thumbnail"];
                    //}
                }

                if (!string.IsNullOrEmpty(key)) dict.Add(key, tplName);
            }

            return dict;
        }


        /// <summary>
        /// 获取下拉选框
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="selectValue"></param>
        /// <returns></returns>
        public static string GetOptions(IDictionary<string, string> dict, string selectValue)
        {
            var sb = new StringBuilder();
            foreach (var p in dict)
                sb.Append("<option value=\"").Append(p.Key).Append("\"")
                    .Append(string.Compare(p.Key, selectValue, true) == 0 ? " selected=\"selected\"" : "")
                    .Append(">").Append(p.Value).Append("</option>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取模板选项
        /// </summary>
        /// <param name="tpl"></param>
        /// <returns></returns>
        public static string GetTemplateOptions(string tpl)
        {
            var tpls = GetTemplates();
            var sb = new StringBuilder();
            foreach (var p in tpls)
                sb.Append("<option value=\"").Append(p.Key).Append("\"")
                    .Append(string.Compare(p.Key, tpl, true) == 0 ? " selected=\"selected\"" : "")
                    .Append(">")
                    .Append(p.Value == string.Empty ? p.Key : p.Value)
                    .Append(p.Key == p.Value ? "" : "(" + p.Key + ")")
                    .Append("</option>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取用户组选项
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static string GetUserGroupOptions(int groupId)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var u in CmsLogic.UserBll.GetUserGroups())
            {
                if (u.Id == 1) continue;
                dict.Add(u.Id.ToString(), u.Name);
            }

            return GetOptions(dict, groupId.ToString());
        }

        /// <summary>
        /// 获取站点选项
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static string GetSiteOptions(int siteId)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var e in SiteCacheManager.GetAllSites()) dict.Add(e.SiteId.ToString(), e.Name);
            return GetOptions(dict, siteId.ToString());
        }

        /// <summary>
        /// 生成备份目录路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetBackupFilePath(string filePath)
        {
            return CmsVariables.TEMP_PATH + "/.files/" + filePath;
            const string pattern = "^(.+)(/|\\\\)([^/\\\\]+)$";
            if (Regex.IsMatch(filePath, pattern))
            {
                var match = Regex.Match(filePath, pattern);
                return string.Concat(match.Groups[1].Value, "/.backup/", match.Groups[3].Value);
            }

            return filePath;
        }

        public static string GetCategoryDropOptions(int siteId, int sameLftId)
        {
            var sb = new StringBuilder();
            //加载栏目
            LocalService.Instance.SiteService.HandleCategoryTree(siteId, 1, (category, level, isLast) =>
            {
                //if (sameLftId < 0 || category.Lft != sameLftId)
                //{
                sb.Append("<option value=\"").Append(category.Get().Path).Append("\">");
                for (var i = 0; i < level; i++) sb.Append(CmsCharMap.Dot);
                sb.Append(category.Get().Name).Append("</option>");
                //}
            });

            return sb.ToString();
        }


        public static string GetCategoryIdSelector(int siteId, int categoryId)
        {
            var sb = new StringBuilder();
            //加载栏目
            LocalService.Instance.SiteService.HandleCategoryTree(siteId, 0, (category, level, isLast) =>
            {
                sb.Append("<option class=\"").Append("level level_").Append(level.ToString());
                if (isLast) sb.Append(" last");
                sb.Append("\" value=\"").Append(category.GetDomainId().ToString()).Append("\"");

                if (category.GetDomainId() == categoryId) sb.Append(" selected=\"selected\"");

                sb.Append(">"); //.Append(isLast ? "└" : "├");


                for (var i = 0; i < level - 1; i++)
                    sb.Append(CmsCharMap.Connect);
                //                    if (i == 0 || i == level - 1)
                //                    {
                //                        sb.Append("─");
                //                    }
                //                    else
                //                    {
                //                        sb.Append("├");
                //                    }

                sb.Append(" ").Append(category.Get().Name).Append("</option>");
            });

            return sb.ToString();
        }

        /// <summary>
        /// 如果新增了模板文件,则重新加载模板
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isDir"></param>
        public static void CheckReloadTemplate(string filePath, bool isDir)
        {
            if (filePath.IndexOf("/templates/", StringComparison.Ordinal) == -1) return;
            if (isDir || filePath.EndsWith(".html"))
            {
                Cms.Template.Reload();
            }
        }
    }
}