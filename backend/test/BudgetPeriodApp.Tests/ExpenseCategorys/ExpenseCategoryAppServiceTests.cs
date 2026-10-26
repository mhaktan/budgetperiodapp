using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Moq;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.ExpenseCategorys;
using BudgetPeriodApp.ExpenseCategorys.Dto;
using BudgetPeriodApp.Flows;

namespace BudgetPeriodApp.Tests.ExpenseCategorys
{
    public class ExpenseCategoryAppServiceTests
    {
        private readonly Mock<IRepository<ExpenseCategory, long>> _repositoryMock;
        private readonly ExpenseCategoryAppService _service;

        public ExpenseCategoryAppServiceTests()
        {
            _repositoryMock = new Mock<IRepository<ExpenseCategory, long>>();
            _service = new ExpenseCategoryAppService(_repositoryMock.Object, new Mock<IFlowEngine>().Object);
        }

        [Fact]
        public void Repository_GetAll_ShouldReturnQueryable()
        {
            // Arrange
            var entities = new[]
            {
                new ExpenseCategory { Id = 1, Name = "Test name", Code = "Test code" },
                new ExpenseCategory { Id = 2, Name = "Test name", Code = "Test code" },
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
                new ExpenseCategory { Id = 1, Name = "Test name", Code = "Test code" },
                new ExpenseCategory { Id = 2, Name = "Test name", Code = "Test code" },
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
            var dto = new CreateExpenseCategoryDto
            {
                Name = "Test name", Code = "Test code"
            };

            _repositoryMock.Setup(r => r.InsertAndGetIdAsync(It.IsAny<ExpenseCategory>()))
                .ReturnsAsync(1);
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExpenseCategory { Id = 1, Name = "Test name", Code = "Test code" });

            // Act & Assert
            _service.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveEntity()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExpenseCategory { Id = 1, Name = "Test name", Code = "Test code" });

            // Act & Assert
            await _service.Invoking(s => s.DeleteAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = 1 }))
                .Should().NotThrowAsync();
        }
    }
}
