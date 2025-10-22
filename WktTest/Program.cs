using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Wkhtmltopdf.NetCore;

namespace WktTest
{
	internal static class Program
	{
		public static IServiceProvider ServiceProvider;

		[STAThread]
		static void Main()
		{
			ApplicationConfiguration.Initialize();

			var host = Host.CreateDefaultBuilder()
				.ConfigureServices((context, services) =>
				{
					services.AddSingleton<IWebHostEnvironment>(new FakeWebHostEnvironment
					{
						EnvironmentName = Environments.Development,
						ApplicationName = "WktTest",
						WebRootPath = AppContext.BaseDirectory,
						ContentRootPath = AppContext.BaseDirectory
					});

					var diagnosticListener = new DiagnosticListener("Microsoft.AspNetCore");
					services.AddSingleton<DiagnosticListener>(diagnosticListener);
					services.AddSingleton<DiagnosticSource>(diagnosticListener);

					services.AddLogging();
					services.AddHttpContextAccessor();

					services.AddControllersWithViews()
							.AddRazorRuntimeCompilation();

					services.AddWkhtmltopdf("wkhtmltopdf");
					services.AddScoped<wktService>();

					services.AddTransient<Form1>();
				})
				.Build();

			ServiceProvider = host.Services;

			var form = ServiceProvider.GetRequiredService<Form1>();
			Application.Run(form);
		}
	}

	public class FakeWebHostEnvironment : IWebHostEnvironment //hh
	{
		public string ApplicationName { get; set; } = string.Empty;
		public IFileProvider WebRootFileProvider { get; set; } = new PhysicalFileProvider(AppContext.BaseDirectory);
		public string WebRootPath { get; set; } = AppContext.BaseDirectory;
		public string EnvironmentName { get; set; } = Environments.Development;
		public IFileProvider ContentRootFileProvider { get; set; } = new PhysicalFileProvider(AppContext.BaseDirectory);
		public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
	}
}
