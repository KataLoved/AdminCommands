using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bloodstone.API;
using AdminCommands.Commands.Converters;
using ProjectM;
using ProjectM.Network;
using VampireCommandFramework;

namespace AdminCommands.Commands;
internal class AdminCommands
{
	[Command("unlock", description: "Unlocks all vblood/research/journal quests", adminOnly: true)]
	public void UnlockCommand(ChatCommandContext ctx, FoundPlayer player = null)
	{
		var User = player?.Value.User ?? ctx.Event.SenderUserEntity;
		var Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;

		try
		{
			var debugEventsSystem = VWorld.Server.GetExistingSystem<DebugEventsSystem>();
			var fromCharacter = new FromCharacter()
			{
				User = User,
				Character = Character
			};
			
			Helper.Unlock(fromCharacter);
			ctx.Reply($"Unlocked everything for {player?.Value.Name ?? "you"}.");
		}
		catch (Exception e)
		{ 
			throw ctx.Error(e.ToString());
		}	
	}

	[Command("toggleadmin", description: "Toggles admin for a player", adminOnly: true)]
	public void AdminCommand(ChatCommandContext ctx, FoundPlayer player = null)
	{
		var User = player?.Value.User ?? ctx.Event.SenderUserEntity;
		var Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;

		if (Helper.ToggleAdmin(Character, User))
		{
			ctx.Reply("Admin authed");
			if (player?.Value.Character != null)
			{
				ServerChatUtils.SendSystemMessageToClient(VWorld.Server.EntityManager, User.Read<User>(), "You were admin auth'd");
			}
		}
		else
		{
			ctx.Reply("Admin deauthed");
			if (player?.Value.Character != null)
			{
				ServerChatUtils.SendSystemMessageToClient(VWorld.Server.EntityManager, User.Read<User>(), "You were admin deauth'd");
			}
		}
	}
}
