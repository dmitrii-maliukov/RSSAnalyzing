using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using RSSAnalyzing;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System.IO;

namespace RSSAnalyzing.Tests
{
    public class RawToDTOFormatterTests
    {
		#region CompaniesURIs input

		[Fact]
		public void CompaniesURIs_EmptyCompanyNameThrowsException()
		{
			// Arrange
			var inputValues = new Dictionary<string, Uri>()
			{
				{ "FirstCompany", new Uri("https://www.example1.com") },
				{ "SecondCompany", new Uri("https://www.example2.com") },
				{ "", new Uri("https://www.example3.com") }
			};

			// Act
			Func<Task> act = () => RawToDTOFormatter.Format(inputValues);

			// Assert
			Assert.ThrowsAsync<ArgumentException>(act);
		}

		[Fact]
		public void CompaniesURIs_EmptyURIThrowsException()
		{
			// Arrange 
			var inputValues = new Dictionary<string, Uri>()
			{
				{ "FirstCompany", new Uri("https://www.example1.com") },
				{ "SecondCompany", null }
			};

			// Act
			Func<Task> act = () => RawToDTOFormatter.Format(inputValues);

			// Assert
			Assert.ThrowsAsync<ArgumentException>(act);
		}

		[Fact]
		public void CompaniesURIs_NotExistingUri()
		{
			// Arrange
			var inputValues = new Dictionary<string, Uri>()
			{
				{ "FirstCompany", new Uri("https://www.dropbox.com/s/4rwsu8goeq7zx1h/bbc_world.xml?dl=1") },
				{ "SecondCompany", new Uri("http://example.com/rss.xml") },
			};

			// Act
			Func<Task> act = () => RawToDTOFormatter.Format(inputValues);

			// Assert
			Assert.ThrowsAsync<WebException>(act);
		}

		[Fact]
		public async Task CompaniesURIs_UsualHandling()
		{
			// Arrange
			var inputValues = new Dictionary<string, Uri>()
			{
				{ "FirstCompany", new Uri("https://www.dropbox.com/s/4rwsu8goeq7zx1h/bbc_world.xml?dl=1") },
				{ "SecondCompany", new Uri("https://www.dropbox.com/s/acypcml3d89v38i/bbc_world_addition.xml?dl=1") },
			};

			// Act
			var result = await RawToDTOFormatter.Format(inputValues);

			// Assert
			var firstCompanyFeedCount = result.First(x => x.Company == "FirstCompany").Feed.Count();
			var secondCompanyFeedCount = result.First(x => x.Company == "SecondCompany").Feed.Count();
			Assert.True(firstCompanyFeedCount == 4 && secondCompanyFeedCount == 11);
		}

		#endregion

		#region CompaniesXMLs_input
		[Fact]
		public void CompaniesXMLs_EmptyCompanyNameThrowsException()
		{
			// Arrange 
			var inputValues = new Dictionary<string, string>()
			{
				{ "FirstCompany", "xml" },
				{ "", "xml" }
			};

			// Act
			Func<Task> act = () => RawToDTOFormatter.Format(inputValues);

			// Assert
			Assert.ThrowsAsync<ArgumentException>(act);
		}

		[Fact]
		public void CompaniesXMLs_EmptyXMLThrowsException()
		{
			// Arrange 
			var inputValues = new Dictionary<string, string>()
			{
				{ "FirstCompany", "xml" },
				{ "SecondCompany", "" }
			};

			// Act
			Func<Task> act = () => RawToDTOFormatter.Format(inputValues);

			// Assert
			Assert.ThrowsAsync<ArgumentException>(act);
		}

		[Fact]
		public async Task CompaniesXMLs_UsualHandling()
		{
			// Arrange 
			var exampleXmlNotOlder2Days = File.ReadAllText("RssExample_NotOlder2Days.xml");
			var exampleXmlOlder2Days = File.ReadAllText("RssExample_Older2Days.xml");
			var inputValues = new Dictionary<string, string>()
			{
				{ "FirstCompany", exampleXmlNotOlder2Days },
				{ "SecondCompany", exampleXmlOlder2Days }
			};

			// Act
			var result = await RawToDTOFormatter.Format(inputValues);

			// Assert
			var itemsCountNotOlder = result.First(x => x.Company == "FirstCompany").Feed.Count();
			var itemsCountOlder = result.First(x => x.Company == "SecondCompany").Feed.Count();
			Assert.True(itemsCountNotOlder == 2 && itemsCountOlder == 2);
		}
		#endregion
	}
}
