using RSSAnalyzing.DTOs;
using System.Collections.Generic;

namespace RSSAnalyzing
{
	public interface ICompanyInactiveCalcAlgo
	{
		/// <summary>
		///		Returns all companies which have no events on RSS feed for <paramref name="inactiveDaysCount"/>
		/// </summary>
		/// <param name="companiesFeeds"> set of companies and appropriated feeds </param>
		/// <param name="inactiveDaysCount"> count of days for making check </param>
		/// <returns> array of companies </returns>
		IEnumerable<string> GetInactiveCompanies(IEnumerable<CompanyFeedDTO> companiesFeeds, int inactiveDaysCount);
	}
}
