using System;
using System.Collections.Generic;
 

namespace TheContractor
{
	/// <summary>
	/// Defines quest templates and types for The Contractor
	/// </summary>
	public static class QuestTemplates
	{
		/// <summary>
		/// Quest types available
		/// </summary>
		public enum QuestType
		{
			Elimination,    // Kill X enemies
			Collection,     // Collect X items
			Survival,       // Survive X raids
			Extraction,     // Extract from specific locations
			MarkLocation,   // Mark specific locations
			PlaceItem,      // Place items in specific locations
			FindInRaid      // Find items in raid
		}

		/// <summary>
		/// Quest difficulty levels
		/// </summary>
		public enum QuestDifficulty
		{
			Easy,
			Medium,
			Hard,
			Extreme
		}

		/// <summary>
		/// Reward types
		/// </summary>
		public enum RewardType
		{
			Money,
			Items,
			Experience,
			Reputation
		}

		/// <summary>
		/// Available maps for quest objectives
		/// </summary>
		public static readonly List<string> AvailableMaps = new List<string>
		{
			"Customs",
			"Woods",
			"Shoreline",
			"Interchange",
			"Reserve",
			"Lighthouse",
			"Streets of Tarkov",
			"Factory",
			"Labs"
		};

		/// <summary>
		/// Common enemy types for elimination quests
		/// </summary>
		public static readonly List<string> EnemyTypes = new List<string>
		{
			"Scavs",
			"PMCs",
			"Raiders",
			"Rogues",
			"Cultists",
			"Bosses"
		};

		/// <summary>
		/// Get a random quest type
		/// </summary>
		public static QuestType GetRandomQuestType(Random random)
		{
			var values = Enum.GetValues(typeof(QuestType));
			return (QuestType)values.GetValue(random.Next(values.Length));
		}

		/// <summary>
		/// Get a random map
		/// </summary>
		public static string GetRandomMap(Random random)
		{
			return AvailableMaps[random.Next(AvailableMaps.Count)];
		}

		/// <summary>
		/// Get a random enemy type
		/// </summary>
		public static string GetRandomEnemyType(Random random)
		{
			return EnemyTypes[random.Next(EnemyTypes.Count)];
		}
	}
}
