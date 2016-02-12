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
        public void when_assign_moderator_to_forum_that_does_not_exists_then_should_thrown_domain_exception()
        {
            // Arrange
            var sut = ForumApplicationServiceTestable.CreateSut(null, null);

            // Act
            Action action = () => sut.AssignModeratorToForum("1", "1", "1");

            // Assert
            action.ShouldThrow<DomainException>().WithMessage("Forum not found: 1");
        }

        [Fact]
        public void when_assign_moderator_to_forum_exists_then_should_apply_forum_moderator_changed_event()
        {
            // Arrange
            var forum = Forum();
            var sut = ForumApplicationServiceTestable.CreateSut(forum, Moderator());

            // Act
            sut.AssignModeratorToForum("1", "1", "1");

            // Assert
            forum.GetMutatingEvents()
                .SingleOrDefault(e => e.GetType() == typeof (ForumModeratorChanged))
                .Should()
                .NotBe(null);
        }

        Forum Forum()
        {
            var forum = new Forum(
                new Tenant("1"),
                new ForumId("1"),
                Creator(),
                Moderator(),
                "test",
                "test",
                "test");

            return forum;
        }

        Creator Creator()
        {
            var creator = new Creator("luis", "luis", "lruiz@plainconcepts.com");
            return creator;
        }

        Moderator Moderator()
        {
            return new Moderator("roberto", "roberto", "rgonzalez@plainconcepts.com");
        }

        public class ForumApplicationServiceTestable : ForumApplicationService
        {
            ForumApplicationServiceTestable(
                Mock<IForumQueryService> forumQueryService,
                Mock<IForumRepository> forumRepository,
                ForumIdentityService forumIdentityService,
                Mock<IDiscussionQueryService> discussionQueryService,
                Mock<IDiscussionRepository> discussionRepository,
                Mock<ICollaboratorService> collaboratorService) :
            base(
                forumQueryService.Object,
                forumRepository.Object,
                forumIdentityService,
                discussionQueryService.Object,
                discussionRepository.Object,
                collaboratorService.Object)
            {
            }

            public static ForumApplicationServiceTestable CreateSut(Forum forum, Moderator moderator)
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

                forumRepository
                    .Setup(x => x.Get(It.IsAny<Tenant>(), It.IsAny<ForumId>()))
                    .Returns(forum);

                collaboratorService
                    .Setup(x => x.GetModeratorFrom(It.IsAny<Tenant>(), It.IsAny<string>()))
                    .Returns(moderator);

                return new ForumApplicationServiceTestable(
                    forumQueryService,
                    forumRepository,
                    forumIdentityService,
                    discussionQueryService,
                    discussionRepository,
                    collaboratorService);
            }
        }
    }
}
