using AdminCommands.Data;
using AdminCommands.Models;
using ProjectM;
using UnityEngine;
using VampireCommandFramework;

namespace AdminCommands.Commands;

internal static class BloodCommands
{
	[Command("bloodpotion", "bp", usage: "{Blood Name} [quantity=1] [quality=100]", description: "Creates a Potion with specified Blood Type, Quality, and Quantity", adminOnly: true)]
	public static void GiveBloodPotionCommand(ChatCommandContext ctx, BloodType type = BloodType.Frailed, int quantity = 1, float quality = 100f)
	{
		quality = Mathf.Clamp(quality, 0, 100);
		int i;
		for (i = 0; i < quantity; i++)
		{
			if (Helper.AddItemToInventory(ctx.Event.SenderCharacterEntity, Prefabs.Item_Consumable_PrisonPotion_Bloodwine, 1, out var bloodPotionEntity))
			{
				var blood = new StoredBlood()
				{
					BloodQuality = quality,
					BloodType = new PrefabGUID((int)type)
				};
				
				bloodPotionEntity.Write(blood);
			}
			else
			{
				break;
			}
		}		

		ctx.Reply($"Got {i} Blood Potion(s) Type <color=#ff0>{type}</color> with <color=#ff0>{quality}</color>% quality");
	}
}
