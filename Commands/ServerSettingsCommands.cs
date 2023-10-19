using ProjectM;
using VampireCommandFramework;
using ProjectM.Network;
using Bloodstone.API;

namespace AdminCommands.Commands;

internal static class ServerSettingsCommands
{
	public static SetDebugSettingEvent BuildingCostsDebugSetting = new SetDebugSettingEvent()
	{
		SettingType = DebugSettingType.BuildCostsDisabled,
		Value = false
	};

	public static SetDebugSettingEvent BuildingPlacementRestrictionsDisabledSetting = new SetDebugSettingEvent()
	{
		SettingType = DebugSettingType.BuildingPlacementRestrictionsDisabled,
		Value = false
	};

	public static SetDebugSettingEvent CastleLimitsDisabledSetting = new SetDebugSettingEvent()
	{
		SettingType = DebugSettingType.CastleLimitsDisabled,
		Value = false
	};

	[Command("togglefreebuild", description: "Makes building costs free for everyone and removes placement restrictions", adminOnly: true)]
	public static void ToggleBuildingCostsCommand(ChatCommandContext ctx)
	{
		var User = ctx.Event.User;
		var debugEventsSystem = VWorld.Server.GetExistingSystem<DebugEventsSystem>();

		BuildingCostsDebugSetting.Value = !BuildingCostsDebugSetting.Value;
		debugEventsSystem.SetDebugSetting(User.Index, ref BuildingCostsDebugSetting);


		BuildingPlacementRestrictionsDisabledSetting.Value = !BuildingPlacementRestrictionsDisabledSetting.Value;
		debugEventsSystem.SetDebugSetting(User.Index, ref BuildingPlacementRestrictionsDisabledSetting);

		CastleLimitsDisabledSetting.Value = !CastleLimitsDisabledSetting.Value;
		debugEventsSystem.SetDebugSetting(User.Index, ref CastleLimitsDisabledSetting);

		if (BuildingCostsDebugSetting.Value)
		{
			ctx.Reply("Free building enabled globally -- Do not place hearts with this enabled, they will crash the server");
		}
		else
		{
			ctx.Reply("Free building disabled");
		}
	}
}
