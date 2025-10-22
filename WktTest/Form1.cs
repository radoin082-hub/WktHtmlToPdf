using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

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

				using (SaveFileDialog saveDialog = new SaveFileDialog())
				{
					saveDialog.Title = "اختر مكان حفظ ملف PDF";
					saveDialog.Filter = "ملف PDF (*.pdf)|*.pdf";
					saveDialog.FileName = "Report.pdf"; 

					if (saveDialog.ShowDialog() == DialogResult.OK)
					{
						string filePath = saveDialog.FileName;

						await File.WriteAllBytesAsync(filePath, pdfBytes);

						System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
						{
							FileName = filePath,
							UseShellExecute = true
						});

					}
				}
			}
			catch (Exception ex)
			{
			}
		}
	}
}
