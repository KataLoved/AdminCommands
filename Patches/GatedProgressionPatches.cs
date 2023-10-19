using System.Collections.Generic;
using HarmonyLib;
using ProjectM;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;
using Bloodstone.API;
using ProjectM.Terrain;
using Unity.Mathematics;
using AdminCommands.Commands;

namespace AdminCommands.Patches;

[HarmonyPatch(typeof(UpdateUserWorldRegionSystem), nameof(UpdateUserWorldRegionSystem.OnUpdate))]
public static class UpdateUserWorldRegionSystemPatch
{
	public static List<WorldRegionType> BannedRegions = new List<WorldRegionType>()
	{
	};
	public static void Prefix(UpdateUserWorldRegionSystem __instance)
	{
		try
		{
			if (BannedRegions.Count > 0)
			{
				var users = __instance.__UpdateUsersCurrentMapRegions_entityQuery.ToEntityArray(Allocator.Temp);
				foreach (var User in users)
				{
					var user = User.Read<User>();
					if (user.IsAdmin) continue;

					if (BannedRegions.Contains(User.Read<CurrentWorldRegion>().CurrentRegion))
					{
						var Character = user.LocalCharacter._Entity;
						if (!Character.Read<Health>().IsDead)
						{
							Helper.KillCharacter(Character);
							var action = new ScheduledAction(Helper.TeleportPlayer, new object[] { Character, User, new float3(-1967f, 5f, -3169.5f) });
							ActionScheduler.ScheduleAction(action, 2); //teleporting the player somewhere else that they don't get killed before their region updates, but delaying it so their bag drops where they died
							ServerChatUtils.SendSystemMessageToClient(VWorld.Server.EntityManager, user, $"You have been killed for trespassing in a zone that isn't open yet.");
						}
					}
				}
			}
		}
		catch (System.Exception e)
		{

		}
	}
}

[HarmonyPatch(typeof(BloodAltarSystem_StartTrackVBloodUnit_System_V2), nameof(BloodAltarSystem_StartTrackVBloodUnit_System_V2.HandleEvent))]
public static class BloodAltarSystem_StartTrackVBloodUnit_System_V2Patch
{
	public static bool Prefix(BloodAltarSystem_StartTrackVBloodUnit_System_V2 __instance, StartTrackVBloodUnitEventV2 trackVBloodUnitEvent, FromCharacter fromCharacter, NativeHashMap<NetworkId, Entity> networkIdToEntityMap, EntityCommandBuffer spawnCommandBuffer, EntityCommandBuffer destroyCommandBuffer)
	{
		try
		{
			var LockedVBloods = GatedProgressionCommands.GetLockedBosses();
			if (LockedVBloods.Contains(trackVBloodUnitEvent.HuntTarget))
			{
				ServerChatUtils.SendSystemMessageToClient(VWorld.Server.EntityManager, fromCharacter.User.Read<User>(), $"The VBlood you are attempting to track is currently disabled.");
				return false;
			}
		}
		catch (System.Exception e)
		{

		}
		return true;
	}
}
