using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TerrariaNoRangePylon
{
	public class PylonConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;
		
		[Header("Override pylon range")]
		[Label("Enable pylon teleportation from a custom distance")]
		[Tooltip("Allows you to teleport to pylons without being next to them.")]
		[DefaultValue(true)]
		[ReloadRequired]
		public bool OverrideTeleportAnywhere;

		[Label("Set the range limit to infinity")]
		[Tooltip("Allows you to use pylons from any distance. This requires the first option to be enabled.")]
		[DefaultValue(true)]
		public bool InfinitePylonRange;
		
		[Label("Set the range limit to a custom distance")]
		[Tooltip("Allows you to use pylons from a custom distance (in blocks).\nThis requires the first option to be enabled.\nThis setting will be ignored if the range limit is set to infinity.")]
		[DefaultValue(40)]
		[Range(0, 1000)]
		[Increment(20)]
		public int CustomPylonRange;

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
		
		[Header("Limit number of pylons")]
		[Label("Disable the limit on the number of pylons")]
		[Tooltip("Disables the limit that only allows one of each type of pylon.")]
		[DefaultValue(true)]
		public bool OverrideTypeLimit;
		
		[Header("Biome limit")]
		[Label("Disable the biome check")]
		[Tooltip("This will allow you to teleport to pylons that are placed in any biome.")]
		[DefaultValue(true)]
		public bool OverrideBiomeLimit;
		
		[Header("Danger limit")]
		[Label("Disable the danger checks")]
		[Tooltip("Allows you to teleport to pylons while ignoring any active invasions or other dangers.")]
		[DefaultValue(true)]
		public bool OverrideDangerLimit;
	}
}