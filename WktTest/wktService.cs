using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;
using Wkhtmltopdf.NetCore.Options;
using Wkhtmltopdf.NetCore.Options;
using Orientation = Wkhtmltopdf.NetCore.Options.Orientation;
using Size = Wkhtmltopdf.NetCore.Options.Size;
namespace WktTest
{
	public class wktService(IGeneratePdf generatePdf)
	{
		public async Task<byte[]> generatePdfPage()
		{
			var data = new DataTest
			{
				index = 1,
				data = "Test A"

			};

			var result = await generatePdf.GetByteArray("Views/razor.cshtml", data);
			return result;


		}

	}
	public class DataTest
	{
		public int index { get; set; }
		public string data { get; set; }

	}

}
