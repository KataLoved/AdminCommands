using System;
using Bloodstone.API;
using AdminCommands.Commands.Converters;
using Il2CppInterop.Runtime;
using ProjectM;
using ProjectM.Network;
using Unity.Entities;
using VampireCommandFramework;

namespace AdminCommands.Commands;
internal class ServantCommands
{
	[Command("reviveservants", description: "Revives all servants", adminOnly: true)]
	public void ReviveServantsCommand(ChatCommandContext ctx, FoundPlayer player = null)
	{
		var Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;
		var playerTeam = Character.Read<Team>();
		var servantCoffinComponentType = new ComponentType(Il2CppType.Of<ServantCoffinstation>(), ComponentType.AccessMode.ReadWrite);
		var query = VWorld.Server.EntityManager.CreateEntityQuery(servantCoffinComponentType);
		var servantCoffins = query.ToEntityArray(Unity.Collections.Allocator.Temp);
		foreach (var servantCoffin in servantCoffins)
		{
			var coffinTeam = servantCoffin.Read<Team>();
			if (coffinTeam.Value == playerTeam.Value)
			{
				var coffin = servantCoffin.Read<ServantCoffinstation>();
				if (coffin.State == ServantCoffinState.Converting || coffin.State == ServantCoffinState.Reviving || coffin.State == ServantCoffinState.ServantRevivable)
				{
					if (coffin.State == ServantCoffinState.ServantRevivable)
					{
						coffin.State = ServantCoffinState.Reviving;
					}
					coffin.ConvertionProgress = 600;
				}
					
				servantCoffin.Write(coffin);
			}	
		}

		ctx.Reply($"Resurrected servants for {player?.Value.Name ?? "you"}.");
	}

	[Command("setservants", description: "Overwrites servants with units of your choice, do .setservants ? to see options", adminOnly: true)]
	public void SetServantsCommand(ChatCommandContext ctx, string servantTypes, float quality = 1f, bool includeGear = true, FoundPlayer targetPlayer = null)
	{
		var User = targetPlayer?.Value.User ?? ctx.Event.SenderUserEntity;
		var Character = targetPlayer?.Value.Character ?? ctx.Event.SenderCharacterEntity;

		if (servantTypes == "?")
		{
			for (var i = 0; i < WillisCore.Data.ServantData.ServantTypes.Count; i++)
			{
				ctx.Reply($"{i.ToString("X")}: {WillisCore.Data.ServantData.ServantTypes[i]}");
			}
			return;
		}

		var playerTeam = Character.Read<Team>();
		var servantCoffins = Helper.GetEntitiesByComponentTypes<ServantCoffinstation>();

		var ClanUsers = Helper.GetClanMembersByUser(User);
		var ClanIndex = 0;
		var index = 0;
		if (servantCoffins.Length > 0)
		{
			foreach (var servantCoffin in servantCoffins)
			{
				var coffinTeam = servantCoffin.Read<Team>();
				if (coffinTeam.Value == playerTeam.Value)
				{
					// Get the current character from the servantTypes string and convert it to a hex number
					var hexChar = servantTypes[index % servantTypes.Length];
					var hexIndex = Convert.ToInt32(hexChar.ToString(), 16);

					var servantType = WillisCore.Data.ServantData.UnitToServantList[hexIndex];
					index++;

					var servantCoffinStation = servantCoffin.Read<ServantCoffinstation>();
					var servant = servantCoffinStation.ConnectedServant._Entity;
					if (servant.Index > 0 && servantCoffinStation.State == ServantCoffinState.ServantAlive)
					{
						var servantEquipment = servant.Read<ServantEquipment>();
						servantEquipment.Reset();
						servant.Write(servantEquipment);
						InventoryUtilitiesServer.ClearInventory(VWorld.Server.EntityManager, servant);
						StatChangeUtility.KillEntity(VWorld.Server.EntityManager, servant, Entity.Null, 0, true);
					}
					servantCoffinStation.ServantName = ClanUsers[ClanIndex % ClanUsers.Count].Read<User>().CharacterName;
					ClanIndex++;
					servantCoffinStation.State = ServantCoffinState.Reviving;
					servantCoffinStation.ConvertionProgress = 600;
					servantCoffinStation.ConvertFromUnit = servantType.Key;
					servantCoffinStation.ConvertToUnit = servantType.Value;
					servantCoffinStation.BloodQuality = 100;
					servantCoffinStation.ServantProficiency = quality * .44f;

					servantCoffin.Write(servantCoffinStation);
				}
				if (includeGear)
				{
					var scheduledAction = new ScheduledAction(Helper.CreateAndEquipServantGear, new object[] { playerTeam });
					ActionScheduler.ScheduleAction(scheduledAction, 5);
				}
			}
			ctx.Reply("Replaced all servants with the ones specified");
		}
		else
		{
			ctx.Reply("No servant coffins found");
		}
	}
}
