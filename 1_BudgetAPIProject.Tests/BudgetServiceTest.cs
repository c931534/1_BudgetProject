using System;
using System.Collections.Generic;
using BudgetAPIProject;
using NSubstitute;
using NUnit.Framework;

namespace _1_BudgetAPIProject.Tests;

[TestFixture]
public class BudgetServiceTest
{
    [Test]
    public void FullMonth()
    {
        var budgetRepo = Substitute.For<IBudgetRepo>();
        budgetRepo.GetAll().Returns(new List<Budget>()
        {
            new Budget(){YearMonth = "202412", Amount = "3100"},
            
        })
        var budgetService = new BudgetService(budgetRepo);
        var start = new DateTime(2024, 12, 01);
        var end = new DateTime(2024, 12, 31);
        var amount = budgetService.Query(start, end);
        Assert.Equals(amount, 0);
    }
    
}