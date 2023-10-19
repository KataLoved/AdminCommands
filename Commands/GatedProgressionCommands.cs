using System.Collections.Generic;
using AdminCommands.Commands.Converters;
using AdminCommands.Patches;
using ProjectM;
using ProjectM.Terrain;
using VampireCommandFramework;
using ProjectM.Gameplay.Scripting;
using AdminCommands.Data;

namespace AdminCommands.Commands;
internal class GatedProgressionCommands
{
	public record struct FoundRegion(WorldRegionType Value);

	public class FoundRegionConverter : CommandArgumentConverter<FoundRegion>
	{
		private static Dictionary<string, WorldRegionType> NameToRegionMap = new Dictionary<string, WorldRegionType>
		{
			{"none", WorldRegionType.None},
			{"other", WorldRegionType.Other},
			{"startcave", WorldRegionType.StartCave},
			{"farbane", WorldRegionType.FarbaneWoods},
			{"dunleyfarmlands", WorldRegionType.DunleyFarmlands},
			{"cursedforest", WorldRegionType.CursedForest},
			{"hallowedmountains", WorldRegionType.HallowedMountains},
			{"silverlighthills", WorldRegionType.SilverlightHills},
			{"gloomrotsouth", WorldRegionType.Gloomrot_South},
			{"gloomrotnorth", WorldRegionType.Gloomrot_North } 
		};

		public override FoundRegion Parse(ICommandContext ctx, string input)
		{
			foreach (var kvp in NameToRegionMap)
			{
				if (kvp.Key.Contains(input.Trim().ToLower()))
				{
					return new FoundRegion(kvp.Value);
				}
			}
			throw ctx.Error("Invalid world region name");
		}
	}

	[Command("banregion", description: "Prevents non-admins from entering a specified region", adminOnly: true)]
	public void BanRegionCommand(ChatCommandContext ctx, FoundRegion region)
	{
		UpdateUserWorldRegionSystemPatch.BannedRegions.Add(region.Value);
		ctx.Reply($"Enabled killzone around {region.Value}");
	}

	[Command("unbanregion", description: "Removes killzone from specified region", adminOnly: true)]
	public void UnbanRegionCommand(ChatCommandContext ctx, FoundRegion region)
	{
		UpdateUserWorldRegionSystemPatch.BannedRegions.Remove(region.Value);
		ctx.Reply($"Disabled killzone around {region.Value}");
	}

	[Command("lockboss", description: "Removes a boss from the world until you unlock it", adminOnly: true)]
	public void LockBossCommand(ChatCommandContext ctx, FoundVBlood bossName)
	{
		var prefabGUID = bossName.Value;
		var entities = Helper.GetEntitiesByComponentTypes<VBloodUnit>(true);
		foreach (var entity in entities)
		{
			if (entity.Read<PrefabGUID>() == prefabGUID)
			{
				var applyBuffUnderHealthThreshold = entity.Read<Script_ApplyBuffUnderHealthThreshhold_DataServer>();
				applyBuffUnderHealthThreshold.NewBuffEntity = Prefabs.ServantMissionBuff;
				applyBuffUnderHealthThreshold.HealthFactor = 1;
				entity.Write(applyBuffUnderHealthThreshold);
			}
		}
		ctx.Reply($"Locked {prefabGUID.LookupName()}");
	}

	[Command("unlockboss", description: "Removes the lock on a boss", adminOnly: true)]
	public void UnlockBossCommand(ChatCommandContext ctx, FoundVBlood bossName)
	{
		var prefabGUID = bossName.Value;
		var entities = Helper.GetEntitiesByComponentTypes<VBloodUnit>(true);
		foreach (var entity in entities)
		{			
			if (entity.Read<PrefabGUID>() == prefabGUID)
			{
				var applyBuffUnderHealthThreshold = entity.Read<Script_ApplyBuffUnderHealthThreshhold_DataServer>();
				applyBuffUnderHealthThreshold.NewBuffEntity = Prefabs.Buff_General_VBlood_Downed;
				applyBuffUnderHealthThreshold.HealthFactor = 0;
				applyBuffUnderHealthThreshold.ThresholdMet = false;
				entity.Write(applyBuffUnderHealthThreshold);
				Helper.UnbuffCharacter(entity, Prefabs.ServantMissionBuff);
			}
		}
		ctx.Reply($"Unlocked {prefabGUID.LookupName()}");
	}

	[Command("listlockedbosses", description: "Lists all the bosses currently locked", adminOnly: false)]
	public void ListLockedBosses(ChatCommandContext ctx)
	{
		var LockedBosses = GetLockedBosses();

		if (LockedBosses.Count > 0)
		{
			ctx.Reply("[Locked Bosses]");
			foreach (var lockedBossGuid in LockedBosses)
			{
				ctx.Reply(FoundVBloodConverter.VBloodPrefabToName[lockedBossGuid]);
			}
		}
		else
		{
			ctx.Reply("No locked bosses");
		}
	}

	public static List<PrefabGUID> GetLockedBosses()
	{
		var entities = Helper.GetEntitiesByComponentTypes<VBloodUnit, Script_ApplyBuffUnderHealthThreshhold_DataServer>(true);
		List<PrefabGUID> LockedBosses = new List<PrefabGUID>();
		foreach (var entity in entities)
		{
			var applyBuffUnderHealthThreshold = entity.Read<Script_ApplyBuffUnderHealthThreshhold_DataServer>();
			if (applyBuffUnderHealthThreshold.NewBuffEntity == Prefabs.ServantMissionBuff)
			{
				if (!LockedBosses.Contains(entity.Read<PrefabGUID>()))
				{
					LockedBosses.Add(entity.Read<PrefabGUID>());
				}
			}
		}
		return LockedBosses;
	}
}
