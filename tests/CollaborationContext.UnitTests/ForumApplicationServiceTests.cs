using System;
using FluentAssertions;
using Moq;
using SaaSOvation.Collaboration.Application.Forums;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;
using SaaSOvation.Collaboration.Domain.Model.Forums;
using SaaSOvation.Common.Domain.Model;
using Xunit;

namespace CollaborationContext.UnitTests
{
    public class ForumApplicationServiceTests
    {
        [Fact]
        public void AssignModeratorToForum_ForumNull_ThrowException()
        {
            var forumQueryService = new Mock<IForumQueryService>();
            var forumRepository = new Mock<IForumRepository>();
            var discussionRepository = new Mock<IDiscussionRepository>();
            var postRepository = new Mock<IPostRepository>();
            var forumIdentityService = new ForumIdentityService(
                discussionRepository.Object,
                forumRepository.Object,
                postRepository.Object);
            var discussionQueryService = new Mock<IDiscussionQueryService>();
            var collaboratorService = new Mock<ICollaboratorService>();
            var sut = new ForumApplicationService(
                forumQueryService.Object,
                forumRepository.Object,
                forumIdentityService,
                discussionQueryService.Object,
                discussionRepository.Object,
                collaboratorService.Object);

            Action action = () => sut.AssignModeratorToForum("1", "1", "1");

            action.ShouldThrow<DomainException>().WithMessage($"Forum not found: 1");
        }
    }
}
