using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amica.vNext;
using Amica.vNext.Business;
using Amica.vNext.Models;
using Amica.vNext.Models.Documents;
using NUnit.Framework;

namespace Business.Tests
{
	[TestFixture]
    public class DocumentsTest
    {
		private readonly SqliteObjectCache cache = new SqliteObjectCache { ApplicationName = "test"};

		[SetUp]
		public void Setup()
        {
            DefaultFactories.Bootstrap();
        }

		[Test]
		public async Task GetNextNumberNoCache()
        {
			// a just created document will have its DocumentNumber.Numeric property set to 1
            var doc = Factory<Document>.Create();
            doc.Number = await Documents.GetNextNumber(doc);
            Assert.That(doc.Number.Numeric, Is.EqualTo(1));

			// 1 again as there's no cache storing previous value
            doc = Factory<Document>.Create();
            doc.Number = await Documents.GetNextNumber(doc);
            Assert.That(doc.Number.Numeric, Is.EqualTo(1));

			// since there's no cache available, if document has a number already, it will simply be increased by 1
            doc.Number.Numeric = 10;
            doc.Number = await Documents.GetNextNumber(doc);
            Assert.That(doc.Number.Numeric, Is.EqualTo(11));

			// same behavior as above if there's a String part, as we cannot retrieve the previous value from the cache
            doc.Number.String = "A";
            doc.Number = await Documents.GetNextNumber(doc);
            Assert.That(doc.Number.Numeric, Is.EqualTo(12));
            Assert.That(doc.Number.String, Is.EqualTo("A"));
        }
		[Test]
		public async Task GetNextNumberCached()
        {
            BusinessLogic.Cache = cache;

			// a just created document will have its DocumentNumber.Numeric property set to 1
            var doc = Factory<Document>.Create();
            doc.Number = await Documents.GetNextNumber(doc);
            Assert.That(doc.Number.Numeric, Is.EqualTo(1));

			// 2 thist ime as there's a cache storing previous value
            doc = Factory<Document>.Create();
            doc.Number = await Documents.GetNextNumber(doc);
            Assert.That(doc.Number.Numeric, Is.EqualTo(2));

			//// since there's no cache available, if document has a number already, it will simply be increased by 1
   //         doc.Number.Numeric = 10;
   //         doc.Number = await Documents.GetNextNumber(doc);
   //         Assert.That(doc.Number.Numeric, Is.EqualTo(11));

			//// same behavior as above if there's a String part, as we cannot retrieve the previous value from the cache
   //         doc.Number.String = "A";
   //         doc.Number = await Documents.GetNextNumber(doc);
   //         Assert.That(doc.Number.Numeric, Is.EqualTo(12));
   //         Assert.That(doc.Number.String, Is.EqualTo("A"));
        }
    }
}
