using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using RSSAnalyzing.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace RSSAnalyzing
{
    public class RawToDTOFormatter
	{
		/// <summary>
		///		Builds list of CompanyFeedDTO records based <paramref name="companiesFeedsRaw"/>
		/// </summary>
		/// <param name="companiesFeedsRaw"> dictionary of CompanyName and RSS URI </param>
		/// <returns> list of CompanyFeedDTO records </returns>
		public static async Task<IEnumerable<CompanyFeedDTO>> Format(Dictionary<string, Uri> companiesFeedsRaw)
		{
			var result = new List<CompanyFeedDTO>();

			if (companiesFeedsRaw.Keys.Any(x => string.IsNullOrEmpty(x)))
				throw new ArgumentException("At least one Company name is empty");

			if (companiesFeedsRaw.Values.Any(x => x == null))
				throw new ArgumentException("At least one RSS URI is null");

			foreach (var rawItem in companiesFeedsRaw)
			{
				var company = rawItem.Key;
				var feedUri = rawItem.Value;

				using (var xmlReader = XmlReader.Create(feedUri.AbsoluteUri))
				{
					var feed = await GetRSSItems(xmlReader);
					result.Add(new CompanyFeedDTO(company, feed));
				}
			}

			return result;
		}

		/// <summary>
		///		Builds list of CompanyFeedDTO records based <paramref name="companiesFeedsRaw"/>
		/// </summary>
		/// <param name="companiesFeedsRaw"> dictionary of CompanyName and RSS XML as string </param>
		/// <returns> list of CompanyFeedDTO records </returns>
		public static async Task<IEnumerable<CompanyFeedDTO>> Format(Dictionary<string, string> companiesFeedsRaw)
		{
			var result = new List<CompanyFeedDTO>();

			if (companiesFeedsRaw.Keys.Any(x => string.IsNullOrEmpty(x)))
				throw new ArgumentException("At least one Company name is empty");

			if (companiesFeedsRaw.Values.Any(x => string.IsNullOrEmpty(x)))
				throw new ArgumentException("At least one feed XML is null");

			foreach (var rawItem in companiesFeedsRaw)
			{
				var company = rawItem.Key;
				var feedXml = rawItem.Value;

				using (var textReader = new StringReader(feedXml))
				{
					using (var xmlReader = XmlReader.Create(textReader))
					{
						var feed = await GetRSSItems(xmlReader);
						result.Add(new CompanyFeedDTO(company, feed));
					}
				}
			}

			return result;
		}

		/// <summary>
		///		Gets all RSS items from <paramref name="xmlReader"/>
		/// </summary>
		/// <param name="xmlReader"> xml feed reader </param>
		/// <returns> list of RSS items </returns>
		private static async Task<IEnumerable<ISyndicationItem>> GetRSSItems(XmlReader xmlReader)
		{
			var result = new List<ISyndicationItem>();

			var feedReader = new RssFeedReader(xmlReader);
			while (await feedReader.Read())
			{
				if (feedReader.ElementType == SyndicationElementType.Item)
					result.Add(await feedReader.ReadItem());
			}

			return result;
		}
	}
}
