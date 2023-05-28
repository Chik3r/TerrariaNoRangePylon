using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TerrariaNoRangePylon
{
	public class PylonConfig : ModConfig {
		public const string Base = "$Mods.TerrariaNoRangePylon.Configs.PylonConfig.";
		
		public override ConfigScope Mode => ConfigScope.ServerSide;
		
		[Header("OverridePylonRange")]
		[LabelKey(Base + "OverrideTeleportAnywhere.Label")]
		[TooltipKey(Base + "OverrideTeleportAnywhere.Tooltip")]
		[DefaultValue(true)]
		[ReloadRequired]
		public bool OverrideTeleportAnywhere;

		[LabelKey(Base + "InfinitePylonRange.Label")]
		[TooltipKey(Base + "InfinitePylonRange.Tooltip")]
		[DefaultValue(true)]
		public bool InfinitePylonRange;
		
		[LabelKey(Base + "CustomPylonRange.Label")]
		[TooltipKey(Base + "CustomPylonRange.Tooltip")]
		[DefaultValue(40)]
		[Range(0, 1000)]
		[Increment(20)]
		public int CustomPylonRange;

		[Header("RequiredNPCsForPylon")]
		[LabelKey(Base + "OverrideRequiredNPCs.Label")]
		[TooltipKey(Base + "OverrideRequiredNPCs.Tooltip")]
		[DefaultValue(true)]
		public bool OverrideRequiredNPCs;
		
		[LabelKey(Base + "RequiredNPCCount.Label")]
		[TooltipKey(Base + "RequiredNPCCount.Tooltip")]
		[DefaultValue(0)]
		[Range(0, 10)]
		[DrawTicks]
		public int RequiredNPCCount;
		
		[Header("LimitNumberOfPylons")]
		[LabelKey(Base + "OverrideTypeLimit.Label")]
		[TooltipKey(Base + "OverrideTypeLimit.Tooltip")]
		[DefaultValue(true)]
		public bool OverrideTypeLimit;
		
		[Header("BiomeLimit")]
		[LabelKey(Base + "OverrideBiomeLimit.Label")]
		[TooltipKey(Base + "OverrideBiomeLimit.Tooltip")]
		[DefaultValue(true)]
		public bool OverrideBiomeLimit;
		
		[Header("DangerLimit")]
		[LabelKey(Base + "OverrideDangerLimit.Label")]
		[TooltipKey(Base + "OverrideDangerLimit.Tooltip")]
		[DefaultValue(true)]
		public bool OverrideDangerLimit;
	}
}