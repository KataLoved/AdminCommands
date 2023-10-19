using System.Collections.Generic;
using System.Linq;
using AdminCommands.Data;
using Bloodstone.API;
using Il2CppInterop.Runtime;
using Il2CppSystem;
using ProjectM;
using ProjectM.Network;
using ProjectM.Shared;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace AdminCommands;

//this is horrible god help us all
public static partial class Helper
{
	public const int NO_DURATION = 0;
	public const int DEFAULT_DURATION = -1;
	public const int RANDOM_POWER = -1;

	public static DebugEventsSystem debugEventsSystem = VWorld.Server.GetExistingSystem<DebugEventsSystem>();
	public static NetworkIdSystem networkIdSystem = VWorld.Server.GetExistingSystem<NetworkIdSystem>();
	public static JewelSpawnSystem jewelSpawnSystem = VWorld.Server.GetExistingSystem<JewelSpawnSystem>();
	public static AdminAuthSystem adminAuthSystem = VWorld.Server.GetExistingSystem<AdminAuthSystem>();
	public static PrefabCollectionSystem prefabCollectionSystem = VWorld.Server.GetExistingSystem<PrefabCollectionSystem>();

	public static NativeHashSet<PrefabGUID> prefabGUIDs;

	public static System.Random random = new System.Random();

	public static void ReviveCharacter(Entity Character, Entity User)
    {
		var pos = Character.Read<LocalToWorld>().Position;

		var sbs = VWorld.Server.GetExistingSystem<ServerBootstrapSystem>();
		var bufferSystem = VWorld.Server.GetExistingSystem<EntityCommandBufferSystem>();
		var buffer = bufferSystem.CreateCommandBuffer();

		Nullable_Unboxed<float3> spawnLoc = new();
		spawnLoc.value = pos;
		spawnLoc.has_value = true;
		var health = Character.Read<Health>();
		if (BuffUtility.HasBuff(VWorld.Server.EntityManager, Character, Prefabs.Buff_General_Vampire_Wounded_Buff))
		{
			UnbuffCharacter(Character, Prefabs.Buff_General_Vampire_Wounded_Buff);

			health.Value = health.MaxHealth;
			health.MaxRecoveryHealth = health.MaxHealth;
			Character.Write(health);
		}
		if (health.IsDead)
		{
			sbs.RespawnCharacter(buffer, User,
				customSpawnLocation: spawnLoc,
				previousCharacter: Character);
		}
	}

	public static NativeArray<Entity> GetEntitiesByComponentTypes<T1>(bool includeAll = false)
	{
		EntityQueryOptions options = includeAll ? EntityQueryOptions.IncludeAll : EntityQueryOptions.Default;

		EntityQueryDesc queryDesc = new EntityQueryDesc
		{
			All = new ComponentType[] { new ComponentType(Il2CppType.Of<T1>(), ComponentType.AccessMode.ReadWrite) },
			Options = options
		};

		var query = VWorld.Server.EntityManager.CreateEntityQuery(queryDesc);

		var entities = query.ToEntityArray(Allocator.Temp);
		return entities;
	}

	public static NativeArray<Entity> GetEntitiesByComponentTypes<T1, T2>(bool includeAll = false)
	{
		EntityQueryOptions options = includeAll ? EntityQueryOptions.IncludeAll : EntityQueryOptions.Default;

		EntityQueryDesc queryDesc = new EntityQueryDesc
		{
			All = new ComponentType[] { new ComponentType(Il2CppType.Of<T1>(), ComponentType.AccessMode.ReadWrite), new ComponentType(Il2CppType.Of<T2>(), ComponentType.AccessMode.ReadWrite) },
			Options = options
		};

		var query = VWorld.Server.EntityManager.CreateEntityQuery(queryDesc);

		var entities = query.ToEntityArray(Allocator.Temp);
		return entities;
	}

	public static NativeArray<Entity> GetEntitiesByComponentTypes<T1, T2, T3>(bool includeAll = false)
	{
		EntityQueryOptions options = includeAll ? EntityQueryOptions.IncludeAll : EntityQueryOptions.Default;

		EntityQueryDesc queryDesc = new EntityQueryDesc
		{
			All = new ComponentType[] { new ComponentType(Il2CppType.Of<T1>(), ComponentType.AccessMode.ReadWrite), new ComponentType(Il2CppType.Of<T2>(), ComponentType.AccessMode.ReadWrite), new ComponentType(Il2CppType.Of<T3>(), ComponentType.AccessMode.ReadWrite) },
			Options = options
		};

		var query = VWorld.Server.EntityManager.CreateEntityQuery(queryDesc);

		var entities = query.ToEntityArray(Allocator.Temp);
		return entities;
	}

