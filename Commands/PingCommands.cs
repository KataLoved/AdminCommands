using ProjectM.Network;
using VampireCommandFramework;

namespace AdminCommands.Commands;
internal static class PingCommands
{ 
	[Command("ping", shortHand:"p", description:"Shows your latency.")]
	public static void PingCommand(ChatCommandContext ctx, string mode = "")
	{
		var ping = (int)(ctx.Event.SenderCharacterEntity.Read<Latency>().Value * 1000);
		ctx.Reply($"Your latency is <color=#ffff00>{ping}</color>ms");
	}
}
