using System.Collections.Generic;
using ProjectM;

namespace AdminCommands.Data;

public static class LegendaryData
{
	public static readonly Dictionary<string, PrefabGUID> weaponToPrefabDictionary = new Dictionary<string, PrefabGUID>
	{
		{"slashers", Prefabs.Item_Weapon_Slashers_Legendary_T08},
		{"spear", Prefabs.Item_Weapon_Spear_Legendary_T08},
		{"axe", Prefabs.Item_Weapon_Axe_Legendary_T08},
		{"greatsword", Prefabs.Item_Weapon_GreatSword_Legendary_T08},
		{"crossbow", Prefabs.Item_Weapon_Crossbow_Legendary_T08},
		{"pistols", Prefabs.Item_Weapon_Pistols_Legendary_T08},
		{"reaper", Prefabs.Item_Weapon_Reaper_Legendary_T08},
		{"sword", Prefabs.Item_Weapon_Sword_Legendary_T08},
		{"mace", Prefabs.Item_Weapon_Mace_Legendary_T08},
	};

	public static readonly Dictionary<string, PrefabGUID> infusionToPrefabDictionary = new Dictionary<string, PrefabGUID>
	{
		{"blood", Prefabs.SpellMod_Weapon_BloodInfused},
		{"chaos", Prefabs.SpellMod_Weapon_ChaosInfused},
		{"frost", Prefabs.SpellMod_Weapon_FrostInfused},
		{"illusion", Prefabs.SpellMod_Weapon_IllusionInfused},
		{"static", Prefabs.SpellMod_Weapon_StormInfused},
		{"unholy", Prefabs.SpellMod_Weapon_UndeadInfused},
	};

	public static List<PrefabGUID> statMods = new List<PrefabGUID>()
	{
		Prefabs.StatMod_AttackSpeed,
		Prefabs.StatMod_CriticalStrikePhysical,
		Prefabs.StatMod_CriticalStrikePhysicalPower,
		Prefabs.StatMod_SpellPower,
		Prefabs.StatMod_PhysicalResistance,
		Prefabs.StatMod_MovementSpeed,
		Prefabs.StatMod_CriticalStrikeSpells,
		Prefabs.StatMod_CriticalStrikeSpellPower,
		Prefabs.StatMod_SpellLeech,
		Prefabs.StatMod_ResourceYield,
		Prefabs.StatMod_MaxHealth,
		Prefabs.StatMod_HealthRegen,
		Prefabs.StatMod_PhysicalPower,
	};

	public static List<string> statModDescriptions = new List<string>()
	{
		"Attack Speed",
		"Physical Critical Strike Chance",
		"Physical Critical Strike Damage",
		"Spell Power",
		"Physical Damage Reduction",
		"Movement Speed",
		"Spell Critical Strike Chance",
		"Spell Critical Strike Damage",
		"Spell Life Leech",
		"Resource Yield",
		"Max Health",
		"Health Regen",
		"Physical Power"
	};
}
