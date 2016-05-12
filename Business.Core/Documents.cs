using System;
using System.Collections.Generic;
using System.Text;
using Amica.vNext.Models.Documents;
using Amica.vNext;
using System.Threading.Tasks;

namespace Amica.vNext.Business
{
    public class Documents
    {
		public static async Task<DocumentNumber> GetNextNumber(Document document)
        {
            var number = (BusinessLogic.Cache != null && document.Category != null && document.Number != null) ?
                await BusinessLogic.Cache?.Get<DocumentNumber>(DocumentNumberCacheKey(document)) :
                (document.Number != null) ?
                    document.Number :
                    new DocumentNumber();
            number.Numeric += 1;

			if (BusinessLogic.Cache != null)
				await BusinessLogic.Cache.Insert(DocumentNumberCacheKey(document), number);
            return number;
        }
		private static string DocumentNumberCacheKey(Document document)
        {
            return $"{(int)document.Category.Code}/{document.Number.String.ToString()}";
        }

    }
}
