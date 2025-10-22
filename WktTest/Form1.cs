using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Etiquetage.Service; 

namespace WktTest
{
	public partial class Form1 : Form
	{
		private readonly wktService _wktService;

		public Form1(wktService wktService)
		{
			InitializeComponent();
			_wktService = wktService;
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			try
			{
				byte[] pdfBytes = await _wktService.generatePdfPage();

				string tempFile = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");
				await File.WriteAllBytesAsync(tempFile, pdfBytes);

				await Task.Run(() =>
				{
					PdfPrinter.PrintPdf(tempFile, "name printer");
				});

				try { File.Delete(tempFile); } catch { }

			}
			catch (Exception ex)
			{
			}
		}
	}
}
