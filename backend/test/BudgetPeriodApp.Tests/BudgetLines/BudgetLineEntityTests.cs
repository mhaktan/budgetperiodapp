using System;
using Xunit;
using FluentAssertions;
using BudgetPeriodApp.Entities;

namespace BudgetPeriodApp.Tests.BudgetLines
{
    public class BudgetLineEntityTests
    {
        [Fact]
        public void BudgetLine_ShouldBeCreatable()
        {
            // Act
            var entity = new BudgetLine();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void BudgetLine_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new BudgetLine();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void BudgetLine_Description_ShouldAcceptValue()
        {
            var entity = new BudgetLine { Description = "Test Value" };
            entity.Description.Should().Be("Test Value");
        }

    }
}
