﻿using T2.Cms.DataTransfer;
using System.Collections.Generic;
using T2.Cms.Domain.Interface.Content;

namespace T2.Cms.ServiceContract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContentServiceContract
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="typeIndent"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        IBaseContent GetContent(int siteId,string typeIndent, int contentId);

      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="typeIndent"></param>
        /// <param name="contentId"></param>
        /// <param name="relatedLinkId"></param>
        void RemoveRelatedLink(int siteId, string typeIndent, int contentId, int relatedLinkId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="typeIndent"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        IEnumerable<RelatedLinkDto> GetRelatedLinks(int siteId, string typeIndent, int contentId);

        IDictionary<int, RelateIndent> GetRelatedIndents();

        void SetRelatedIndents(IDictionary<int, RelateIndent> relatedIndents);

        /// <summary>
        /// 保存关联文档
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="link"></param>
        int SaveRelatedLink(int siteId, RelatedLinkDto link);
    }
}
