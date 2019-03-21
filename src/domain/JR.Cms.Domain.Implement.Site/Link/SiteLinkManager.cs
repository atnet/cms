﻿using System.Collections.Generic;
using JR.Cms.Domain.Interface.Site;
using JR.Cms.Domain.Interface.Site.Link;

namespace JR.Cms.Domain.Implement.Site.Link
{
    internal class SiteLinkManager:ISiteLinkManager
    {
        private ISiteRepo _siteRep;
        private ISite _site;

        public SiteLinkManager(ISiteRepo siteRep,ISite site)
        {
            this._siteRep = siteRep;
            this._site = site;
        }

        public bool DeleteLink(int linkId)
        {
           return this._siteRep.DeleteSiteLink(this._site.GetAggregaterootId(), linkId);
        }


        public ISiteLink GetLinkById(int linkId)
        {
            return this._siteRep.GetSiteLinkById(this._site.GetAggregaterootId(), linkId);
        }

        public IEnumerable<ISiteLink> GetLinks(SiteLinkType type)
        {
            return this._siteRep.GetSiteLinks(this._site.GetAggregaterootId(), type);
        }
    }
}
