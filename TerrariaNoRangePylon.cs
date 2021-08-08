using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using OnTeleportPylonsSystem = On.Terraria.GameContent.TeleportPylonsSystem;

namespace TerrariaNoRangePylon
{
	public class TerrariaNoRangePylon : Mod
	{
		public override void Load()
		{
			On.Terraria.GameContent.TeleportPylonsSystem.IsPlayerNearAPylon += OnPlayerNearAPylon; 
			IL.Terraria.GameContent.TeleportPylonsSystem.HandleTeleportRequest += ILTeleportRequestInRange;
			
			if (true)
				IL.Terraria.GameContent.TeleportPylonsSystem.DoesPylonHaveEnoughNPCsAroundIt += ILPylonEnoughNPCs;
		}

		private void ILTeleportRequestInRange(ILContext il)
		{
			/*
			 * IL_0190: ldloc.s   V_22
			 * IL_0192: ldfld     valuetype Terraria.DataStructures.Point16 Terraria.GameContent.TeleportPylonInfo::PositionInTiles
			 * IL_0197: ldfld     int16 Terraria.DataStructures.Point16::Y
			 * IL_019C: callvirt  instance bool Terraria.Player::InInteractionRange(int32, int32)
			 * IL_01A1: ldc.i4.0
			 * IL_01A2: ceq
			 * IL_01A4: stloc.s   V_24
			 * IL_01A6: ldloc.s   V_24
			 * INSERT:  br        IL_01AF
			 * INSERT:  pop
			 * REMOVE:  IL_01A8: brfalse.s IL_01AF
			 */

			ILCursor c = new(il);
			ILLabel label = null;

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchLdloc(22),
				i => i.MatchLdfld(typeof(TeleportPylonInfo).GetField("PositionInTiles")),
				i => i.MatchLdfld(typeof(Point16).GetField("Y")),
				i => i.MatchCallvirt(out _),
				i => i.MatchLdcI4(0),
				i => i.MatchCeq(),
				i => i.MatchStloc(24),
				i => i.MatchLdloc(24),
				i => i.MatchBrfalse(out label))) {
				Logger.Warn("Failed to IL edit HandleTeleportRequest");
				return;
			}

			c.Index--;
			c.Remove();
			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Br_S, label);
		}

		private bool OnPlayerNearAPylon(OnTeleportPylonsSystem.orig_IsPlayerNearAPylon orig, Player player)
		{
			return true;
		}
		
		private void ILPylonEnoughNPCs(ILContext il)
		{
			/*
			 * IL_0036: ldc.i4.2
			 * IL_0037: div
			 * IL_0038: sub
			 * IL_0039: ldsfld    int32 Terraria.Main::buffScanAreaWidth
			 * IL_003E: ldsfld    int32 Terraria.Main::buffScanAreaHeight
			 * IL_0043: call      instance void [FNA]Microsoft.Xna.Framework.Rectangle::.ctor(int32, int32, int32, int32)
			 * IL_0048: ldarg.2
			 * IL_0049: stloc.2
			 * <===>
			 * INSERT:  ldc.i4.0
			 * INSERT:  stloc.2
			 * <===>
			 * IL_004A: ldc.i4.0
			 */
			
			
			ILCursor c = new(il);

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchLdcI4(2),
				i => i.MatchDiv(),
				i => i.MatchSub(),
				i => i.MatchLdsfld(typeof(Main).GetField("buffScanAreaWidth", BindingFlags.Static | BindingFlags.Public)),
				i => i.MatchLdsfld(typeof(Main).GetField("buffScanAreaHeight", BindingFlags.Static | BindingFlags.Public)),
				i => i.MatchCall(out _),
				i => i.MatchLdarg(2),
				i => i.MatchStloc(2))) {
				Logger.Warn("Failed to apply IL to DoesPylonHaveEnoughNPCsAroundIt");
				return;
			}

			c.Emit(OpCodes.Ldc_I4_0);
			c.Emit(OpCodes.Stloc_2);
		}
	}
}