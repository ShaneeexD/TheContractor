using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Quests;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Services.Mod;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Models.Spt.Mod;

namespace TheContractor
{
    /// <summary>
    /// Loads quest JSON and locales from this mod's data folder and registers them with the server.
    /// Place quest definitions in data/quests/*.json as Dictionary<string, Quest> where key is quest id.
    /// Place locales in data/locales/*.json as a flat key->value map. Keys should contain the quest id to be picked up.
    /// </summary>
    [Injectable(TypePriority = 400006)]
    public class ContractorQuests : IOnLoad
    {
        private readonly ModHelper _modHelper;
        private readonly CustomQuestService _customQuestService;
        private readonly DatabaseServer _databaseServer;

        public ContractorQuests(ModHelper modHelper, CustomQuestService customQuestService, DatabaseServer databaseServer)
        {
            _modHelper = modHelper;
            _customQuestService = customQuestService;
            _databaseServer = databaseServer;
        }

        public Task OnLoad()
        {
            var modPath = _modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            var questsDir = System.IO.Path.Combine(modPath, "data", "quests");
            if (!System.IO.Directory.Exists(questsDir))
            {
                return Task.CompletedTask;
            }

            var loadedQuestIds = new List<string>();
            foreach (var questFile in System.IO.Directory.GetFiles(questsDir, "*.json", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var relative = System.IO.Path.GetRelativePath(modPath, questFile);
                    var questDict = _modHelper.GetJsonDataFromFile<Dictionary<string, Quest>>(modPath, relative);
                    if (questDict == null)
                        continue;

                    foreach (var kvp in questDict)
                    {
                        var quest = kvp.Value;
                        if (quest == null)
                            continue;

                        if (string.IsNullOrWhiteSpace(quest.Id))
                        {
                            quest.Id = kvp.Key;
                        }
                        if (string.IsNullOrWhiteSpace(quest.TemplateId))
                        {
                            quest.TemplateId = quest.Id;
                        }

                        var locales = LoadQuestLocales(modPath, quest.Id);
                        var details = new NewQuestDetails
                        {
                            NewQuest = quest,
                            Locales = locales
                        };
                        _customQuestService.CreateQuest(details);
                        loadedQuestIds.Add(quest.Id);
                    }
                }
                catch
                {
                    // swallow to avoid startup crash on malformed quest files
                }
            }

            // Simple daily rotation: lock all loaded quests behind Level 99, then unlock one at Level 1
            try
            {
                var tables = _databaseServer.GetTables();
                var quests = tables.Templates.Quests;
                foreach (var qid in loadedQuestIds)
                {
                    if (quests.TryGetValue(qid, out var q))
                    {
                        if (q.Conditions.AvailableForStart == null)
                        {
                            q.Conditions.AvailableForStart = new List<SPTarkov.Server.Core.Models.Eft.Common.Tables.QuestCondition>();
                        }
                        var levelCond = q.Conditions.AvailableForStart.FirstOrDefault(c => c.ConditionType == "Level");
                        if (levelCond == null)
                        {
                            levelCond = new SPTarkov.Server.Core.Models.Eft.Common.Tables.QuestCondition { Id = new MongoId(), ConditionType = "Level", CompareMethod = ">=", Value = 99, DynamicLocale = true };
                            q.Conditions.AvailableForStart.Add(levelCond);
                        }
                        else
                        {
                            levelCond.Value = 99;
                        }
                    }
                }

                if (loadedQuestIds.Count > 0)
                {
                    var rng = new Random();
                    var pick = loadedQuestIds[rng.Next(loadedQuestIds.Count)];
                    if (quests.TryGetValue(pick, out var qpick))
                    {
                        var levelCond = qpick.Conditions.AvailableForStart?.FirstOrDefault(c => c.ConditionType == "Level");
                        if (levelCond != null)
                        {
                            levelCond.Value = 1;
                        }
                    }
                }
            }
            catch
            {
                // ignore rotation errors
            }

            return Task.CompletedTask;
        }

        private Dictionary<string, Dictionary<string, string>> LoadQuestLocales(string modPath, MongoId questId)
        {
            var locales = new Dictionary<string, Dictionary<string, string>>();
            var localesPath = System.IO.Path.Combine(modPath, "data", "locales");
            if (!System.IO.Directory.Exists(localesPath))
                return locales;

            foreach (var localeFile in System.IO.Directory.GetFiles(localesPath, "*.json", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var languageCode = System.IO.Path.GetFileNameWithoutExtension(localeFile);
                    var relative = System.IO.Path.GetRelativePath(modPath, localeFile);
                    var localeData = _modHelper.GetJsonDataFromFile<Dictionary<string, string>>(modPath, relative) ?? new Dictionary<string, string>();
                    var questLocales = new Dictionary<string, string>();
                    foreach (var kvp in localeData)
                    {
                        if (kvp.Key != null && kvp.Key.Contains(questId))
                        {
                            questLocales[kvp.Key] = kvp.Value;
                        }
                    }
                    if (questLocales.Count > 0)
                    {
                        locales[languageCode] = questLocales;
                    }
                }
                catch
                {
                    // ignore malformed locale file
                }
            }

            return locales;
        }
    }
}