	public static List<Entity> GetClanMembersByUser(Entity User, bool includeStartingUser = true)
    {
		List<Entity> clanMembers = new List<Entity>();
		if (includeStartingUser)
        {
			clanMembers.Add(User);
		}
		
		var entities = GetEntitiesByComponentTypes<ClanRole>();
		var team = User.Read<Team>();
		foreach (var entity in entities)
		{
			if (entity != User && entity.Read<Team>().Value == team.Value)
			{
				clanMembers.Add(entity);
			}
		}

		return clanMembers;
	}

	public static void CreateAndEquipServantGear(Team playerTeam)
	{
		var servantCoffinComponentType = new ComponentType(Il2CppType.Of<ServantCoffinstation>(), ComponentType.AccessMode.ReadWrite);
		var query = VWorld.Server.EntityManager.CreateEntityQuery(servantCoffinComponentType);
		var servantCoffins = query.ToEntityArray(Unity.Collections.Allocator.Temp);
		foreach (var servantCoffin in servantCoffins)
		{
			var servantCoffinStation = servantCoffin.Read<ServantCoffinstation>();
			var coffinTeam = servantCoffin.Read<Team>();
			if (coffinTeam.Value == playerTeam.Value)
			{
				var servant = servantCoffinStation.ConnectedServant._Entity;
				AddItemToInventory(servant, Kit.servantGear[0], 1, out Entity item, false);
				AddItemToInventory(servant, Kit.servantGear[1], 1, out item);
				AddItemToInventory(servant, Kit.servantGear[2], 1, out item);
				AddItemToInventory(servant, Kit.servantGear[3], 1, out item);
				AddItemToInventory(servant, Kit.servantGear[4], 1, out item);
				AddItemToInventory(servant, Kit.servantGear[5], 1, out item);
				if (InventoryUtilities.TryGetItemAtSlot(VWorld.Server.EntityManager, servant, 0, out InventoryBuffer weapon))
                {
					var servantEquipment = servant.Read<ServantEquipment>();
					servantEquipment.SetEquipped(EquipmentType.Weapon, weapon.ItemEntity._Entity, Kit.servantGear[0]);
					servant.Write(servantEquipment);
					var scheduledAction = new ScheduledAction(InventoryUtilitiesServer.ClearInventory, new object[] { VWorld.Server.EntityManager, servant });
					ActionScheduler.ScheduleAction(scheduledAction, 100);
				}
            }
        }
	}

	public static void ClearInventory(Entity Character, bool all = false)
    {
		int start = 9;
		if (all)
        {
			start = 0;
        }
		for (int i = start; i < 36; i++)
		{
			InventoryUtilitiesServer.ClearSlot(VWorld.Server.EntityManager, Character, i);
		}
	}

	public static void Unlock(FromCharacter fromCharacter)
    {
		debugEventsSystem.UnlockAllResearch(fromCharacter);
		debugEventsSystem.UnlockAllVBloods(fromCharacter);
		debugEventsSystem.CompleteAllAchievements(fromCharacter);
		UnlockAllWaypoints(fromCharacter.User);
		UnlockAllContent(fromCharacter);
	}

	public static void UnlockAllContent(FromCharacter fromCharacter)
    {
		SetUserContentDebugEvent setUserContentDebugEvent = new SetUserContentDebugEvent
		{
			Value = UserContentFlags.EarlyAccess | UserContentFlags.DLC_DraculasRelics_EA | UserContentFlags.GiveAway_Razer01 | UserContentFlags.DLC_FoundersPack_EA | UserContentFlags.Halloween2022 | UserContentFlags.DLC_Gloomrot
		};
		debugEventsSystem.SetUserContentDebugEvent(fromCharacter.User.Read<User>().Index, ref setUserContentDebugEvent, ref fromCharacter);
	}

	public static void UnlockAllWaypoints(Entity User)
    {
		var buffer = VWorld.Server.EntityManager.AddBuffer<UnlockedWaypointElement>(User);
		var waypointComponentType = new ComponentType(Il2CppType.Of<ChunkWaypoint>(), ComponentType.AccessMode.ReadWrite);
		var query = VWorld.Server.EntityManager.CreateEntityQuery(waypointComponentType);
		var waypoints = query.ToEntityArray(Allocator.Temp);
		foreach (var waypoint in waypoints)
		{
			var unlockedWaypoint = new UnlockedWaypointElement();
			unlockedWaypoint.Waypoint = waypoint.Read<NetworkId>();
			buffer.Add(unlockedWaypoint);
		}
	}

