using RSSAnalyzing.Algorithms;
using RSSAnalyzing.Formatters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RSSAnalyzing
{
	public class Analyzer
	{
		private ICompanyInactiveCalcAlgo _companyInactiveCalcAlgorithm;

		public Analyzer()
		{
			Initialize();
		}

		private void Initialize()
		{
			_companyInactiveCalcAlgorithm = new CompanyInactiveCalculator();
		}

		/// <summary>
		///		Gets list of companies which were inactive for <paramref name="inactiveDaysCount"/>
		///		based on <paramref name="companiesRssUris"/>
		/// </summary>
		/// <param name="companiesRssUris"> dictionary of CompanyName and RSS URI </param>
		/// <param name="inactiveDaysCount"> days count during on which we need to check companies for activity </param>
		/// <returns> list of inactive companies </returns>
		public async Task<IEnumerable<string>> GetInactiveCompanies(Dictionary<string, Uri> companiesRssUris, int inactiveDaysCount)
		{
			var companiesFeeds = await RawToDTOFormatter.Format(companiesRssUris);
			return _companyInactiveCalcAlgorithm.GetInactiveCompanies(companiesFeeds, inactiveDaysCount);
		}

		/// <summary>
		///		Gets list of companies which were inactive for <paramref name="inactiveDaysCount"/>
		///		based on <paramref name="companiesRssUris"/>
		/// </summary>
		/// <param name="companiesRssUris"> dictionary of CompanyName and RSS XML </param>
		/// <param name="inactiveDaysCount"> days count during on which we need to check companies for activity </param>
		/// <returns> list of inactive companies </returns>
		public async Task<IEnumerable<string>> GetInactiveCompanies(Dictionary<string, string> companiesRssXmls, int inactiveDaysCount)
		{
			var companiesFeeds = await RawToDTOFormatter.Format(companiesRssXmls);
			return _companyInactiveCalcAlgorithm.GetInactiveCompanies(companiesFeeds, inactiveDaysCount);
		}
	}
}
