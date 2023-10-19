
using AdminCommands.Commands.Converters;
using ProjectM;
using ProjectM.Network;
using VampireCommandFramework;
using Bloodstone.API;
using Il2CppSystem;
using Unity.Mathematics;

namespace AdminCommands.Commands;
internal class CastCommands
{
	[Command("cast", description: "Cast any ability", adminOnly: true)]
	public void CastCommand(ChatCommandContext ctx, FoundPrefabGuid prefabGuid, FoundPlayer player = null)
	{
		var User = player?.Value.User ?? ctx.Event.SenderUserEntity;
		var Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;
		FromCharacter fromData = new FromCharacter()
		{
			User = User,
			Character = Character
		};

		DebugEventsSystem des = VWorld.Server.GetExistingSystem<DebugEventsSystem>();
		CastAbilityServerDebugEvent castAbilityServerDebugEvent = new CastAbilityServerDebugEvent
		{
			AbilityGroup = prefabGuid.Value,
			AimPosition = new Nullable_Unboxed<float3>(User.Read<EntityInput>().AimPosition),
			Who = Character.Read<NetworkId>()
		};
		des.CastAbilityServerDebugEvent(User.Read<User>().Index, ref castAbilityServerDebugEvent, ref fromData);
	}
}