	public static void RenamePlayer(FromCharacter fromCharacter, string newName)
    {
		var networkId = fromCharacter.User.Read<NetworkId>();
		var renameEvent = new RenameUserDebugEvent
		{
			NewName = newName,
			Target = networkId
		};
		debugEventsSystem.RenameUser(fromCharacter, renameEvent);
	}

	public static void ResetAllServants(Team playerTeam)
	{
		var servantCoffinComponentType = new ComponentType(Il2CppType.Of<ServantCoffinstation>(), ComponentType.AccessMode.ReadWrite);
		var query = VWorld.Server.EntityManager.CreateEntityQuery(servantCoffinComponentType);
		var servantCoffins = query.ToEntityArray(Allocator.Temp);

		foreach (var servantCoffin in servantCoffins)
		{
			try
			{
				var coffinTeam = servantCoffin.Read<Team>();
				if (coffinTeam.Value == playerTeam.Value)
				{
					var servantCoffinStation = servantCoffin.Read<ServantCoffinstation>();
					var servant = servantCoffinStation.ConnectedServant._Entity;
					var servantEquipment = servant.Read<ServantEquipment>();
					servantEquipment.Reset();
					servant.Write(servantEquipment);
					StatChangeUtility.KillEntity(VWorld.Server.EntityManager, servant, Entity.Null, 0, true);
				}
			}
			catch (System.Exception e)
			{

			}
		}
	}

	public static void GenerateLegendaryViaEvent(FromCharacter fromData, string weapon, string infusion, string mods, float power = 1)
    {
		var weaponPrefabGUID = LegendaryData.weaponToPrefabDictionary[weapon];
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
		bool randomPower = false;
		if (power == RANDOM_POWER)
        {
			randomPower = true;
        }
		else
        {
			power = 0.15f + power * (1 - 0.15f);
		}

		if (mods.Length > 0)
		{
			var mod1 = System.Convert.ToInt32(mods[0].ToString(), 16);
			legendaryWeaponDebugEvent.StatMod1 = LegendaryData.statMods[mod1];
			if (randomPower)
			{
				power = (float)random.NextDouble();
				power = 0.15f + power * (1 - 0.15f);
			}
			legendaryWeaponDebugEvent.StatMod1Power = power;
			if (mods.Length > 1)
			{
				var mod2 = System.Convert.ToInt32(mods[1].ToString(), 16);
				legendaryWeaponDebugEvent.StatMod2 = LegendaryData.statMods[mod2];
				if (randomPower)
				{
					power = (float)random.NextDouble();
					power = 0.15f + power * (1 - 0.15f);
				}

				legendaryWeaponDebugEvent.StatMod2Power = power;
				if (mods.Length > 2)
				{
					var mod3 = System.Convert.ToInt32(mods[2].ToString(), 16);
					legendaryWeaponDebugEvent.StatMod3 = LegendaryData.statMods[mod3];
					if (randomPower)
					{
						power = (float)random.NextDouble();
						power = 0.15f + power * (1 - 0.15f);
					}
					legendaryWeaponDebugEvent.StatMod3Power = power;
					if (mods.Length > 3)
					{
						var mod4 = System.Convert.ToInt32(mods[3].ToString(), 16);
						legendaryWeaponDebugEvent.StatMod4 = LegendaryData.statMods[mod4];
						legendaryWeaponDebugEvent.StatMod4Power = 1;
					}
				}
			}
		}

		var handleClientDebugEvent = itemEventEntity.Read<HandleClientDebugEvent>();
		handleClientDebugEvent.FromUserIndex = fromData.User.Read<User>().Index;

		itemEventEntity.Write(handleClientDebugEvent);
		itemEventEntity.Write(fromData);
		itemEventEntity.Write(legendaryWeaponDebugEvent);
	}

