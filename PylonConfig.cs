using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TerrariaNoRangePylon
{
	public class PylonConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("Required NPCs for pylon")]
		[Label("Override the required number of NPCs")]
		[Tooltip("Override the required number of NPCs to be able to teleport to the pylon")]
		[DefaultValue(false)]
		[ReloadRequired]
		public bool OverrideRequiredNPCs;
		
		[Label("Number of required NPCs")]
		[Tooltip("The number of NPCs required to teleport to this pylon")]
		[DefaultValue(2)]
		[Range(0, 10)]
		[DrawTicks]
		public int RequiredNPCCount;
	}
}