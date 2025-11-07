using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SimpleConstructor.Utils
{
    public class SpawnPlayer : ModPlayer
    {
        public override void OnEnterWorld()
        {
            if (Constants.ENV == "DEV")
            {
                // Delay execution to ensure world is fully loaded
                // Use a one-time timer or hook into a later lifecycle event if available
                // For now, use a safe method with Main.rand for async-like delay isn't possible here,
                // so instead ensure tile placement is solid and updated.
                Player.noFallDmg = true;
                int centerX = (int)((Player.position.X + Player.width / 2f) / 16f);
                int underworldTileY = Main.maxTilesY - 150;

                // Ensure Y is within valid range
                if (underworldTileY < 0 || underworldTileY >= Main.maxTilesY)
                    underworldTileY = Main.maxTilesY - 150; // fallback

                // Place solid tiles below player
                for (int i = -1; i <= 1; ++i)
                {
                    int x = centerX - i;
                    int y = underworldTileY + 1;

                    if (WorldGen.PlaceTile(x, y, TileID.Dirt, false, true))
                    {
                        // Force tile to be checked as solid
                        NetMessage.SendTileSquare(-1, x, y, 3); // Update clients
                    }
                }

                // Re-check that tiles are solid before spawning
                bool tilesBelowAreSolid = true;
                for (int i = -1; i <= 1; ++i)
                {
                    Tile tile = Framing.GetTileSafely(centerX - i, underworldTileY + 1);
                    if (!tile.HasTile || !Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType])
                    {
                        tilesBelowAreSolid = false;
                        break;
                    }
                }

                if (tilesBelowAreSolid)
                {
                    Player.SpawnX = centerX;
                    Player.SpawnY = underworldTileY;

                    // Position player just above the placed tiles
                    Player.position = new Vector2(centerX * 16, (underworldTileY + 1) * 16 - Player.height);
                    Player.velocity = Vector2.Zero; // Reset velocity
                }
                else
                {
                    // Fallback: teleport to safe world spawn
                    Player.SpawnX = Main.spawnTileX;
                    Player.SpawnY = Main.spawnTileY;
                    Player.position = new Vector2(Main.spawnTileX * 16, Main.spawnTileY * 16 - Player.height);
                    Player.velocity = Vector2.Zero;
                }
            }
        }
    }
}

