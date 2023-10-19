using System;
using AdminCommands.Commands.Converters;
using AdminCommands.Models;
using ProjectM;
using Unity.Transforms;
using VampireCommandFramework;
using Bloodstone.API;
using AdminCommands.Data;
using ProjectM.Network;

namespace AdminCommands.Commands;
internal class TeleportCommands
{
	[Command("teleport", "tp", description: "Teleports you to a set waypoint", adminOnly: false)]
	public void TeleportCommand(ChatCommandContext ctx, string nameOrId, FoundPlayer player = null)
	{
		var User = player?.Value.User ?? ctx.Event.SenderUserEntity;
		var Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;
		if (WaypointManager.TryFindWaypoint(nameOrId, out var Waypoint))
		{
			if (User.Read<User>().IsAdmin || !Waypoint.IsAdminOnly)
			{
				try
				{
					if (User.Read<User>().IsAdmin || !BuffUtility.HasBuff(VWorld.Server.EntityManager, Character, Prefabs.Buff_InCombat))
					{
						Helper.TeleportPlayer(Character, User, Waypoint.Position);
						ctx.Reply("Teleported to waypoint: " + Waypoint.Name);
					}
					else
					{
						ctx.Reply("Can't teleport while in combat");
					}
				}
				catch (Exception e)
				{
					throw ctx.Error(e.ToString());
				}
			}
			else
			{
				throw ctx.Error("Could not find waypoint");
			}
		}
		else
		{
			throw ctx.Error("Could not find waypoint");
		}
	}

	[Command("waypoint create", shortHand:"waypoint add", adminOnly: true)]
	public void CreateWaypointCommand(ChatCommandContext ctx, string name, bool adminOnly = true, string id = "")
	{
		var Character = ctx.Event.SenderCharacterEntity;
		LocalToWorld playerPosition = Character.Read<LocalToWorld>();
		Waypoint waypoint = new Waypoint(id, name, playerPosition.Position, adminOnly);

		WaypointManager.AddWaypoint(waypoint);
		ctx.Reply("Waypoint added! " + waypoint.ToString());
	}

	[Command("waypoint remove", adminOnly: true)]
	public void RemoveWaypointCommand(ChatCommandContext ctx, string nameOrId)
	{
		var Character = ctx.Event.SenderCharacterEntity;
		if (WaypointManager.TryFindWaypoint(nameOrId, out var waypoint))
		{
			WaypointManager.RemoveWaypoint(waypoint.ID);
			ctx.Reply("Waypoint removed");
		}
		else
		{
			ctx.Reply("Waypoint doesn't exist");
		}
	}

	[Command("waypoint list", adminOnly: false)]
	public void ListWaypointsCommand(ChatCommandContext ctx)
	{
		WaypointManager.LoadWaypoints();
		if (WaypointManager.Waypoints.Count > 0)
		{
			ctx.Reply("Waypoints");
			foreach (var waypoint in WaypointManager.Waypoints)
			{
				if (ctx.User.IsAdmin || !waypoint.Value.IsAdminOnly)
				{
					ctx.Reply($"{waypoint.Value.ID} - {waypoint.Value.Name}{(waypoint.Value.IsAdminOnly ? " (admin)" : "")}");
				}
			}
		}
		else
		{
			ctx.Reply("No active waypoints");
		}
	}
}
