using System;
using Microsoft.Xna.Framework;
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

			if (ModContent.GetInstance<PylonConfig>().OverrideRequiredNPCs)
				IL.Terraria.GameContent.TeleportPylonsSystem.HowManyNPCsDoesPylonNeed += ILNPCPylonCount;
			else
				IL.Terraria.GameContent.TeleportPylonsSystem.HandleTeleportRequest += ILHandleTeleportNPCCount;

			IL.Terraria.GameContent.TeleportPylonsSystem.HandleTeleportRequest += ILHandlePlayerPylonRange;
			IL.Terraria.Map.TeleportPylonsMapLayer.Draw += ILPylonMapColor;
		}

		private bool OnPlayerNearAPylon(OnTeleportPylonsSystem.orig_IsPlayerNearAPylon orig, Player player) => true;

		private void ILNPCPylonCount(ILContext il)
		{
			/*
			 * IL_0000: ldarg.1
			 * IL_0001: ldfld valuetype Terraria.GameContent.TeleportPylonType Terraria.GameContent.TeleportPylonInfo::TypeOfPylon
			 * IL_0006: ldc.i4.8
			 * IL_0007: beq.s IL_000b

			 * IL_0009: ldc.i4.2
			 * INSERT:  pop
			 * INSERT   delegate to npc count
			 * IL_000a: ret
			 */

			ILCursor c = new(il);

			if (!c.TryGotoNext(MoveType.After,
				i => i.MatchLdarg(1),
				i => i.MatchLdfld(typeof(TeleportPylonInfo).GetField("TypeOfPylon")),
				i => i.MatchLdcI4(8),
				i => i.MatchBeq(out _),
				i => i.MatchLdcI4(2))) {
				Logger.Warn("Failed to IL edit HowManyNPCsDoesPylonNeed");
				return;
			}

			c.Emit(OpCodes.Pop);
			c.EmitDelegate(() => ModContent.GetInstance<PylonConfig>().RequiredNPCCount);
		}
		
		private void ILHandleTeleportNPCCount(ILContext il)
		{
			/*
			 * IL_001b: ldstr "Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon"
			 * IL_0020: stloc.1

			 * IL_0021: ldloc.2
			 * INSERT:  pop
			 * INSERT:  ldc.i4.0
			 * IL_0022: brfalse.s IL_0043
			 */

			ILCursor c = new(il);

			if (!c.TryGotoNext(MoveType.After,
				    i => i.MatchLdstr("Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon"),
				    i => i.MatchStloc(1),
				    i => i.MatchLdloc(2))) {
				Logger.Warn("Failed to IL edit HandleTeleportRequest");
				return;
			}

			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldc_I4_0);
		}
		
		private void ILHandlePlayerPylonRange(ILContext il)
		{
			/*
			 * // if (!player.InInteractionRange(info2.PositionInTiles.X, info2.PositionInTiles.Y))
			 * IL_012d: ldloc.0
			 * IL_012e: ldloc.s 9
			 * IL_0130: ldfld valuetype Terraria.DataStructures.Point16 Terraria.GameContent.TeleportPylonInfo::PositionInTiles
			 * IL_0135: ldfld int16 Terraria.DataStructures.Point16::X
			 * IL_013a: ldloc.s 9
			 * IL_013c: ldfld valuetype Terraria.DataStructures.Point16 Terraria.GameContent.TeleportPylonInfo::PositionInTiles
			 * IL_0141: ldfld int16 Terraria.DataStructures.Point16::Y
			 * IL_0146: callvirt instance bool Terraria.Player::InInteractionRange(int32, int32)
			 * INSERT: pop
			 * INSERT: ldc.i4.1
			 * IL_014b: brfalse IL_01d5
			 */
			
			ILCursor c = new(il);

			if (!c.TryGotoNext(MoveType.After,
				    i => i.MatchLdloc(0),
				    i => i.MatchLdloc(out _),
				    i => i.MatchLdfld<TeleportPylonInfo>("PositionInTiles"),
				    i => i.MatchLdfld<Point16>("X"),
				    i => i.MatchLdloc(out _),
				    i => i.MatchLdfld<TeleportPylonInfo>("PositionInTiles"),
				    i => i.MatchLdfld<Point16>("Y"),
				    i => i.MatchCallvirt<Player>("InInteractionRange"))) {
				Logger.Warn("Failed to IL edit ILHandlePlayerPylonRange");
				return;
			}

			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldc_I4_1);
		}
		
		private void ILPylonMapColor(ILContext il)
		{
			/*
			 * IL_006e: ldc.r4 1.5
			 * IL_0073: ldc.r4 2
			 * IL_0078: newobj instance void [FNA]Microsoft.Xna.Framework.Vector2::.ctor(float32, float32)
			 * IL_007d: call valuetype [FNA]Microsoft.Xna.Framework.Vector2 [FNA]Microsoft.Xna.Framework.Vector2::op_Addition(valuetype [FNA]Microsoft.Xna.Framework.Vector2, valuetype [FNA]Microsoft.Xna.Framework.Vector2)
			 * IL_0082: ldloc.s 4
			 * INSERT: pop
			 * INSERT: Color.White
			 */
			
			ILCursor c = new(il);

			if (!c.TryGotoNext(MoveType.After,
				    i => i.MatchLdcR4(1.5f),
				    i => i.MatchLdcR4(2),
				    i => i.MatchNewobj<Vector2>(),
				    i => i.MatchCall<Vector2>("op_Addition"),
				    i => i.MatchLdloc(4))) {
				Logger.Warn("Failed to IL edit ILPylonMapColor");
				return;
			}

			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Call, typeof(Color).GetProperty("White")!.GetGetMethod());
		}
	}
}