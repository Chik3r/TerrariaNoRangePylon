using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TerrariaNoRangePylon; 

public class NoRangePylon : GlobalPylon {
	private static PylonConfig Config => ModContent.GetInstance<PylonConfig>();
	
	public override bool? ValidTeleportCheck_PreNPCCount(TeleportPylonInfo pylonInfo, ref int defaultNecessaryNpcCount) {
		if (pylonInfo.TypeOfPylon == TeleportPylonType.Victory)
			return null;

		if (Config.OverrideRequiredNPCs)
			defaultNecessaryNpcCount = Config.RequiredNPCCount;
			
		return base.ValidTeleportCheck_PreNPCCount(pylonInfo, ref defaultNecessaryNpcCount);
	}

	public override bool? ValidTeleportCheck_PreAnyDanger(TeleportPylonInfo pylonInfo) {
		if (Config.OverrideDangerLimit)
			return true;
		
		return base.ValidTeleportCheck_PreAnyDanger(pylonInfo);
	}

	public override bool? ValidTeleportCheck_PreBiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData) {
		if (Config.OverrideBiomeLimit)
			return true;
		
		return base.ValidTeleportCheck_PreBiomeRequirements(pylonInfo, sceneData);
	}

	public override bool? PreCanPlacePylon(int x, int y, int tileType, TeleportPylonType pylonType) {
		if (Config.OverrideTypeLimit)
			return true;
		
		return base.PreCanPlacePylon(x, y, tileType, pylonType);
	}
}