using System.Linq;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TerrariaNoRangePylon
{
	public class TerrariaNoRangePylon : Mod {
		private static PylonConfig Config => ModContent.GetInstance<PylonConfig>();
		
		public override void Load() {
			if (Config.OverrideTeleportAnywhere) {
				On_TeleportPylonsSystem.IsPlayerNearAPylon += OnPlayerNearAPylon;
				IL_TeleportPylonsSystem.HandleTeleportRequest += ILHandlePlayerPylonRange;
			}
		}

		private bool OnPlayerNearAPylon(On_TeleportPylonsSystem.orig_IsPlayerNearAPylon orig, Player player) {
			if (Config.InfinitePylonRange)
				return true;

			return Main.PylonSystem.Pylons.Any(pylonInfo =>
				Vector2.Distance(player.Center, pylonInfo.PositionInTiles.ToWorldCoordinates()) < (Config.CustomPylonRange * 16));
		}

		private void ILHandlePlayerPylonRange(ILContext il) {
			/*
			 * // if (!player.InInteractionRange(teleportPylonInfo.PositionInTiles.X, teleportPylonInfo.PositionInTiles.Y, TileReachCheckSettings.Pylons))
			 * IL_017f: ldloc.0
			 * IL_0180: ldloc.s 14
			 * IL_0182: ldfld valuetype Terraria.DataStructures.Point16 Terraria.GameContent.TeleportPylonInfo::PositionInTiles
			 * IL_0187: ldfld int16 Terraria.DataStructures.Point16::X
			 * IL_018c: ldloc.s 14
			 * IL_018e: ldfld valuetype Terraria.DataStructures.Point16 Terraria.GameContent.TeleportPylonInfo::PositionInTiles
			 * IL_0193: ldfld int16 Terraria.DataStructures.Point16::Y
			 * IL_0198: call valuetype Terraria.DataStructures.TileReachCheckSettings Terraria.DataStructures.TileReachCheckSettings::get_Pylons()
			 * IL_019d: callvirt instance bool Terraria.Player::InInteractionRange(int32, int32, valuetype Terraria.DataStructures.TileReachCheckSettings)
			 * INSERT: pop
			 * INSERT: ldc.i4.1
			 * IL_01a2: brfalse IL_022e
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
				    i => i.MatchCall<TileReachCheckSettings>("get_Pylons"),
				    i => i.MatchCallvirt<Player>("InInteractionRange"))) {
				Logger.Warn("Failed to IL edit ILHandlePlayerPylonRange");
				return;
			}

			c.Emit(OpCodes.Pop);
			c.Emit(OpCodes.Ldc_I4_1);
		}
	}
}