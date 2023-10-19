using AdminCommands.Commands.Converters;
using VampireCommandFramework;

namespace AdminCommands.Commands;
internal static class RepairCommands
{
	[Command("repair", description: "Repairs all gear.")]
	public static void RepairOtherCommand(ChatCommandContext ctx, FoundPlayer player)
	{
		var Character = player.Value.Character;
		Helper.RepairGear(Character);
		ctx.Reply($"Repaired.");
	}

	[Command("repair", description: "Repairs all gear.")]
	public static void RepairMeCommand(ChatCommandContext ctx)
	{
		Helper.RepairGear(ctx.Event.SenderCharacterEntity);
		ctx.Reply($"Repaired.");
	}

	[Command("breakgear", description: "Breaks all gear.")]
	public static void BreakTheirGearCommand(ChatCommandContext ctx, FoundPlayer player)
	{
		var Character = player.Value.Character;
		Helper.RepairGear(Character, false);
		ctx.Reply($"Repaired.");
	}

	[Command("breakgear", description: "Breaks all gear.")]
	public static void BreakMyGearCommand(ChatCommandContext ctx)
	{
		Helper.RepairGear(ctx.Event.SenderCharacterEntity, false);
		ctx.Reply($"Repaired.");
	}
}
