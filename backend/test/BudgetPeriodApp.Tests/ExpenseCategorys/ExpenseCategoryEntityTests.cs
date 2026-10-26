using System;
using Xunit;
using FluentAssertions;
using BudgetPeriodApp.Entities;

namespace BudgetPeriodApp.Tests.ExpenseCategorys
{
    public class ExpenseCategoryEntityTests
    {
        [Fact]
        public void ExpenseCategory_ShouldBeCreatable()
        {
            // Act
            var entity = new ExpenseCategory();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void ExpenseCategory_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new ExpenseCategory();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void ExpenseCategory_Name_ShouldAcceptValue()
        {
            var entity = new ExpenseCategory { Name = "Test Value" };
            entity.Name.Should().Be("Test Value");
        }

        [Fact]
        public void ExpenseCategory_Code_ShouldAcceptValue()
        {
            var entity = new ExpenseCategory { Code = "Test Value" };
            entity.Code.Should().Be("Test Value");
        }

    }
}
