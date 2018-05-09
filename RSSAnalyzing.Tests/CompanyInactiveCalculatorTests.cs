using System;
using System.Collections.Generic;
using Xunit;
using RSSAnalyzing.DTOs;
using Microsoft.SyndicationFeed;
using RSSAnalyzing.Algorithms;

namespace RSSAnalyzing.Tests
{
    public class CompanyInactiveCalculatorTests
    {
		private CompanyInactiveCalculator _calculator = new CompanyInactiveCalculator();

		private List<CompanyFeedDTO> ArrangeActiveCompaniesFor3Days()
		{
			var company1 = "ActiveCompany1";
			var feed1 = new List<SyndicationItem>()
			{
				new SyndicationItem() { Published = DateTime.UtcNow.AddHours(-23) },
				new SyndicationItem() { Published = DateTime.UtcNow.AddHours(-30) },
				new SyndicationItem() { Published = DateTime.UtcNow.AddHours(-36) }
			};
			CompanyFeedDTO activeCompany1 = new CompanyFeedDTO(company1, feed1);

			var company2 = "ActiveCompany2";
			var feed2 = new List<SyndicationItem>()
			{
				new SyndicationItem() { Published = DateTime.UtcNow.AddHours(-33) },
				new SyndicationItem() { Published = DateTime.UtcNow.AddHours(-50) },
				new SyndicationItem() { Published = DateTime.UtcNow.AddHours(-56) }
			};
			CompanyFeedDTO activeCompany2 = new CompanyFeedDTO(company2, feed2);

			return new List<CompanyFeedDTO>() { activeCompany1, activeCompany2 };
		}

		[Fact]
		public void OnlyActiveCompaniesFor3Days()
		{
			// Arrange
			var activeCompanies = ArrangeActiveCompaniesFor3Days();

			// Act
			var result = _calculator.GetInactiveCompanies(activeCompanies, 3);

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public void OneInactiveCompanyFor3Days()
		{
			// Arrange
			var companies = ArrangeActiveCompaniesFor3Days();

			var inactiveCompanyName = "InactiveCompany";
			var inactiveFeed = new List<SyndicationItem>()
			{
				new SyndicationItem() { Published = DateTime.UtcNow.AddHours(-74) },
				new SyndicationItem() { Published = DateTime.UtcNow.AddHours(-77) },
				new SyndicationItem() { Published = DateTime.UtcNow.AddHours(-80) }
			};
			var inactiveCompany = new CompanyFeedDTO(inactiveCompanyName, inactiveFeed);

			companies.Add(inactiveCompany);

			// Act
			var result = _calculator.GetInactiveCompanies(companies, 3);

			// Assert 
			Assert.Single(result);
		}
    }
}
