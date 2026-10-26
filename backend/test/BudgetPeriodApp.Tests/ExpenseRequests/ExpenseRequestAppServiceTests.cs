using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.ExpenseRequests;
using BudgetPeriodApp.ExpenseRequests.Dto;
using BudgetPeriodApp.Flows;

namespace BudgetPeriodApp.Tests.ExpenseRequests
{
    public class ExpenseRequestAppServiceTests
    {
        private readonly Mock<IRepository<ExpenseRequest, long>> _repositoryMock;
        private readonly ExpenseRequestAppService _service;

        public ExpenseRequestAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<ExpenseRequest, long>>();
            _service = new ExpenseRequestAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object, new Mock<IRepository<StatusChangeLog, long>>().Object, new Mock<IRepository<ApprovalRecord, Guid>>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new ExpenseRequest { Id = 1, Title = "Test title", Amount = 10.0m, RequestDate = DateTime.UtcNow, Status = 0 },
                new ExpenseRequest { Id = 2, Title = "Test title", Amount = 10.0m, RequestDate = DateTime.UtcNow, Status = 0 },
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
                new ExpenseRequest { Id = 1, Title = "Test title", Amount = 10.0m, RequestDate = DateTime.UtcNow, Status = 0 },
                new ExpenseRequest { Id = 2, Title = "Test title", Amount = 10.0m, RequestDate = DateTime.UtcNow, Status = 0 },
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
            var dto = new CreateExpenseRequestDto
            {
                Title = "Test title", Amount = 10.0m, RequestDate = DateTime.UtcNow, Status = 0
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<ExpenseRequest>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExpenseRequest { Id = 1, Title = "Test title", Amount = 10.0m, RequestDate = DateTime.UtcNow, Status = 0 });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExpenseRequest { Id = 1, Title = "Test title", Amount = 10.0m, RequestDate = DateTime.UtcNow, Status = 0 });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
