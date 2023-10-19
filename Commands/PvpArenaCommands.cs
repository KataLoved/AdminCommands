using AdminCommands.Commands.Converters;
using Unity.Entities;
using VampireCommandFramework;

namespace AdminCommands.Commands;

//may move this into a separate mod at some point, leaving here for now
internal static class PvpArenaCommands
{
	/*	[Command("shards", adminOnly: true)]
		public static void ShardBuffsCommand(ChatCommandContext ctx, int shardType, OnlinePlayer player = null)
		{
			int solarus = 1;
			int winged_horror = 2;
			int behemoth = 3;
			int all = 4;
			if (!player?.Value.IsOnline ?? false)
			{
				throw ctx.Error("Player not found or not online.");
			}

			var User = player?.Value.User ?? ctx.Event.SenderUserEntity;
			var Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;

			if (shardType == solarus)
			{
				if (BuffUtility.HasBuff(VWorld.Server.EntityManager, Character, Prefabs.AB_Interact_UseRelic_Monster_Buff))
				{
					Helper.UnbuffPlayer(Character, Prefabs.AB_Interact_UseRelic_Monster_Buff);
					ctx.Reply("Unbuff'd");
				}
				else
				{
					Helper.BuffPlayer(Character, User, Prefabs.AB_Interact_UseRelic_Monster_Buff, Helper.NO_DURATION, true);
					ctx.Reply("Buff'd");
				}
			}
			else if (shardType == winged_horror)
			{
				if (BuffUtility.HasBuff(VWorld.Server.EntityManager, Character, Prefabs.AB_Interact_UseRelic_Manticore_Buff))
				{
					Helper.UnbuffPlayer(Character, Prefabs.AB_Interact_UseRelic_Manticore_Buff);
					ctx.Reply("Unbuff'd");
				}
				else
				{
					Helper.BuffPlayer(Character, User, Prefabs.AB_Interact_UseRelic_Manticore_Buff, Helper.NO_DURATION, true);
					ctx.Reply("Buff'd");
				}
			}
			else if (shardType == behemoth)
			{
				if (BuffUtility.HasBuff(VWorld.Server.EntityManager, Character, Prefabs.AB_Interact_UseRelic_Behemoth_Buff))
				{
					Helper.UnbuffPlayer(Character, Prefabs.AB_Interact_UseRelic_Behemoth_Buff);
					ctx.Reply("Unbuff'd");
				}
				else
				{
					Helper.BuffPlayer(Character, User, Prefabs.AB_Interact_UseRelic_Behemoth_Buff, Helper.NO_DURATION, true);
					ctx.Reply("Buff'd");
				}
			}
			else if (shardType == all)
			{
				if (BuffUtility.HasBuff(VWorld.Server.EntityManager, Character, Prefabs.AB_Interact_UseRelic_Monster_Buff) && BuffUtility.HasBuff(VWorld.Server.EntityManager, Character, Prefabs.AB_Interact_UseRelic_Manticore_Buff) && BuffUtility.HasBuff(VWorld.Server.EntityManager, Character, Prefabs.AB_Interact_UseRelic_Behemoth_Buff))
				{
					Helper.UnbuffPlayer(Character, Prefabs.AB_Interact_UseRelic_Monster_Buff);
					var action = new ScheduledAction(Helper.UnbuffPlayer, new object[] { Character, Prefabs.AB_Interact_UseRelic_Manticore_Buff });
					ActionScheduler.ScheduleAction(action, 2);
					action = new ScheduledAction(Helper.UnbuffPlayer, new object[] { Character, Prefabs.AB_Interact_UseRelic_Behemoth_Buff });
					ActionScheduler.ScheduleAction(action, 3);
					ctx.Reply("Unbuff'd");
				}
				else
				{
					Helper.BuffPlayer(Character, User, Prefabs.AB_Interact_UseRelic_Monster_Buff, Helper.NO_DURATION, true);
					var action = new ScheduledAction(Helper.BuffPlayer, new object[] { Character, User, Prefabs.AB_Interact_UseRelic_Manticore_Buff, Helper.NO_DURATION, true });
					ActionScheduler.ScheduleAction(action, 2);
					action = new ScheduledAction(Helper.BuffPlayer, new object[] { Character, User, Prefabs.AB_Interact_UseRelic_Behemoth_Buff, Helper.NO_DURATION, true });
					ActionScheduler.ScheduleAction(action, 3);
					ctx.Reply("Buff'd");
				}
			}
		}

		[Command("buffs clear", adminOnly: true)]
		public static void ClearBuffsCommand(ChatCommandContext ctx, FoundPlayer player = null)
		{
			var Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;
			Helper.ClearConsumablesAndShards(Character);
		}

		[Command("buffs big", adminOnly: true)]
		public static void BigBuffsCommand(ChatCommandContext ctx, FoundPlayer player = null)
		{
			var User = player?.Value.User ?? ctx.Event.SenderUserEntity;
			var Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;

			if (BuffUtility.HasBuff(VWorld.Server.EntityManager, Character, Prefabs.AB_Consumable_PhysicalBrew_T02_Buff))
			{
				Helper.UnbuffPlayer(Character, Prefabs.AB_Consumable_PhysicalBrew_T02_Buff);
				Helper.UnbuffPlayer(Character, Prefabs.AB_Consumable_SpellBrew_T02_Buff);
				ctx.Reply("Unbuff'd");
			}
			else
			{
				Helper.BuffPlayer(Character, User, Prefabs.AB_Consumable_PhysicalBrew_T02_Buff, Helper.NO_DURATION);
				var action = new ScheduledAction(Helper.BuffPlayer, new object[] { Character, User, Prefabs.AB_Consumable_SpellBrew_T02_Buff, Helper.NO_DURATION, false });
				ActionScheduler.ScheduleAction(action, 2);
				ctx.Reply("Buff'd");
			}
		}

		[Command("buffs small", adminOnly: true)]
		public static void SmallBuffsCommand(ChatCommandContext ctx, FoundPlayer player = null)
		{
			var User = player?.Value.User ?? ctx.Event.SenderUserEntity;
			var Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;

			if (BuffUtility.HasBuff(VWorld.Server.EntityManager, Character, Prefabs.AB_Consumable_SpellBrew_T01_Buff))
			{
				Helper.UnbuffPlayer(Character, Prefabs.AB_Consumable_SpellBrew_T01_Buff);
				Helper.UnbuffPlayer(Character, Prefabs.AB_Consumable_PhysicalBrew_T01_Buff);
				ctx.Reply("Unbuff'd");
			}
			else
			{
				Helper.BuffPlayer(Character, User, Prefabs.AB_Consumable_SpellBrew_T01_Buff, Helper.NO_DURATION);
				var action = new ScheduledAction(Helper.BuffPlayer, new object[] { Character, User, Prefabs.AB_Consumable_PhysicalBrew_T01_Buff, Helper.NO_DURATION, false });
				ActionScheduler.ScheduleAction(action, 2);
				ctx.Reply("Buff'd");
			}
		}*/

