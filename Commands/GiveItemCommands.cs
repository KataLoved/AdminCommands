using ProjectM;
using VampireCommandFramework;
using Bloodstone.API;

namespace AdminCommands.Commands;
internal class GiveItemCommands
{
	public record struct GivenItem(PrefabGUID Value);

	internal class GiveItemConverter : CommandArgumentConverter<GivenItem>
	{
		public override GivenItem Parse(ICommandContext ctx, string input)
		{
			if (Helper.TryGetItemPrefabGUIDFromString(input, out PrefabGUID prefab))
			{
				return new GivenItem(prefab);
			}

			throw ctx.Error($"Could not find item: {input}");
		}
	}

	[Command("give", "g", "<PrefabGUID or name> [quantity=1]", "Gives the specified item to the player", adminOnly: true)]
	public static void GiveItem(ChatCommandContext ctx, GivenItem item, int quantity = 1)
	{
		if (Helper.AddItemToInventory(ctx.Event.SenderCharacterEntity, item.Value, quantity, out var entity))
		{
			var prefabSys = VWorld.Server.GetExistingSystem<PrefabCollectionSystem>();
			prefabSys.PrefabGuidToNameDictionary.TryGetValue(item.Value, out var name); // seems excessive
			ctx.Reply($"Gave {quantity} {name}");
		}
	}
}
