using AdminCommands.Commands;
using HarmonyLib;
using Il2CppSystem;
using ProjectM;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Bloodstone.API;
using AdminCommands.Data;

namespace AdminCommands.Patches;


[HarmonyPatch(typeof(DeathEventListenerSystem), nameof(DeathEventListenerSystem.OnUpdate))]
public static class AutoRevivePatches
{
	public static void Postfix(DeathEventListenerSystem __instance)
	{
		foreach (var killCall in __instance._OnKillCalls)
		{
			if (killCall.Killed.Has<PlayerCharacter>() && ReviveCommands.AutoReviveCharactersToUsers.ContainsKey(killCall.Killed) && ReviveCommands.AutoReviveDictionary.ContainsKey(killCall.Killed) && ReviveCommands.AutoReviveDictionary[killCall.Killed])
			{
				var pos = killCall.Killed.Read<LocalToWorld>().Position;

				var sbs = VWorld.Server.GetExistingSystem<ServerBootstrapSystem>();
				var bufferSystem = VWorld.Server.GetExistingSystem<EntityCommandBufferSystem>();
				var buffer = bufferSystem.CreateCommandBuffer();

				Nullable_Unboxed<float3> spawnLoc = new();
				spawnLoc.value = pos;
				spawnLoc.has_value = true;

				sbs.RespawnCharacter(buffer, ReviveCommands.AutoReviveCharactersToUsers[killCall.Killed],
					customSpawnLocation: spawnLoc,
					previousCharacter: killCall.Killed, spawnLocationIndex: 0);

				Helper.ResetCooldown(killCall.Killed);
				Helper.UnbuffCharacter(killCall.Killed, new PrefabGUID(697095869)); //pvp in combat
				Helper.UnbuffCharacter(killCall.Killed, new PrefabGUID(1591132469)); //pvp death
			}
		}
	}
}


[HarmonyPatch(typeof(VampireDownedServerEventSystem), nameof(VampireDownedServerEventSystem.OnUpdate))]
public static class VampireDownedPatch
{
	public static void Postfix(VampireDownedServerEventSystem __instance)
	{
		var downedEvents = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Allocator.Temp);
		foreach (var entity in downedEvents)
		{
			if (!VampireDownedServerEventSystem.TryFindRootOwner(entity, 1, VWorld.Server.EntityManager, out var victimEntity))
			{
				Plugin.PluginLog.LogMessage("Couldn't get victim entity");
				return;
			}

			if (ReviveCommands.AutoReviveDictionary.ContainsKey(victimEntity) && ReviveCommands.AutoReviveDictionary[victimEntity])
			{
				Health health = victimEntity.Read<Health>();
				health.Value = health.MaxHealth;
				health.MaxRecoveryHealth = health.MaxHealth;
				victimEntity.Write(health);
				Helper.ResetCooldown(victimEntity);
				var action = new ScheduledAction(Helper.UnbuffCharacter, new object[] { victimEntity, Data.Buff.Buff_InCombat_PvPVampire });
				ActionScheduler.ScheduleAction(action, 2);
				action = new ScheduledAction(Helper.UnbuffCharacter, new object[] { victimEntity, Data.Buff.Buff_General_VampirePvPDeathDebuff });
				ActionScheduler.ScheduleAction(action, 2);
				Helper.UnbuffCharacter(victimEntity, Data.Buff.Buff_General_Vampire_Wounded_Buff);
				if (!GodCommands.isBuffEnabled(victimEntity, "immortal") && !GodCommands.isBuffEnabled(victimEntity, "immortal"))
				{
					Helper.BuffPlayer(victimEntity, ReviveCommands.AutoReviveCharactersToUsers[victimEntity], Prefabs.AB_Vampire_Sword_Shockwave_Recast_ImmaterialBuff, 2);
					Helper.BuffPlayer(victimEntity, ReviveCommands.AutoReviveCharactersToUsers[victimEntity], Prefabs.VampireDeathBuff, 2);
					Helper.BuffPlayer(victimEntity, ReviveCommands.AutoReviveCharactersToUsers[victimEntity], Prefabs.AB_Blood_BloodRite_Immaterial, 2);
					ActionScheduler.ScheduleAction(action, 60);
				}
			}
		}
	}
}