	/*	[Command("jewel", "j", adminOnly: true)]
		public static void JewelCommand(ChatCommandContext ctx, string jewelName, string mods, float power = 1)
		{
			var User = ctx.Event.SenderUserEntity;
			var Character = ctx.Event.SenderCharacterEntity;

			if (!Regex.IsMatch(mods, @"^\d{3,8}$") && mods != "?")
			{
				throw ctx.Error("Invalid mods - should be at least three numbers, i.e. 123");
			}

			var jss = VWorld.Server.GetExistingSystem<JewelSpawnSystem>();

			var processedJewelName = jewelName.Replace(" ", "").ToLowerInvariant();
			PrefabGUID abilityPrefab;
			try
			{
				abilityPrefab = JewelData.abilityToPrefabDictionary[processedJewelName];
			}
			catch (Exception e)
			{
				throw ctx.Error("Invalid ability name");
			}


			if (mods == "?")
			{
				ctx.Reply("Mods for " + jewelName);
				int i = 1;
				foreach (KeyValuePair<PrefabGUID, string> kvp in JewelData.SpellMods[processedJewelName])
				{
					ctx.Reply(i + " - " + kvp.Value);
					i++;
				}
			}
			else
			{
				if (jss.TryCreateJewelAndAddToInventory(Character, abilityPrefab, 2, out Entity jewelEntity))
				{
					try
					{
						if (power > 1)
						{
							power = 1;
						}
						else if (power < 0)
						{
							power = 0;
						}
						power = (float)(0.15 + power * (1 - 0.15));
						SpellModSetComponent spellModSet = jewelEntity.Read<SpellModSetComponent>();
						int mod0 = int.Parse(mods[0].ToString()) - 1;
						int mod1 = int.Parse(mods[1].ToString()) - 1;
						int mod2 = int.Parse(mods[2].ToString()) - 1;
						spellModSet.SpellMods.Mod0.Id = JewelData.SpellMods[processedJewelName][mod0].Key;
						spellModSet.SpellMods.Mod0.Power = power;
						spellModSet.SpellMods.Mod1.Id = JewelData.SpellMods[processedJewelName][mod1].Key;
						spellModSet.SpellMods.Mod1.Power = power;
						spellModSet.SpellMods.Mod2.Id = JewelData.SpellMods[processedJewelName][mod2].Key;
						spellModSet.SpellMods.Mod2.Power = power;
						if (mods.Length > 3)
						{
							int mod3 = int.Parse(mods[3].ToString()) - 1;
							spellModSet.SpellMods.Mod3.Id = JewelData.SpellMods[processedJewelName][mod3].Key;
							spellModSet.SpellMods.Mod3.Power = power;
							if (mods.Length > 4)
							{
								int mod4 = int.Parse(mods[4].ToString()) - 1;
								spellModSet.SpellMods.Mod4.Id = JewelData.SpellMods[processedJewelName][mod4].Key;
								spellModSet.SpellMods.Mod4.Power = power;
								if (mods.Length > 5)
								{
									int mod5 = int.Parse(mods[5].ToString()) - 1;
									spellModSet.SpellMods.Mod5.Id = JewelData.SpellMods[processedJewelName][mod5].Key;
									spellModSet.SpellMods.Mod5.Power = power;
									if (mods.Length > 6)
									{
										int mod6 = int.Parse(mods[6].ToString()) - 1;
										spellModSet.SpellMods.Mod6.Id = JewelData.SpellMods[processedJewelName][mod6].Key;
										spellModSet.SpellMods.Mod6.Power = power;
										if (mods.Length > 7)
										{
											int mod7 = int.Parse(mods[7].ToString()) - 1;
											spellModSet.SpellMods.Mod7.Id = JewelData.SpellMods[processedJewelName][mod7].Key;
											spellModSet.SpellMods.Mod7.Power = power;
										}
									}
								}
							}
						}
						jewelEntity.Write(spellModSet);
						ctx.Reply("Generated jewel");
					}
					catch (Exception e)
					{
						throw ctx.Error(e.ToString());
					}
				}
				else
				{
					throw ctx.Error("No room for jewel in inventory");
				}
			}
		}*/

