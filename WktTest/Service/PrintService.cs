using System.Drawing.Printing;
using System.Management;

namespace Etiquetage.Service
{
    public static class PrintService
    {

        private static readonly string[] virtualPrinters = new[]
             {
              "microsoft print to pdf",
              "onenote for windows 10",
              "fax"
             };
        public static bool IsPrinterOnline(string printerName, out string error)
        {
            error = "";
            try
            {
                var searcher = new ManagementObjectSearcher(
                    $"SELECT * FROM Win32_Printer WHERE Name = '{printerName.Replace("\\", "\\\\")}'");

                foreach (var printer in searcher.Get())
                {
                    bool workOffline = (bool)printer["WorkOffline"];
                    ushort status = Convert.ToUInt16(printer["PrinterStatus"]);
                    return !workOffline && status == 3;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return false;
        }

        public static string GetDefaultPrinter()
        {
            return new PrinterSettings().PrinterName;
        }

        /*public static string? GetPrinterFromJson(string jsonPath)
        {
            if (File.Exists(jsonPath))
            {
                var json = File.ReadAllText(jsonPath);
                var settings = JsonSerializer.Deserialize<PrinterSettingsModel>(json);
                return settings?.SelectedPrinter;
            }
            return null;
        }*/
       

        public static bool IsVirtualPrinter(string printerName)
        {
            string lowerName = printerName.ToLowerInvariant();
            return virtualPrinters.Any(p => lowerName.Contains(p));
        }

        public static bool IsPrinterReady(string printerName, out string statusMessage)
        {
            statusMessage = "";

           
            if (IsVirtualPrinter(printerName))
            {
                statusMessage = $" لا يمكن استخدام \"{printerName}\" لأنها طابعة افتراضية .";
                return false;
            }

            try
            {
                var searcher = new ManagementObjectSearcher(
                    $"select * from Win32_Printer where Name = '{printerName.Replace("\\", "\\\\")}'");

                var printers = searcher.Get();

                if (printers.Count == 0)
                {
                    statusMessage = $" الطابعة \"{printerName}\" غير موجودة على هذا الجهاز.";
                    return false;
                }

                foreach (ManagementObject printer in printers)
                {
                    bool workOffline = (bool)(printer["WorkOffline"] ?? false);
                    ushort printerStatus = Convert.ToUInt16(printer["PrinterStatus"] ?? 0);
                    ushort detectedErrorState = Convert.ToUInt16(printer["DetectedErrorState"] ?? 0);

                    if (workOffline)
                    {
                        statusMessage = " الطابعة غير متصلة.";
                        return false;
                    }

                    if (printerStatus != 3)
                    {
                        statusMessage = $" الطابعة غير جاهزة ";
                        return false;
                    }

                    if (detectedErrorState != 0)
                    {
                        statusMessage = $" الطابعة بها خطأ (DetectedErrorState ";
                        return false;
                    }

                    statusMessage = " الطابعة جاهزة للطباعة.";
                    return true;
                }

                statusMessage = " لم يتم التحقق من حالة الطابعة.";
            }
            catch (Exception ex)
            {
                statusMessage = $" خطأ أثناء فحص الطابعة: {ex.Message}";
            }

            return false;
        }





    }
}
