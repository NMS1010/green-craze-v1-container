using green_craze_be_v1.Application.Intefaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace green_craze_be_v1.Infrastructure.Data.Context
{
	public class AppDBContextFactory : IDesignTimeDbContextFactory<AppDBContext>
	{
		private readonly ICurrentUserService _currentUserService;
		private readonly IDateTimeService _dateTimeService;

		public AppDBContextFactory()
		{
		}

		public AppDBContextFactory(ICurrentUserService currentUserService, IDateTimeService dateTimeService)
		{
			_currentUserService = currentUserService;
			_dateTimeService = dateTimeService;
		}

		public AppDBContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
				.Build();

			var connectionString = configuration.GetConnectionString("AppDBContext");

			var optionBuilder = new DbContextOptionsBuilder<AppDBContext>();
			optionBuilder.UseMySQL(connectionString);

			return new AppDBContext(optionBuilder.Options, _currentUserService, _dateTimeService);
		}
	}
}