	public static void CreateJewel(FromCharacter fromData, string spellName, string mods = "", float power = 1)
	{
		PrefabGUID abilityPrefab = JewelData.abilityToPrefabDictionary[spellName];
		if (jewelSpawnSystem.TryCreateJewelAndAddToInventory(fromData.Character, abilityPrefab, 2, out Entity jewelEntity))
		{
			bool randomPower = false;
			if (power == JewelData.RANDOM_POWER)
            {
				randomPower = true;
            }
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

			if (mods == "")
            {
				for (var i = 0; i < JewelData.SpellMods[spellName].Count && i < 8; i++)
                {
					mods += (i+1).ToString();
                }
            }
			int mod0 = int.Parse(mods[0].ToString()) - 1;
			int mod1 = int.Parse(mods[1].ToString()) - 1;
			int mod2 = int.Parse(mods[2].ToString()) - 1;
			spellModSet.SpellMods.Mod0.Id = JewelData.SpellMods[spellName][mod0].Key;
			if (randomPower)
			{
				power = (float)random.NextDouble();
				power = 0.15f + power * (1 - 0.15f);
			}
			spellModSet.SpellMods.Mod0.Power = power;
			spellModSet.SpellMods.Mod1.Id = JewelData.SpellMods[spellName][mod1].Key;
			if (randomPower)
			{
				power = (float)random.NextDouble();
				power = 0.15f + power * (1 - 0.15f);
			}
			spellModSet.SpellMods.Mod1.Power = power;
			spellModSet.SpellMods.Mod2.Id = JewelData.SpellMods[spellName][mod2].Key;
			if (randomPower)
			{
				power = (float)random.NextDouble();
				power = 0.15f + power * (1 - 0.15f);
			}
			spellModSet.SpellMods.Mod2.Power = power;
			if (mods.Length > 3)
			{
				int mod3 = int.Parse(mods[3].ToString()) - 1;
				spellModSet.SpellMods.Mod3.Id = JewelData.SpellMods[spellName][mod3].Key;
				if (randomPower)
				{
					power = (float)random.NextDouble();
					power = 0.15f + power * (1 - 0.15f);
				}
				spellModSet.SpellMods.Mod3.Power = power;
				if (mods.Length > 4)
				{
					int mod4 = int.Parse(mods[4].ToString()) - 1;
					spellModSet.SpellMods.Mod4.Id = JewelData.SpellMods[spellName][mod4].Key;
					if (randomPower)
					{
						power = (float)random.NextDouble();
						power = 0.15f + power * (1 - 0.15f);
					}
					spellModSet.SpellMods.Mod4.Power = power;
					if (mods.Length > 5)
					{
						int mod5 = int.Parse(mods[5].ToString()) - 1;
						spellModSet.SpellMods.Mod5.Id = JewelData.SpellMods[spellName][mod5].Key;
						if (randomPower)
						{
							power = (float)random.NextDouble();
							power = 0.15f + power * (1 - 0.15f);
						}
						spellModSet.SpellMods.Mod5.Power = power;
						if (mods.Length > 6)
						{
							int mod6 = int.Parse(mods[6].ToString()) - 1;
							spellModSet.SpellMods.Mod6.Id = JewelData.SpellMods[spellName][mod6].Key;
							if (randomPower)
							{
								power = (float)random.NextDouble();
								power = 0.15f + power * (1 - 0.15f);
							}
							spellModSet.SpellMods.Mod6.Power = power;
							if (mods.Length > 7)
							{
								int mod7 = int.Parse(mods[7].ToString()) - 1;
								spellModSet.SpellMods.Mod7.Id = JewelData.SpellMods[spellName][mod7].Key;
								if (randomPower)
								{
									power = (float)random.NextDouble();
									power = 0.15f + power * (1 - 0.15f);
								}
								spellModSet.SpellMods.Mod7.Power = power;
							}
						}
					}
				}
			}
			jewelEntity.Write(spellModSet);
		}
	}

	public static void RepairGear(Entity Character, bool repair = true)
    {
		Equipment equipment = Character.Read<Equipment>();
		NativeList<Entity> equippedItems = new NativeList<Entity>(Allocator.Temp);
		equipment.GetAllEquipmentEntities(equippedItems);
		foreach (var equippedItem in equippedItems)
		{
			if (equippedItem.Has<Durability>())
			{
				var durability = equippedItem.Read<Durability>();
				if (repair)
                {
					durability.Value = durability.MaxDurability;
				}
				else
                {
					durability.Value = 0;
				}
				
				equippedItem.Write(durability);
			}
		}
		equippedItems.Dispose();

		for (int i = 0; i < 36; i++)
		{
			if (InventoryUtilities.TryGetItemAtSlot(VWorld.Server.EntityManager, Character, i, out InventoryBuffer item))
			{
				var itemEntity = item.ItemEntity._Entity;
				if (itemEntity.Has<Durability>())
				{
					var durability = itemEntity.Read<Durability>();
					if (repair)
                    {
						durability.Value = durability.MaxDurability;
					}
					else
                    {
						durability.Value = 0;
					}
					
					itemEntity.Write(durability);
				}
			}
		}
	}

