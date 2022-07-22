using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TerrariaNoRangePylon; 

public class NoRangePylon : GlobalPylon {
	public override bool? ValidTeleportCheck_PreNPCCount(TeleportPylonInfo pylonInfo, ref int defaultNecessaryNPCCount) {
		if (pylonInfo.TypeOfPylon == TeleportPylonType.Victory)
			return null;

		if (ModContent.GetInstance<PylonConfig>().OverrideRequiredNPCs)
			defaultNecessaryNPCCount = ModContent.GetInstance<PylonConfig>().RequiredNPCCount;
			
		return base.ValidTeleportCheck_PreNPCCount(pylonInfo, ref defaultNecessaryNPCCount);
	}

	public override bool? ValidTeleportCheck_PreAnyDanger(TeleportPylonInfo pylonInfo) {
		if (ModContent.GetInstance<PylonConfig>().OverrideDangerLimit)
			return true;
		
		return base.ValidTeleportCheck_PreAnyDanger(pylonInfo);
	}

	public override bool? ValidTeleportCheck_PreBiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData) {
		if (ModContent.GetInstance<PylonConfig>().OverrideBiomeLimit)
			return true;
		
		return base.ValidTeleportCheck_PreBiomeRequirements(pylonInfo, sceneData);
	}

	public override bool? PreCanPlacePylon(int x, int y, int tileType, TeleportPylonType pylonType) {
		if (ModContent.GetInstance<PylonConfig>().OverrideTypeLimit)
			return true;
		
		return base.PreCanPlacePylon(x, y, tileType, pylonType);
	}
}