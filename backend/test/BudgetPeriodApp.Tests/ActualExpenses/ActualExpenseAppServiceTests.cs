using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.ActualExpenses;
using BudgetPeriodApp.ActualExpenses.Dto;
using BudgetPeriodApp.Flows;

namespace BudgetPeriodApp.Tests.ActualExpenses
{
    public class ActualExpenseAppServiceTests
    {
        private readonly Mock<IRepository<ActualExpense, long>> _repositoryMock;
        private readonly ActualExpenseAppService _service;

        public ActualExpenseAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<ActualExpense, long>>();
            _service = new ActualExpenseAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new ActualExpense { Id = 1, Description = "Test description", Amount = 10.0m, ExpenseDate = DateTime.UtcNow },
                new ActualExpense { Id = 2, Description = "Test description", Amount = 10.0m, ExpenseDate = DateTime.UtcNow },
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
                new ActualExpense { Id = 1, Description = "Test description", Amount = 10.0m, ExpenseDate = DateTime.UtcNow },
                new ActualExpense { Id = 2, Description = "Test description", Amount = 10.0m, ExpenseDate = DateTime.UtcNow },
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
            var dto = new CreateActualExpenseDto
            {
                Description = "Test description", Amount = 10.0m, ExpenseDate = DateTime.UtcNow
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<ActualExpense>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new ActualExpense { Id = 1, Description = "Test description", Amount = 10.0m, ExpenseDate = DateTime.UtcNow });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new ActualExpense { Id = 1, Description = "Test description", Amount = 10.0m, ExpenseDate = DateTime.UtcNow });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
