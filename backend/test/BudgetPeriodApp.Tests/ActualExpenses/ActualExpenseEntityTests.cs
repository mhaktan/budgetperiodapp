using System;
using Xunit;
using FluentAssertions;
using BudgetPeriodApp.Entities;

namespace BudgetPeriodApp.Tests.ActualExpenses
{
    public class ActualExpenseEntityTests
    {
        [Fact]
        public void ActualExpense_ShouldBeCreatable()
        {
            // Act
            var entity = new ActualExpense();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void ActualExpense_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new ActualExpense();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void ActualExpense_Description_ShouldAcceptValue()
        {
            var entity = new ActualExpense { Description = "Test Value" };
            entity.Description.Should().Be("Test Value");
        }

    }
}
