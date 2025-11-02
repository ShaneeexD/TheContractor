using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Routers;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Utils;

namespace TheContractor
{
    [Injectable(TypePriority = 400002)]
    public class AddTraderWithAssortJson : IOnLoad
    {
        private readonly ModHelper _modHelper;
        private readonly ImageRouter _imageRouter;
        private readonly TimeUtil _timeUtil;
        private readonly AddCustomTraderHelper _addCustomTraderHelper;
        private readonly ILogger<AddTraderWithAssortJson> _logger;
        private readonly TraderConfig _traderConfig;
        private readonly RagfairConfig _ragfairConfig;

        public AddTraderWithAssortJson(
            ModHelper modHelper,
            ImageRouter imageRouter,
            ConfigServer configServer,
            TimeUtil timeUtil,
            AddCustomTraderHelper addCustomTraderHelper,
            ILogger<AddTraderWithAssortJson> logger)
        {
            _modHelper = modHelper;
            _imageRouter = imageRouter;
            _timeUtil = timeUtil;
            _addCustomTraderHelper = addCustomTraderHelper;
            _logger = logger;
            _traderConfig = configServer.GetConfig<TraderConfig>();
            _ragfairConfig = configServer.GetConfig<RagfairConfig>();
        }

        public Task OnLoad()
        {
            string modFolder = _modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            string avatarPath = System.IO.Path.Combine(modFolder, "data/TheContractor.png");
            TraderBase baseJson = _modHelper.GetJsonDataFromFile<TraderBase>(modFolder, "data/base.json");
            _imageRouter.AddRoute(baseJson.Avatar.Replace(".png", string.Empty), avatarPath);
            _addCustomTraderHelper.SetTraderUpdateTime(_traderConfig, baseJson, _timeUtil.GetHoursAsSeconds(1), _timeUtil.GetHoursAsSeconds(2));
            _ragfairConfig.Traders.TryAdd(baseJson.Id, true);
            _addCustomTraderHelper.AddTraderWithEmptyAssortToDb(baseJson);
            _addCustomTraderHelper.AddTraderToLocales(baseJson, "The Contractor", "A mysterious figure who specializes in giving out contracts and special assignments. He rewards those who complete his daily and weekly tasks.");
            TraderAssort assort = _modHelper.GetJsonDataFromFile<TraderAssort>(modFolder, "data/assort.json");
            _addCustomTraderHelper.OverwriteTraderAssort(baseJson.Id, assort);
            _logger.LogInformation("[THE CONTRACTOR has arrived in Tarkov]");
            return Task.CompletedTask;
        }
    }
}
