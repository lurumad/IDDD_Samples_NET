using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaaSOvation.Collaboration.Domain.Model.Tenants;

namespace SaaSOvation.Collaboration.Domain.Model.Forums
{
    public interface IForumRepository
    {
        Forum Get(Tenants.Tenant tenantId, ForumId forumId);
        
        ForumId GetNextIdentity();

        void Save(Forum forum);
        void Get(Tenant tenantId);
    }
}
