using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TerrariaNoRangePylon
{
	public class PylonConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;
		
		[Header("Teleportation from anywhere")]
		[Label("Enable pylon teleportation from anywhere")]
		[Tooltip("Allows you to teleport to pylons without being next to them.")]
		[DefaultValue(true)]
		[ReloadRequired]
		public bool OverrideTeleportAnywhere;

		[Header("Required NPCs for pylon")]
		[Label("Override the required number of NPCs")]
		[Tooltip("Override the required number of NPCs to be able to teleport to the pylon.")]
		[DefaultValue(true)]
		public bool OverrideRequiredNPCs;
		
		[Label("Number of required NPCs")]
		[Tooltip("The number of NPCs required to teleport to this pylon.")]
		[DefaultValue(0)]
		[Range(0, 10)]
		[DrawTicks]
		public int RequiredNPCCount;
	}
}