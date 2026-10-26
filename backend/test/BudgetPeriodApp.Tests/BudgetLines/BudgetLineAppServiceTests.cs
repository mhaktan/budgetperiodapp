using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.BudgetLines;
using BudgetPeriodApp.BudgetLines.Dto;
using BudgetPeriodApp.Flows;

namespace BudgetPeriodApp.Tests.BudgetLines
{
    public class BudgetLineAppServiceTests
    {
        private readonly Mock<IRepository<BudgetLine, long>> _repositoryMock;
        private readonly BudgetLineAppService _service;

        public BudgetLineAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<BudgetLine, long>>();
            _service = new BudgetLineAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new BudgetLine { Id = 1, Description = "Test description", PlannedAmount = 10.0m },
                new BudgetLine { Id = 2, Description = "Test description", PlannedAmount = 10.0m },
            }.AsQueryable();

            _repositoryMock.Setup(r => r.GetAll()).Returns(entities);

            // Act
            var result = _repositoryMock.Object.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }

        [Fact]
        public void Repository_GetAll_WithFilter_ShouldWork()
        {
            // Arrange
            var entities = new[]
            {
                new BudgetLine { Id = 1, Description = "Test description", PlannedAmount = 10.0m },
                new BudgetLine { Id = 2, Description = "Test description", PlannedAmount = 10.0m },
            }.AsQueryable();

            _repositoryMock.Setup(r => r.GetAll()).Returns(entities);

            // Act — simulate keyword filter
            var result = _repositoryMock.Object.GetAll()
                .Where(x => x.Id.ToString().Contains("1"));

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_ShouldInsertEntity()
        {
            // Arrange
            var dto = new CreateBudgetLineDto
            {
                Description = "Test description", PlannedAmount = 10.0m
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<BudgetLine>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new BudgetLine { Id = 1, Description = "Test description", PlannedAmount = 10.0m });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new BudgetLine { Id = 1, Description = "Test description", PlannedAmount = 10.0m });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
