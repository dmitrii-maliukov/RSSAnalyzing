using Microsoft.SyndicationFeed;
using System.Collections.Generic;

namespace RSSAnalyzing.DTOs
{
	public class CompanyFeedDTO
	{
		public string Company { get; private set; }
		public IEnumerable<ISyndicationItem> Feed { get; private set; }

		public CompanyFeedDTO(string company, IEnumerable<ISyndicationItem> feed)
		{
			Company = company;
			Feed = feed;
		}
	}
}
