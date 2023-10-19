using AdminCommands.Commands.Converters;
using VampireCommandFramework;

namespace AdminCommands.Commands;

public class KillCommands
{
	[Command("down", adminOnly: true)]
	public void DownCommand(ChatCommandContext ctx, FoundPlayer player = null)
	{
		var User = player?.Value.User ?? ctx.Event.SenderUserEntity;
		var Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;

		Helper.BuffPlayer(Character, User, Data.Buff.Buff_General_Vampire_Wounded_Buff);

		ctx.Reply($"Downed {player?.Value.Name ?? "you"}.");
	}
}
