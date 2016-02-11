using System;
using System.Linq;
using FluentAssertions;
using Moq;
using SaaSOvation.Collaboration.Application.Forums;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;
using SaaSOvation.Collaboration.Domain.Model.Forums;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
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

        [Fact]
        public void AssignModeratorToForum_ForumValid_Success()
        {
            var forumQueryService = new Mock<IForumQueryService>();
            var forumRepository = new Mock<IForumRepository>();
            var creator = new Creator("luis", "luis", "lruiz@plainconcepts.com");
            var moderator = new Moderator("roberto", "roberto", "rgonzalez@plainconcepts.com");
            var forum = new Forum(
                new Tenant("1"),
                new ForumId("1"),
                creator,
                moderator, "test", "test", "test");
            forumRepository.Setup(x => x.Get(It.IsAny<Tenant>(), It.IsAny<ForumId>())).Returns(forum);
            var discussionRepository = new Mock<IDiscussionRepository>();
            var postRepository = new Mock<IPostRepository>();
            var forumIdentityService = new ForumIdentityService(
                discussionRepository.Object,
                forumRepository.Object,
                postRepository.Object);
            var discussionQueryService = new Mock<IDiscussionQueryService>();
            var collaboratorService = new Mock<ICollaboratorService>();
            collaboratorService.Setup(x => x.GetModeratorFrom(It.IsAny<Tenant>(), It.IsAny<string>())).Returns(moderator);
            var sut = new ForumApplicationService(
                forumQueryService.Object,
                forumRepository.Object,
                forumIdentityService,
                discussionQueryService.Object,
                discussionRepository.Object,
                collaboratorService.Object);

            sut.AssignModeratorToForum("1", "1", "1");

            forum.GetMutatingEvents()
                .SingleOrDefault(e => e.GetType() == typeof (ForumModeratorChanged))
                .Should()
                .NotBe(null);
        }
    }
}