	public static void GenerateJewelViaEvent(FromCharacter fromData, string spellName, int tier = 4, string mods = "", float power = 1)
    {
		PrefabGUID abilityPrefab = JewelData.abilityToPrefabDictionary[spellName];
		power = 0.15f + power * (1 - 0.15f);
		if (tier == 4)
        {
			GenerateJewelDebugEvent generateJewelDebugEvent = new GenerateJewelDebugEvent();
			generateJewelDebugEvent.AbilityPrefabGuid = abilityPrefab;
			generateJewelDebugEvent.Power = 1;
			generateJewelDebugEvent.Tier = 3;

			debugEventsSystem.GenerateJewelEvent(fromData.User.Read<User>().Index, ref generateJewelDebugEvent);
		}
		else
        {
			var jewelEventEntity = VWorld.Server.EntityManager.CreateEntity(
				ComponentType.ReadWrite<FromCharacter>(),
				ComponentType.ReadWrite<CreateJewelDebugEventV2>(),
				ComponentType.ReadWrite<HandleClientDebugEvent>(),
				ComponentType.ReadWrite<NetworkEventType>(),
				ComponentType.ReadWrite<ReceiveNetworkEventTag>()
			);
			
			if (tier == 3 && mods.Length >= 3)
			{
				int mod0 = int.Parse(mods[0].ToString()) - 1;
				int mod1 = int.Parse(mods[1].ToString()) - 1;
				int mod2 = int.Parse(mods[2].ToString()) - 1;

				CreateJewelDebugEventV2 createJewelDebugEvent = jewelEventEntity.Read<CreateJewelDebugEventV2>();
				createJewelDebugEvent.AbilityPrefabGuid = abilityPrefab;
				createJewelDebugEvent.Tier = 2;
				createJewelDebugEvent.SpellMod1 = JewelData.SpellMods[spellName][mod0].Key;
				createJewelDebugEvent.SpellMod1Power = power;
				createJewelDebugEvent.SpellMod2 = JewelData.SpellMods[spellName][mod1].Key;
				createJewelDebugEvent.SpellMod2Power = power;
				createJewelDebugEvent.SpellMod3 = JewelData.SpellMods[spellName][mod2].Key;
				createJewelDebugEvent.SpellMod3Power = power;
				if (mods.Length > 3)
				{
					int mod3 = int.Parse(mods[3].ToString()) - 1;
					createJewelDebugEvent.SpellMod4 = JewelData.SpellMods[spellName][mod3].Key;
					createJewelDebugEvent.SpellMod4Power = power;
				}

				jewelEventEntity.Write(createJewelDebugEvent);

				HandleClientDebugEvent handleClientDebugEvent = jewelEventEntity.Read<HandleClientDebugEvent>();
				handleClientDebugEvent.FromUserIndex = fromData.User.Read<User>().Index;
				jewelEventEntity.Write(handleClientDebugEvent);
			}
		}
	}

	public static bool TryGetPrefabGUIDFromString(string buffNameOrId, out PrefabGUID prefabGUID)
	{
		if (prefabCollectionSystem.NameToPrefabGuidDictionary.ContainsKey(buffNameOrId))
		{
			prefabGUID = prefabCollectionSystem.NameToPrefabGuidDictionary[buffNameOrId];
			return true;
		}
		else
		{
			if (int.TryParse(buffNameOrId, out int prefabGuidId))
            {
				var prefabGuid = new PrefabGUID(prefabGuidId);
				if (prefabCollectionSystem.PrefabGuidToNameDictionary.ContainsKey(prefabGuid))
                {
					prefabGUID = prefabGuid;
					return true;
				}
            }
		}
		prefabGUID = default;
		return false;
	}

	// Create a structure to store item and its matching score
	struct MatchItem
	{
		public int Score;
		public Item Item;
	}

	public static bool TryGetItemPrefabGUIDFromString(string needle, out PrefabGUID prefabGUID)
    {
		List<MatchItem> matchedItems = new List<MatchItem>();

		// Check against OverrideName
		foreach (var item in Items.GiveableItems)
		{
			int score = IsSubsequence(needle, item.OverrideName.ToLower() + "s");
			if (score != -1)
			{
				matchedItems.Add(new MatchItem { Score = score, Item = item });
			}
		}

		// Check against FormalPrefabName
		foreach (var item in Items.GiveableItems)
		{
			int score = IsSubsequence(needle, item.FormalPrefabName.ToLower() + "s");
			if (score != -1)
			{
				matchedItems.Add(new MatchItem { Score = score, Item = item });
			}
		}

		if (int.TryParse(needle, out int result))
		{
			// Check against PrefabGUID
			foreach (var item in Items.GiveableItems)
			{
				if (result == item.PrefabGUID.GuidHash)
				{
					matchedItems.Add(new MatchItem { Score = int.MaxValue, Item = item });  // PrefabGUID gets the highest possible score
				}
			}
		}

		// Sort the matched items by score, descending
		var bestMatch = matchedItems.OrderByDescending(m => m.Score).FirstOrDefault();
		if (bestMatch.Item != null)
        {
			prefabGUID = bestMatch.Item.PrefabGUID;
			return true;
        }
		prefabGUID = default;
		return false;
	}

