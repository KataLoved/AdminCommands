using Bloodstone.API;
using ProjectM;
using ProjectM.Network;
using Unity.Entities;
using VampireCommandFramework;

namespace AdminCommands.Commands;

internal static class MiscellaneousCommands
{
	[Command("control", description: "Takes control over hovered NPC", adminOnly: true)]
	public static void ControlCommand(ChatCommandContext ctx)
	{
		var User = ctx.Event.SenderUserEntity;
		var Character = ctx.Event.SenderCharacterEntity;
		FromCharacter fromData = new FromCharacter()
		{
			User = User,
			Character = Character
		};

		DebugEventsSystem des = VWorld.Server.GetExistingSystem<DebugEventsSystem>();
		var entityInput = Character.Read<EntityInput>();
		if (entityInput.HoveredEntity.Index > 0)
		{
			Entity newCharacter = User.Read<EntityInput>().HoveredEntity;
			if (!newCharacter.Has<PlayerCharacter>())
			{
				ControlDebugEvent controlDebugEvent = new ControlDebugEvent
				{
					EntityTarget = newCharacter,
					Target = User.Read<EntityInput>().HoveredEntityNetworkId
				};
				des.ControlUnit(fromData, controlDebugEvent);
				ctx.Reply($"Controlling hovered unit");
				return;
			}
		}
		if (PlayerService.TryGetCharacterFromName(User.Read<User>().CharacterName.ToString(), out Character))
		{
			ControlDebugEvent controlDebugEvent = new ControlDebugEvent
			{
				EntityTarget = Character,
				Target = Character.Read<NetworkId>()
			};
			des.ControlUnit(fromData, controlDebugEvent);
			ctx.Reply("Controlling self");
		}
		else
		{
			ctx.Reply("An error ocurred while trying to control your original body");
		}
	}
}
