﻿//
// Copyright (C) 2007-2008 TO2.NET,All rights reseved.
// 
// Project: jr.Cms.Manager
// FileName : BasePage.cs
// Author : PC-CWLIU (new.min@msn.com)
// Create : 2011/10/17 9:33:57
// Description :
//
// Get infromation of this software,please visit our site http://to2.net/cms
//
//

using System;
using JR.Cms.Conf;
using JR.DevFw.Template;
using JR.Stand.Core;

namespace JR.Cms.WebImpl.WebManager
{
    public class ManagerTemplate:ReflectTemplateClass
    {
        private static readonly string js;
        private static readonly string css;
        private static readonly string iconTreeCss;

        static ManagerTemplate()
        {
            js = String.Format("<script type=\"text/javascript\" charset=\"utf-8\" src=\"/{0}mui/js/base.js?{1}\"></script>",CmsVariables.FRAMEWORK_PATH,CmsVariables.VERSION);
            css = "<link src=\"Stylesheet\" type=\"text/css\" href=\"public/assets/base.css\"/>"
                +"<link src=\"Stylesheet\" type=\"text/css\" href=\"public/assets/icon-font.css\"/>"
                + "<link rel=\"Stylesheet\" type=\"text/css\" href=\"?res=c3R5bGU=&amp;" + Cms.Version + ".css\"/>";
            iconTreeCss="<link rel=\"Stylesheet\" type=\"text/css\" href=\"public/mui/css/old/sys_themes/default/btn_spirites.css?v=" + Cms.Version + "\"/>";
        }

      
        public static JR.DevFw.Template.TemplateHandler<ITemplateClass> TemplateExc = (ITemplateClass obj, ref string content) =>
        {
            MicroTemplateEngine tpl = new MicroTemplateEngine(obj);
            content=tpl.Execute(content);
        };

        /// <summary>
        /// 加载脚本库
        /// </summary>
        /// <returns></returns>
        [TemplateTag]
        public string JS()
        {
            return js;
        }

        /// <summary>
        /// 基础路径
        /// </summary>
        /// <returns></returns>
        [TemplateTag]
        public string PATH()
        {
            return ResourceMap.BasePath;
        }

        /// <summary>
        /// 加载样式
        /// </summary>
        /// <returns></returns>
        [TemplateTag]
        public string Css()
        {
            return css;
        }
        
        [TemplateTag]
        public string TreeCss()
        {
        	return iconTreeCss;
        }
        
        [TemplateTag]
        public string IconCss()
        {
        	return iconTreeCss;
        }

        /// <summary>
        /// 加载符号
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [TemplateTag]
        public string Icon(string key)
        {
            const string tpl = "<img src=\"public/mui/css/old/sys_themes/default/icon_trans.png\" width=\"18\" height=\"18\" class=\"icon {0}\"/>";
            string cssKey = string.Empty;

            switch (key)
            {
                case "create": cssKey = "icon003a2"; break;
                case "delete": cssKey = "icon003a3"; break;
                case "refresh":
                case "publish":
                    cssKey = "icon003a13";
                    break;
                case "comment":
                    cssKey = "icon034a1";
                    break;

                //编辑
                case "edit":
                    cssKey = "icon003a4";
                    break;
            }

            return string.Format(tpl, cssKey);
        }

        public static string GetScriptTag()
        {
            return js;
        }

        public static string GetCssTag()
        {
            return css;
        }
    }
}

