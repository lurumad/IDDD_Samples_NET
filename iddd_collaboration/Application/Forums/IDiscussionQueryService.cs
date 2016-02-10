using System.Collections.Generic;
using SaaSOvation.Collaboration.Application.Forums.Data;

namespace SaaSOvation.Collaboration.Application.Forums
{
    public interface IDiscussionQueryService
    {
        IList<DiscussionData> GetAllDiscussionsDataByForum(string tenantId, string forumId);
        DiscussionData GetDiscussionDataById(string tenantId, string discussionId);
        string GetDiscussionIdByExclusiveOwner(string tenantId, string exclusiveOwner);
        DiscussionPostsData GetDiscussionPostsDataById(string tenantId, string discussionId);
    }
}