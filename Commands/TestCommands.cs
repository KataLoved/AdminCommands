using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminCommands.Commands.Converters;
using AdminCommands.Patches;
using Il2CppInterop.Runtime;
using ProjectM;
using ProjectM.LightningStorm;
using ProjectM.Network;
using ProjectM.Terrain;
using ProjectM.Tiles;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using VampireCommandFramework;
using ProjectM.Auth;
using ProjectM.Gameplay.Systems;
using ProjectM.Shared;
using System.Reflection;
using static Il2CppSystem.Diagnostics.Tracing.EventProvider;
using Unity.Collections;
using Lidgren.Network;
using Stunlock.Network;
using ProjectM.Scripting;
using Bloodstone.API;
using ProjectM.Gameplay.Scripting;
using Il2CppSystem;
using Unity.Mathematics;
using static ProjectM.BuffUtility;

namespace AdminCommands.Commands;
internal class TestCommands
{
/*	[Command("test", description: "Used for debugging", adminOnly: false)]
	public void TestCommand(ChatCommandContext ctx, FoundPlayer player = null)
	{
		var User = player?.Value.User ?? ctx.Event.SenderUserEntity;
		var Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;

		ctx.Reply("test run");
	}*/
}
