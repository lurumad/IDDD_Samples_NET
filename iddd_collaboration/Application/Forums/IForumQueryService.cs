using System.Collections.Generic;
using SaaSOvation.Collaboration.Application.Forums.Data;

namespace SaaSOvation.Collaboration.Application.Forums
{
    public interface IForumQueryService
    {
        IList<ForumData> GetAllForumsDataByTenant(string tenantId);
        ForumData GetForumDataById(string tenantId, string forumId);
        ForumDiscussionsData GetForumDiscussionsDataById(string tenantId, string forumId);
        string GetForumIdByExclusiveOwner(string tenantId, string exclusiveOwner);
    }
}