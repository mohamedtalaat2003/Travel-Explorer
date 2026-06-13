using AutoMapper;
using Moq;
using Travel_Explorer.Application.Features.Activities;
using Travel_Explorer.Application.Features.Activities.Commands.CreateActivity;
using Travel_Explorer.Application.Features.Activities.Commands.DeleteActivity;
using Travel_Explorer.Application.Features.Activities.Commands.UpdateActivity;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Interfaces;
using Xunit;

namespace UnitTest.TravelExplorer.ActivityTests
{
    public class ActivityCommandHandlerTests
    {
        [Fact]
        public async Task CreateActivityCommandHandler_Should_Return_Created_Activity()
        {

            //arrange
            var activityRepoMock = new Mock<IGenericRepository<Activity>>();
           
            var mappermock = new Mock<IMapper>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            unitOfWorkMock.Setup(uow => uow.Repository<Activity>())
                .Returns(activityRepoMock.Object);

            activityRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Activity>()))
                .Returns(Task.CompletedTask);

            unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            mappermock.Setup(m => m.Map<Activity>(It.IsAny<CreateActivityCommand>()))
                .Returns(new Activity());
              

            var handler = new CreateActivityCommandHandler(unitOfWorkMock.Object,mappermock.Object);

            var command = new CreateActivityCommand("Test", "This is a test activity.", "test-icon.png", new List<string> { "image1.png", "image2.png" }, 1);

            //act
            var result =await handler.Handle(command, CancellationToken.None);

            //assert

            activityRepoMock.Verify(x=>x.AddAsync(It.IsAny<Activity>()), Times.Once);
            unitOfWorkMock.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(2)]
        public async Task Handle_Should_Delete_Activity(int id)
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var activityRepoMock = new Mock<IGenericRepository<Activity>>();

            unitOfWorkMock
                .Setup(uow => uow.Repository<Activity>())
                .Returns(activityRepoMock.Object);

            activityRepoMock.Setup(repo => repo.GetAsync(id))
                .ReturnsAsync(new Activity { Id = id, IsDeleted = false });

            activityRepoMock
                .Setup(repo => repo.Delete(id))
                .Returns(Task.CompletedTask);

            unitOfWorkMock
                .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var handler = new DeleteActivityCommandHandler(unitOfWorkMock.Object);
            var command = new DeleteActivityCommand(id);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            activityRepoMock.Verify(
                x => x.GetAsync(id),
                Times.Once);

            unitOfWorkMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Update_Activity()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var activityRepoMock = new Mock<IGenericRepository<Activity>>();
            var mapperMock = new Mock<IMapper>();



            unitOfWorkMock.Setup(uow=>uow.Repository<Activity>()).
                Returns(activityRepoMock.Object);

            activityRepoMock.Setup(repo => repo.GenericEntitiesWithSpec(It.IsAny<ActivitySpecification>())).
                ReturnsAsync(new Activity { Id = 2, Name = "Old Name", Description = "Old Description" ,Icon = "old-icon.png",ImageUrls = new List<string> { "old-image1.png", "old-image2.png" },DestinationId = 1 });


            unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).
                ReturnsAsync(1);

            var handler = new UpdateActivityCommandHandler(unitOfWorkMock.Object, mapperMock.Object);
            var command = new UpdateActivityCommand( "New Name", "New Description", "new-icon.png", new List<string> { "new-image1.png", "new-image2.png" }, 1);

            var result = await handler.Handle(command, CancellationToken.None);

            activityRepoMock.Verify(x => x.GenericEntitiesWithSpec(It.IsAny<ActivitySpecification>()), Times.AtMost(2));
            unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

    

    }
}
