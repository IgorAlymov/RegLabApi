using ConfigurationManager.ConfigurationManager.API.Models;
using ConfigurationManager.ConfigurationManager.Application.Services;
using ConfigurationManager.ConfigurationManager.Domain.Entities;
using ConfigurationManager.ConfigurationManager.Domain.Events;
using ConfigurationManager.ConfigurationManager.Infrastructure.Data.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace ConfigurationManager.ConfigurationManager.Tests.Services;

public class ConfigurationServiceTests
{
    private readonly Mock<IConfigurationRepository<Configuration>> configurationRepositoryMock;
    private readonly Mock<IRepository<BaseConfigurationVersion>> configurationVersionRepositoryMock;
    private readonly Mock<IPublisher> mediatorMock;
    private readonly Mock<ILogger<ConfigurationService>> logger;
    private readonly ConfigurationService configurationService;

    public ConfigurationServiceTests()
    {
        configurationRepositoryMock = new Mock<IConfigurationRepository<Configuration>>();
        configurationVersionRepositoryMock = new Mock<IRepository<BaseConfigurationVersion>>();
        mediatorMock = new Mock<IPublisher>();
        logger = new Mock<ILogger<ConfigurationService>>();
        configurationService = new ConfigurationService(configurationRepositoryMock.Object,
            configurationVersionRepositoryMock.Object, mediatorMock.Object, logger.Object);
    }

    [Theory(DisplayName = "CreateConfigurationTestAsync")]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public async Task CreateConfigurationTestAsync(bool exist, bool result)
    {
        var userId = Guid.NewGuid();
        var configurationCreateDto = new ConfigurationCreateDto
        {
            UserId = userId, Name = "TestConfig", Description = "Test Description"
        };

        configurationRepositoryMock
            .Setup(repo => repo.ExistsAsync(configurationCreateDto.UserId, configurationCreateDto.Name))
            .ReturnsAsync(exist);
        configurationRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Configuration>(), new CancellationToken()))
            .ReturnsAsync(new Configuration());
        configurationVersionRepositoryMock.Setup(repo =>
                repo.AddAsync(It.IsAny<BaseConfigurationVersion>(), new CancellationToken()))
            .ReturnsAsync(new FontsConfigurationVersion());
        mediatorMock.Setup(m => m.Publish(It.IsAny<ConfigurationCreatedEvent>(), new CancellationToken()))
            .Returns(Task.CompletedTask);

        var configuration = await configurationService.CreateConfigurationAsync(configurationCreateDto);

        configuration.IsSuccess.Should().Be(result);
    }
}
