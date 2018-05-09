using System;
using System.Collections.Generic;
using System.Linq;
using RSSAnalyzing.DTOs;

namespace RSSAnalyzing.Algorithms
{
	public class CompanyInactiveCalculator : ICompanyInactiveCalcAlgo
	{
		/// <inheritdoc />
		public IEnumerable<string> GetInactiveCompanies(IEnumerable<CompanyFeedDTO> companiesFeeds, int inactiveDaysCount)
		{
			if (inactiveDaysCount < 0)
				throw new ArgumentOutOfRangeException("inactiveDaysCount", "Number of inactive days must be more or equal than 0");

			var utcComparingDate = DateTime.UtcNow.AddDays(0 - inactiveDaysCount);
			foreach (var item in companiesFeeds)
			{
				var eventExisted = item.Feed.Any(x => x.Published.UtcDateTime >= utcComparingDate);
				if (!eventExisted)
					yield return item.Company;
			}
		}
	}
}
