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

	public override bool? ValidTeleportCheck_PreBiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData) {
		if (ModContent.GetInstance<PylonConfig>().OverrideTeleportAnywhere)
			return true;
		
		return base.ValidTeleportCheck_PreBiomeRequirements(pylonInfo, sceneData);
	}
}