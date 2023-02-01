using Microsoft.Extensions.DependencyInjection;

namespace BlazorLeaflet.Offline.Services
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddBlazorLeafletOffline(this IServiceCollection services)
			=> services.AddScoped<BlazorLeafletOfflineService>();
	}
}