	public static bool AddItemToInventory(Entity recipient, string needle, int amount, out Entity entity, bool equip = true)
	{
		if (TryGetItemPrefabGUIDFromString(needle, out PrefabGUID item))
        {
			return AddItemToInventory(recipient, item, amount, out entity, equip);
		}

		entity = default;
		return false;
	}


    public static bool AddItemToInventory(Entity recipient, PrefabGUID guid, int amount, out Entity entity, bool equip = true)
	{
		var gameData = VWorld.Server.GetExistingSystem<GameDataSystem>();
		var itemSettings = AddItemSettings.Create(VWorld.Server.EntityManager, gameData.ItemHashLookupMap);
		itemSettings.EquipIfPossible = equip;
		var inventoryResponse = InventoryUtilitiesServer.TryAddItem(itemSettings, recipient, guid, amount);
		if (inventoryResponse.Success)
		{
			entity = inventoryResponse.NewEntity;
			return true;
		}
		else
		{
			entity = new Entity();
			return false;
		}
	}
	
	public static void KillCharacter(Entity Character)
	{
		StatChangeUtility.KillEntity(VWorld.Server.EntityManager, Character, Character, 0, true);
	}

	public static bool BuffCharacter(Entity character, PrefabGUID buff, int duration = DEFAULT_DURATION, bool persistsThroughDeath = false)
	{
		//The user doesn't seem to do anything important, so this can be used to buff non-player characters
		return BuffPlayer(character, GetAnyUser(), buff, duration, persistsThroughDeath);
	}

	private static Entity GetAnyUser()
    {
		var entities = GetEntitiesByComponentTypes<User>(true);
		foreach (var entity in entities)
        {
			return entity;
        }
		return Entity.Null;
    }

