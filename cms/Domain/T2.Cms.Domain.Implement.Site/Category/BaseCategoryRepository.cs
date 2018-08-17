﻿using T2.Cms.Domain.Interface.Site;
using T2.Cms.Domain.Interface.Site.Category;
using T2.Cms.Domain.Interface.Site.Extend;
using T2.Cms.Domain.Interface.Site.Template;

namespace T2.Cms.Domain.Implement.Site.Category
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseCategoryRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryRep"></param>
        /// <param name="extendRep"></param>
        /// <param name="tempRep"></param>
        /// <param name="categoryId"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        public ICategory CreateCategory(
            ICategoryRepository categoryRep,
            IExtendFieldRepository extendRep,
            ITemplateRepository tempRep,
            int categoryId,
            ISite site)
        {
            return new Category(categoryRep, extendRep, tempRep,categoryId, site);
        }
    }
}
