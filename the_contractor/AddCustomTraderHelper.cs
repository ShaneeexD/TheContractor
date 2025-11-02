using System;
using System.Collections.Generic;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils.Cloners;
using SPTarkov.Server.Core.Utils.Json;

namespace TheContractor
{
	[Injectable(TypePriority = 400002)]
	public class AddCustomTraderHelper
	{
		private readonly ISptLogger<AddCustomTraderHelper> _logger;
		private readonly ICloner _cloner;
		private readonly DatabaseService _databaseService;
		private readonly LocaleService _localeService;

		public AddCustomTraderHelper(
			ISptLogger<AddCustomTraderHelper> logger,
			ICloner cloner,
			DatabaseService databaseService,
			LocaleService localeService)
		{
			_logger = logger;
			_cloner = cloner;
			_databaseService = databaseService;
			_localeService = localeService;
		}

		public void SetTraderUpdateTime(TraderConfig traderConfig, TraderBase baseJson, int refreshTimeSecondsMin, int refreshTimeSecondsMax)
		{
			UpdateTime item = new UpdateTime
			{
				TraderId = baseJson.Id,
				Seconds = new MinMax<int>(refreshTimeSecondsMin, refreshTimeSecondsMax)
			};
			traderConfig.UpdateTime.Add(item);
		}

		public void AddTraderWithEmptyAssortToDb(TraderBase traderDetailsToAdd)
		{
			TraderAssort assort = new TraderAssort
			{
				Items = new List<Item>(),
				BarterScheme = new Dictionary<MongoId, List<List<BarterScheme>>>(),
				LoyalLevelItems = new Dictionary<MongoId, int>()
			};
			Trader value = new Trader
			{
				Assort = assort,
				Base = _cloner.Clone<TraderBase>(traderDetailsToAdd),
				QuestAssort = new Dictionary<string, Dictionary<MongoId, MongoId>>
				{
					{
						"Started",
						new Dictionary<MongoId, MongoId>()
					},
					{
						"Success",
						new Dictionary<MongoId, MongoId>()
					},
					{
						"Fail",
						new Dictionary<MongoId, MongoId>()
					}
				},
				Dialogue = new Dictionary<string, List<string>>()
			};
			_databaseService.GetTables().Traders.TryAdd(traderDetailsToAdd.Id, value);
		}

		public void AddTraderToLocales(TraderBase baseJson, string firstName, string description)
		{
			Dictionary<string, LazyLoad<Dictionary<string, string>>> global = _databaseService.GetTables().Locales.Global;
			MongoId newTraderId = baseJson.Id;
			string fullName = baseJson.Name;
			string nickName = baseJson.Nickname;
			string location = baseJson.Location;
			foreach (var kvp in global)
			{
				var lazyLoad = kvp.Value;
				lazyLoad.AddTransformer(locale =>
				{
					locale[$"{newTraderId} FullName"] = fullName;
					locale[$"{newTraderId} FirstName"] = firstName;
					locale[$"{newTraderId} Nickname"] = nickName;
					locale[$"{newTraderId} Location"] = location;
					locale[$"{newTraderId} Description"] = description;
					return locale;
				});
			}
		}

		public void OverwriteTraderAssort(string traderId, TraderAssort newAssorts)
		{
			Trader trader;
			if (!_databaseService.GetTables().Traders.TryGetValue(traderId, out trader))
			{
				_logger.Warning("Unable to update assorts for trader: " + traderId + ", they couldn't be found on the server", null);
				return;
			}
			trader.Assort = newAssorts;
		}
	}
}