	public static bool BuffPlayer(Entity character, Entity user, PrefabGUID buff, int duration = DEFAULT_DURATION, bool persistsThroughDeath = false)
	{
		var des = VWorld.Server.GetExistingSystem<DebugEventsSystem>();
		var buffEvent = new ApplyBuffDebugEvent()
		{
			BuffPrefabGUID = buff
		};
		var fromCharacter = new FromCharacter()
		{
			User = user,
			Character = character
		};
		if (!BuffUtility.TryGetBuff(VWorld.Server.EntityManager, character, buff, out Entity buffEntity))
		{
			des.ApplyBuff(fromCharacter, buffEvent);
			if (BuffUtility.TryGetBuff(VWorld.Server.EntityManager, character, buff, out buffEntity))
			{
                if (buffEntity.Has<CreateGameplayEventsOnSpawn>())
                {
                    buffEntity.Remove<CreateGameplayEventsOnSpawn>();
                }
                if (buffEntity.Has<GameplayEventListeners>())
                {
                    buffEntity.Remove<GameplayEventListeners>();
                }

                if (persistsThroughDeath)
				{
					buffEntity.Add<Buff_Persists_Through_Death>();
					if (buffEntity.Has<RemoveBuffOnGameplayEvent>())
                    {
						buffEntity.Remove<RemoveBuffOnGameplayEvent>();
					}
					
					if (buffEntity.Has<RemoveBuffOnGameplayEventEntry>())
                    {
						buffEntity.Remove<RemoveBuffOnGameplayEventEntry>();
					}
				}
				if (duration > 0 && duration != DEFAULT_DURATION)
				{
					if (buffEntity.Has<LifeTime>())
					{
						var lifetime = buffEntity.Read<LifeTime>();
						lifetime.Duration = duration;
						buffEntity.Write(lifetime);
					}
				}
				else if (duration == NO_DURATION)
				{
					if (buffEntity.Has<LifeTime>())
                    {
						var lifetime = buffEntity.Read<LifeTime>();
						lifetime.Duration = -1;
						lifetime.EndAction = LifeTimeEndAction.None;
						buffEntity.Write(lifetime);
						//buffEntity.Remove<LifeTime>();
					}
					if (buffEntity.Has<RemoveBuffOnGameplayEvent>())
					{
						buffEntity.Remove<RemoveBuffOnGameplayEvent>();
					}
					if (buffEntity.Has<RemoveBuffOnGameplayEventEntry>())
					{
						buffEntity.Remove<RemoveBuffOnGameplayEventEntry>();
					}
				}
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}

	}

	public static void UnbuffCharacter(Entity Character, PrefabGUID buffGUID)
	{
		if (BuffUtility.TryGetBuff(VWorld.Server.EntityManager, Character, buffGUID, out var buffEntity))
		{
			DestroyUtility.Destroy(VWorld.Server.EntityManager, buffEntity, DestroyDebugReason.TryRemoveBuff);
		}
	}

	public static void ClearExtraBuffs(Entity player)
	{
		var buffs = VWorld.Server.EntityManager.GetBuffer<BuffBuffer>(player);
		var stringsToIgnore = new List<string>
		{ 
			"BloodBuff",
			"SetBonus",
			"EquipBuff",
			"Combat",
			"VBlood_Ability_Replace",
			"Shapeshift",
			"Interact",
			"AB_Consumable",
		};

		foreach (var buff in buffs)
		{
			bool shouldRemove = true;
			foreach (string word in stringsToIgnore)
			{
				if (buff.PrefabGuid.LookupName().Contains(word))
				{
					shouldRemove = false;
					break;
				}
			}
			if (shouldRemove)
			{
				DestroyUtility.Destroy(VWorld.Server.EntityManager, buff.Entity, DestroyDebugReason.TryRemoveBuff);
			}
		}
		var equipment = player.Read<Equipment>();
        if (!equipment.IsEquipped(Prefabs.Item_Cloak_Main_ShroudOfTheForest, out var equipmentType) && BuffUtility.HasBuff(VWorld.Server.EntityManager, player, Prefabs.EquipBuff_ShroudOfTheForest))
        {
			UnbuffCharacter(player, Prefabs.EquipBuff_ShroudOfTheForest);
        }
	}

	public static void ClearConsumablesAndShards(Entity player)
	{
        ClearConsumables(player);
		ClearShards(player);
	}

	public static void ClearConsumables(Entity player)
    {
		var buffs = VWorld.Server.EntityManager.GetBuffer<BuffBuffer>(player);
		var stringsToRemove = new List<string>
		{
			"Consumable",
		};
		
		foreach (var buff in buffs)
		{
			bool shouldRemove = false;
			foreach (string word in stringsToRemove)
			{
				if (buff.PrefabGuid.LookupName().Contains(word))
				{
					shouldRemove = true;
					break;
				}
			}
			if (shouldRemove)
			{
				DestroyUtility.Destroy(VWorld.Server.EntityManager, buff.Entity, DestroyDebugReason.TryRemoveBuff);
			}
		}
	}

	public static void ClearShards(Entity player)
    {
		var buffs = VWorld.Server.EntityManager.GetBuffer<BuffBuffer>(player);
		var stringsToRemove = new List<string>
		{
			"UseRelic",
		};
		
		foreach (var buff in buffs)
		{
			bool shouldRemove = false;
			foreach (string word in stringsToRemove)
			{
				if (buff.PrefabGuid.LookupName().Contains(word))
				{
					shouldRemove = true;
					break;
				}
			}
			if (shouldRemove)
			{
				DestroyUtility.Destroy(VWorld.Server.EntityManager, buff.Entity, DestroyDebugReason.TryRemoveBuff);
			}
		}
	}

	public static void MakeAdminPermanently(Entity Character, Entity User)
    {
		var user = User.Read<User>();

		Entity entity;
		if (!adminAuthSystem._LocalAdminList.Contains(User.Read<User>().PlatformId))
        {
			adminAuthSystem._LocalAdminList.Add(User.Read<User>().PlatformId);
		}
		adminAuthSystem._LocalAdminList.Save();

		User.Add<AdminUser>();
		User.Write(new AdminUser()
		{
			AuthMethod = AdminAuthMethod.Authenticated,
			Level = AdminLevel.SuperAdmin
		});

		user.IsAdmin = true;
		User.Write(user);
		entity = VWorld.Server.EntityManager.CreateEntity(
			ComponentType.ReadWrite<FromCharacter>(),
			ComponentType.ReadWrite<AdminAuthEvent>()
		);
		entity.Write(new FromCharacter()
		{
			Character = Character,
			User = User
		});
	}


	
	public static void DisableAdminPermanently(Entity Character, Entity User)
    {
		var user = User.Read<User>();

		Entity entity;
		if (adminAuthSystem._LocalAdminList.Contains(User.Read<User>().PlatformId))
        {
			adminAuthSystem._LocalAdminList.Remove(User.Read<User>().PlatformId);
			adminAuthSystem._LocalAdminList.Save();
		}
		if (User.Has<AdminUser>())
        {
			User.Remove<AdminUser>();
		}
		
		user.IsAdmin = false;
		entity = VWorld.Server.EntityManager.CreateEntity(
			ComponentType.ReadWrite<FromCharacter>(),
			ComponentType.ReadWrite<DeauthAdminEvent>()
		);
		entity.Write(new FromCharacter()
		{
			Character = Character,
			User = User
		});
		User.Write(user);
	}

	public static bool ToggleAdmin(Entity Character, Entity User)
    {
		try
        {
			adminAuthSystem._LocalAdminList.RefreshLocal(true);
		}
		catch (System.Exception e)
        {
			Unity.Debug.Log(e.ToString());
		}
		
		bool isAdmin = false;
		if (adminAuthSystem._LocalAdminList.Contains(User.Read<User>().PlatformId))
        {
			isAdmin = true;
        }
		try
        {
			if (isAdmin)
			{
				Unity.Debug.Log($"Disabling admin permanently: {User.Read<User>().PlatformId}");
				DisableAdminPermanently(Character, User);
				return false;
			}
			else
			{
				Unity.Debug.Log("Attempting to make admin permanently");
				MakeAdminPermanently(Character, User);
				return true;
			}
		}
		catch (System.Exception e)
        {
			Unity.Debug.Log(e.ToString());
		}
		return false;
    }

	public static void TeleportPlayer(Entity Character, Entity User, float3 position)
	{
		var entity = VWorld.Server.EntityManager.CreateEntity(
			ComponentType.ReadWrite<FromCharacter>(),
			ComponentType.ReadWrite<PlayerTeleportDebugEvent>()
		);
		entity.Write<FromCharacter>(new()
		{
			User = User,
			Character = Character
		});
		entity.Write<PlayerTeleportDebugEvent>(new()
		{
			Position = new float3(position.x, position.y, position.z),
			Target = PlayerTeleportDebugEvent.TeleportTarget.Self
		});		
	}

	public static void ResetCharacter(Entity Character)
	{
		ResetCooldown(Character);
		HealCharacter(Character);
		UnbuffCharacter(Character, Data.Buff.Buff_InCombat_PvPVampire);
	}

	public static void ResetCooldown(Entity PlayerCharacter)
	{
		var AbilityBuffer = VWorld.Server.EntityManager.GetBuffer<AbilityGroupSlotBuffer>(PlayerCharacter);
		foreach (var ability in AbilityBuffer)
		{
			var AbilitySlot = ability.GroupSlotEntity._Entity;
			var ActiveAbility = AbilitySlot.Read<AbilityGroupSlot>();
			var ActiveAbility_Entity = ActiveAbility.StateEntity._Entity;
			if (ActiveAbility_Entity.Index > 0)
			{
				var b = ActiveAbility_Entity.Read<PrefabGUID>();
				if (b.GuidHash == 0) continue;

				if (ActiveAbility_Entity.Has<AbilityChargesState>())
				{
					var abilityChargesState = ActiveAbility_Entity.Read<AbilityChargesState>();
					var abilityChargesData = ActiveAbility_Entity.Read<AbilityChargesData>();
					abilityChargesState.CurrentCharges = abilityChargesData.MaxCharges;
					abilityChargesState.ChargeTime = 0;
					ActiveAbility_Entity.Write(abilityChargesState);
				}

				var AbilityStateBuffer = ActiveAbility_Entity.ReadBuffer<AbilityStateBuffer>();
				foreach (var state in AbilityStateBuffer)
				{
					var abilityState = state.StateEntity._Entity;
					var abilityCooldownState = abilityState.Read<AbilityCooldownState>();
					abilityCooldownState.CooldownEndTime = 0;
					abilityState.Write(abilityCooldownState);
				}
			}
		}
	}

	public static void HealCharacter(Entity Character)
	{
		Health health = Character.Read<Health>();
		health.Value = health.MaxHealth;
		health.MaxRecoveryHealth = health.MaxHealth;
		Character.Write(health);
	}

	private static int IsSubsequence(string needle, string haystack)
	{
		int j = 0;
		int maxConsecutiveMatches = 0;
		int currentConsecutiveMatches = 0;

		for (int i = 0; i < needle.Length; i++)
		{
			while (j < haystack.Length && haystack[j] != needle[i])
			{
				j++;
			}

			if (j == haystack.Length)
			{
				return -1;
			}

			if (i > 0 && needle[i - 1] == haystack[j - 1])
			{
				currentConsecutiveMatches++;
			}
			else
			{
				if (currentConsecutiveMatches > maxConsecutiveMatches)
				{
					maxConsecutiveMatches = currentConsecutiveMatches;
				}
				currentConsecutiveMatches = 1;
			}

			j++;
		}

		if (currentConsecutiveMatches > maxConsecutiveMatches)
		{
			maxConsecutiveMatches = currentConsecutiveMatches;
		}

		return maxConsecutiveMatches;
	}
}
