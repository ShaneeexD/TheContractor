using System;
using System.Collections.Generic;

namespace TheContractor
{
	/// <summary>
	/// Handles quest reward generation and distribution
	/// </summary>
	public class QuestRewards
	{
		private readonly Random _random;

		public QuestRewards()
		{
			_random = new Random();
		}

		/// <summary>
		/// Generate rewards based on quest difficulty
		/// </summary>
		public Dictionary<string, object> GenerateRewards(QuestTemplates.QuestDifficulty difficulty)
		{
			var rewards = new Dictionary<string, object>();

			// Base reward amounts based on difficulty
			int baseMoneyReward = difficulty switch
			{
				QuestTemplates.QuestDifficulty.Easy => _random.Next(50000, 100000),
				QuestTemplates.QuestDifficulty.Medium => _random.Next(100000, 200000),
				QuestTemplates.QuestDifficulty.Hard => _random.Next(200000, 400000),
				QuestTemplates.QuestDifficulty.Extreme => _random.Next(400000, 750000),
				_ => 50000
			};

			int baseExperienceReward = difficulty switch
			{
				QuestTemplates.QuestDifficulty.Easy => _random.Next(5000, 10000),
				QuestTemplates.QuestDifficulty.Medium => _random.Next(10000, 20000),
				QuestTemplates.QuestDifficulty.Hard => _random.Next(20000, 40000),
				QuestTemplates.QuestDifficulty.Extreme => _random.Next(40000, 75000),
				_ => 5000
			};

			double reputationReward = difficulty switch
			{
				QuestTemplates.QuestDifficulty.Easy => 0.01,
				QuestTemplates.QuestDifficulty.Medium => 0.02,
				QuestTemplates.QuestDifficulty.Hard => 0.03,
				QuestTemplates.QuestDifficulty.Extreme => 0.05,
				_ => 0.01
			};

			rewards["money"] = baseMoneyReward;
			rewards["experience"] = baseExperienceReward;
			rewards["reputation"] = reputationReward;

			// TODO: Add item rewards based on difficulty
			// rewards["items"] = GenerateItemRewards(difficulty);

			return rewards;
		}

		/// <summary>
		/// Generate item rewards (to be implemented)
		/// </summary>
		private List<string> GenerateItemRewards(QuestTemplates.QuestDifficulty difficulty)
		{
			// TODO: Implement item reward generation
			// - Select random items from database
			// - Scale rarity based on difficulty
			return new List<string>();
		}
	}
}
