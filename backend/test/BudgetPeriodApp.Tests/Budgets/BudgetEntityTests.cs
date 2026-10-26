using System;
using Xunit;
using FluentAssertions;
using BudgetPeriodApp.Entities;

namespace BudgetPeriodApp.Tests.Budgets
{
    public class BudgetEntityTests
    {
        [Fact]
        public void Budget_ShouldBeCreatable()
        {
            // Act
            var entity = new Budget();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void Budget_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new Budget();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void Budget_Title_ShouldAcceptValue()
        {
            var entity = new Budget { Title = "Test Value" };
            entity.Title.Should().Be("Test Value");
        }

    }
}
