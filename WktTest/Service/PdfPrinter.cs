using System.Drawing.Printing;
using PdfiumViewer;

namespace Etiquetage.Service
{
    public static class PdfPrinter
    {
        public static void PrintPdf(string filePath, string printerName)
        {
            using var document = PdfDocument.Load(filePath);
            using var printDoc = document.CreatePrintDocument();

            printDoc.PrinterSettings = new PrinterSettings
            {
                PrinterName = printerName
            };
            printDoc.PrintController = new StandardPrintController(); 
            printDoc.Print();
        }
    }
}
