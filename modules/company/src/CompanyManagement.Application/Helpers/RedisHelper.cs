// OnlineSales.API.Caching.RedisHelper
using System;
using System.Configuration;
using CompanyManagement.Application.CompanyManagement;
using OrderManagement.Application;
using StackExchange.Redis;

public static class RedisHelper
{
	private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(
		() =>
		{
			try
			{
				return ConnectionMultiplexer.Connect(CompanyManagementApplicationModule.StaticConfig["RedisCache:ConnectionStringUser"]);
			}
			catch (Exception ex)
			{
				throw ex;
				
			}
		}


		);
	public static ConnectionMultiplexer Connection => lazyConnection.Value;
	public static IDatabase GetDatabase()
	{
		return Connection.GetDatabase();
	}


}
