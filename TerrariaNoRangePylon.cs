using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using OnTeleportPylonsSystem = On.Terraria.GameContent.TeleportPylonsSystem;

namespace TerrariaNoRangePylon
{
	public class TerrariaNoRangePylon : Mod {
		private static PylonConfig Config => ModContent.GetInstance<PylonConfig>();
		
		public override void Load() {
			if (Config.OverrideTeleportAnywhere) {
				On.Terraria.GameContent.TeleportPylonsSystem.IsPlayerNearAPylon += OnPlayerNearAPylon;
				IL.Terraria.GameContent.TeleportPylonsSystem.HandleTeleportRequest += ILHandlePlayerPylonRange;
			}
		}

		private bool OnPlayerNearAPylon(OnTeleportPylonsSystem.orig_IsPlayerNearAPylon orig, Player player) => true;

		private void ILHandlePlayerPylonRange(ILContext il) {
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
	}
}