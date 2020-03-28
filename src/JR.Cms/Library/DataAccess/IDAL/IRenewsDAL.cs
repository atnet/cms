//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : OSite
//  @ File Name : ICommentDAL.cs
//  @ Date : 2011/8/23
//  @ Author : 
//
//

namespace JR.Cms.Library.DataAccess.IDAL
{
    public interface IRenewsDAL
    {
        /// <summary>
        /// ���µ���
        /// </summary>
        /// <param name="archiveId"></param>
        /// <param name="agree"></param>
        void UpdateEvaluate(string archiveId, bool agree);

        void CreateReviews(string id);
        void DeleteReviews(string id);
        string GetReviewsMembers(string id);
        void UpdateReviews(string id, string members);
    }
}