	/*[Command("legendary", "l", adminOnly: true)]
	public static void LegendaryCommand(ChatCommandContext ctx, string weaponName, string infusion, string mods = "", float power = 1)
	{
		//".chunguslegendary spear static 123"
		if (infusion == "?")
		{
			var i = 0;
			foreach (var description in LegendaryData.statModDescriptions)
			{
				// Convert the index to a hexadecimal string
				var hexValue = i.ToString("X");
				ctx.Reply($"{hexValue} - {description}");
				i++;
			}
		}

		else
		{
			// Updated regex to match hexadecimal values
			if (!Regex.IsMatch(mods, @"^[0-9a-fA-F]{0,3}$") && mods != "?")
			{
				throw ctx.Error("Invalid mods - should be three or four hexadecimal characters, i.e. 123a");
			}
			var weaponPrefabGUID = LegendaryData.weaponToPrefabDictionary[weaponName];
			var infusionPrefabGUID = LegendaryData.infusionToPrefabDictionary[infusion];

			var itemEventEntity = VWorld.Server.EntityManager.CreateEntity(
				ComponentType.ReadWrite<FromCharacter>(),
				ComponentType.ReadWrite<CreateLegendaryWeaponDebugEvent>(),
				ComponentType.ReadWrite<HandleClientDebugEvent>(),
				ComponentType.ReadWrite<NetworkEventType>(),
				ComponentType.ReadWrite<ReceiveNetworkEventTag>()
			);

			var legendaryWeaponDebugEvent = new CreateLegendaryWeaponDebugEvent();
			legendaryWeaponDebugEvent.WeaponPrefabGuid = weaponPrefabGUID;
			legendaryWeaponDebugEvent.Tier = 2;
			legendaryWeaponDebugEvent.InfuseSpellMod = infusionPrefabGUID;

			// Parse mods as hexadecimal values
			if (mods.Length > 0)
			{
				var mod1 = Convert.ToInt32(mods[0].ToString(), 16);
				legendaryWeaponDebugEvent.StatMod1 = LegendaryData.statMods[mod1];
				legendaryWeaponDebugEvent.StatMod1Power = power;
				if (mods.Length > 1)
				{
					var mod2 = Convert.ToInt32(mods[1].ToString(), 16);
					legendaryWeaponDebugEvent.StatMod2 = LegendaryData.statMods[mod2];
					legendaryWeaponDebugEvent.StatMod2Power = power;
					if (mods.Length > 2)
					{
						var mod3 = Convert.ToInt32(mods[2].ToString(), 16);
						legendaryWeaponDebugEvent.StatMod3 = LegendaryData.statMods[mod3];
						legendaryWeaponDebugEvent.StatMod3Power = power;
						if (mods.Length > 3)
						{
							var mod4 = Convert.ToInt32(mods[3].ToString(), 16);
							legendaryWeaponDebugEvent.StatMod4 = LegendaryData.statMods[mod4];
							legendaryWeaponDebugEvent.StatMod4Power = 1;
						}
					}
				}
			}

			var handleClientDebugEvent = itemEventEntity.Read<HandleClientDebugEvent>();
			handleClientDebugEvent.FromUserIndex = ctx.User.Index;

			var fromData = new FromCharacter
			{
				Character = ctx.Event.SenderCharacterEntity,
				User = ctx.Event.SenderUserEntity
			};

			itemEventEntity.Write(handleClientDebugEvent);
			itemEventEntity.Write(fromData);
			itemEventEntity.Write(legendaryWeaponDebugEvent);
			ctx.Reply("Legendary created");
		}
	}*/

	[Command("reset", "r", "Instantly reset cooldown and hp for the player.", adminOnly: true)]
	public static void ResetCommand(ChatCommandContext ctx, FoundPlayer player = null)
	{
		Entity User = player?.Value.User ?? ctx.Event.SenderUserEntity;
		Entity Character = player?.Value.Character ?? ctx.Event.SenderCharacterEntity;
		string name = player?.Value.Name.ToString() ?? ctx.Name;

		Helper.ResetCharacter(Character);

		ctx.Reply($"Player \"{name}\" reset.");
	}
}
