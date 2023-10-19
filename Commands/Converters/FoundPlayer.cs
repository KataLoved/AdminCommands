using VampireCommandFramework;
using static AdminCommands.PlayerService;

namespace AdminCommands.Commands.Converters;

public record FoundPlayer(Player Value);

public record OnlinePlayer(Player Value);

internal class FoundPlayerConverter : CommandArgumentConverter<FoundPlayer>
{
	public override FoundPlayer Parse(ICommandContext ctx, string input)
	{
		var player = HandleFindPlayerData(ctx, input, requireOnline: false);
		return new FoundPlayer(player);
	}

	public static Player HandleFindPlayerData(ICommandContext ctx, string input, bool requireOnline)
	{
		if (TryGetPlayerFromString(input, out Player player))
		{
			if (!requireOnline || player.IsOnline)
			{
				return player;
			}
		}
		throw ctx.Error($"Player {input} not found.");
	}
}
internal class OnlinePlayerConverter : CommandArgumentConverter<OnlinePlayer>
{
	public override OnlinePlayer Parse(ICommandContext ctx, string input)
	{
		var player = FoundPlayerConverter.HandleFindPlayerData(ctx, input, requireOnline: false);
		return new OnlinePlayer(player);
	}
}
