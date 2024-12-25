using System;
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
        budgetRepo.GetAll().Returns([
            new Budget {YearMonth = "202412", Amount = 0},
            new Budget {YearMonth = "202501", Amount = 1000}
        ]);
        var budgetService = new BudgetService(budgetRepo);
        var start = new DateTime(2024, 12, 01);
        var end = new DateTime(2024, 12, 31);
        var amount = budgetService.Query(start, end);
       Assert.That(amount, Is.EqualTo(0.0));
    }
    
}