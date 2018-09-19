﻿

using T2.Cms;
using T2.Cms.CacheService;
using T2.Cms.Conf;
using T2.Cms.Domain.Interface.Common.Language;
using T2.Cms.Domain.Interface.Enum;
using T2.Cms.Domain.Interface.Site.Extend;
using T2.Cms.Utility;

namespace T2.Cms.Web
{
    using T2.Cms.DataTransfer;
    using JR.DevFw.Template;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    public class PageArchive:ITemplateVariableObject
    {
        private string _properies;
        private IDictionary<String, String> _dict;
        private string _tagsHtml;
        private String _url;

        public PageArchive(ArchiveDto archive)
        {
            this.Archive = archive;
        }
        
        private static string FormatUrl(UrlRulePageKeys key, params string[] datas)
        {
            string urlFormat = (Settings.TPL_UseFullPath ?
                Cms.Context.SiteDomain+"/" : Cms.Context.SiteAppPath)
                + TemplateUrlRule.Urls[TemplateUrlRule.RuleIndex, (int)key];
            return datas == null ? urlFormat : String.Format(urlFormat, datas);
        }

        public IDictionary<string, string> __dict__
        {
            get
            {
                if (_dict == null)
                {
                    _dict=new Dictionary<string,string>();
                   
                    foreach (IExtendValue value in this.Archive.ExtendValues)
                    {
                        _dict.Add(value.Field.Name, value.Value);
                    }
                }
                return _dict;
            }
        }

        /// <summary>
        /// 文档
        /// </summary>
        internal ArchiveDto Archive { get; private set; }

        /// <summary>
        /// 编号
        /// </summary>
        [TemplateVariableField("编号")]
        public string Id
        {
            get { return this.Archive.Id.ToString(); }
        }
        
        [TemplateVariableField("别名")]
        public String Alias
        {
            get{
                return String.IsNullOrEmpty(this.Archive.Alias) ? this.Archive.StrId : this.Archive.Alias; 
            }
        }
        
        [TemplateVariableField("地址")]
        public String Url
        {
            get{
                if (this._url == null)
                {
                    String prefix = (Settings.TPL_UseFullPath ? Cms.Context.SiteDomain : Cms.Context.SiteAppPath);
                    this._url = prefix + "/" + this.Archive.Url+".html";
                }
                return this._url;
            }
        }


        /// <summary>
        /// 标题
        /// </summary>
        [TemplateVariableField("标题")]
        public string Title
        {
            get
            {
                return this.Archive.Title;
            }
        }

        [TemplateVariableField("子标题")]
        public string SmallTitle
        {
            get
            {
                return this.Archive.SmallTitle;
            }
        }

        [TemplateVariableField("子标题")]
        public string ContactSmallTitle
        {
            get { return String.IsNullOrEmpty(this.Archive.SmallTitle) ? "" : "-" + this.Archive.SmallTitle; }
        }


        /// <summary>
        /// 作者
        /// </summary>
        [TemplateVariableField("作者")]
        public string Author
        {
            get
            {
                return ServiceCall.Instance.UserService.GetUserRealName(this.Archive.PublisherId) ?? "未知";
            }
        }

        /// <summary>
        /// 来源
        /// </summary>
        [TemplateVariableField("来源")]
        public string Source
        {
            get
            {
                string source = this.Archive.Source;
                return String.IsNullOrEmpty(source) ? "原创" : source;
            }
        }

        /// <summary>
        /// 大纲
        /// </summary>
        [TemplateVariableField("大纲")]
        public string Outline
        {
            get
            {
                if (!String.IsNullOrEmpty(this.Archive.Outline))
                    return this.Archive.Outline;
                return ArchiveUtility.GetOutline(this.Archive.Outline, 200);
            }
        }

        /// <summary>
        /// 内容
        /// </summary>
        [TemplateVariableField("内容")]
        public string Content
        {
            get
            {
                return this.Archive.Content;
            }
        }

        /// <summary>
        /// 缩略图
        /// </summary>
        [TemplateVariableField("缩略图")]
        public string Thumbnail
        {
            get
            {
                return this.Archive.Thumbnail;
            }
        }

        /// <summary>
        /// 标签
        /// </summary>
        [TemplateVariableField("标签Tags")]
        public string Tags
        {
            get
            {
                return this.Archive.Tags;
            }
        }


        /// <summary>
        /// 标签
        /// </summary>
        [TemplateVariableField("标签Tags，HTML内容")]
        public string TagsHtml
        {
            get
            {
                if (this._tagsHtml == null)
                {
                    if (this.Archive.Tags == String.Empty)
                    {
                        return Cms.Language.Get(LanguagePackageKey.PAGE_NO_TAGS);
                    }

                    StringBuilder sb = new StringBuilder();
                    string[] tagArr = this.Archive.Tags.Split(',');
                 
                    int j=0;
                    foreach (string tag in tagArr)
                    {
                        if(j++!=0)sb.Append(",");

                        sb.Append("<a href=\"")
                            .Append(FormatUrl(UrlRulePageKeys.Tag, HttpUtility.UrlEncode(tag)))
                            .Append("\" search-url=\"")
                            .Append(FormatUrl(UrlRulePageKeys.Search, HttpUtility.UrlEncode(tag), String.Empty))
                            .Append("\">")
                            .Append(tag)
                            .Append("</a>");
                    }

                    this._tagsHtml = sb.ToString();
                }
                return this._tagsHtml;
            }
        }

        /// <summary>
        /// 访问统计
        /// </summary>
        [TemplateVariableField("访问量")]
        public string Count
        {
            get
            {
                return this.Archive.ViewCount.ToString();
            }
        }

        /// <summary>
        /// 发布时间
        /// </summary>
        [TemplateVariableField("发布时间")]
        public string Publish
        {
            get
            {
                return String.Format("{0:yyyy-MM-dd HH:mm}", this.Archive.CreateTime);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        [TemplateVariableField("修改时间")]
        public string Modify
        {
            get
            {
                return String.Format("{0:yyyy-MM-dd HH:mm}", this.Archive.UpdateTime);
            }
        }

        /// <summary>
        /// 扩展属性列表
        /// </summary>
        [TemplateVariableField("扩展属性")]
        public string Properies
        {
            get
            {
                if (_properies == null)
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<ul class=\"extend_field_list\">");

                    foreach (IExtendValue value in this.Archive.ExtendValues)
                    {
                        if (!String.IsNullOrEmpty(value.Value))
                        {
                            sb.Append("<li class=\"extend_")
                               .Append(value.Field.GetDomainId().ToString()).Append("\"><span class=\"attrName\">")
                                .Append(value.Field.Name).Append(":</span><span class=\"value\">")
                                .Append(value.Value).Append("</span></li>");
                        }
                    }

                    sb.Append("</ul>");
                    _properies = sb.ToString();

                }
                return _properies;
            }
        }

        public void AddData(string key, string data)
        {
            this.__dict__.Add(key, data);
        }

        public void RemoveData(string key)
        {
            this.__dict__.Remove(key);
        }
    }
}
