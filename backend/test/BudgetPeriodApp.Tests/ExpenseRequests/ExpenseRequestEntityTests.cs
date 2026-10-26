using System;
using Xunit;
using FluentAssertions;
using BudgetPeriodApp.Entities;

namespace BudgetPeriodApp.Tests.ExpenseRequests
{
    public class ExpenseRequestEntityTests
    {
        [Fact]
        public void ExpenseRequest_ShouldBeCreatable()
        {
            // Act
            var entity = new ExpenseRequest();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void ExpenseRequest_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new ExpenseRequest();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void ExpenseRequest_Title_ShouldAcceptValue()
        {
            var entity = new ExpenseRequest { Title = "Test Value" };
            entity.Title.Should().Be("Test Value");
        }

    }
}
