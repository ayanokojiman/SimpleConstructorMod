using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SimpleConstructor.Utils;

namespace SimpleConstructor.Content.Items.Bases
{
    public abstract class ElevatorBase : ModItem
    {
        protected const int MaxDepth = 10000;
        protected virtual int TunnelWallWidth => 1;
        protected virtual int TunnelWidth => 5;
        // Defines the vertical spacing (in tiles) between light source placements
        protected virtual int LightSourceSpacing => 30;
        // Defines how many consecutive rows can fail to destroy a tile before stopping
        protected virtual int MaxFailsToStop => 3;
        protected virtual int TunnelBlockID => 1;
        protected virtual int? TunnelWallID => null;
        protected virtual int LightSourceID => 4;
        protected virtual int LightSourceStyleID => 0;
        protected virtual int FloorID => 19;
        protected virtual int FloorStyleID => 0;
        protected virtual bool PlaceRope => true;
        private bool IsTunnelWidthEven = false;
        public override void SetDefaults()
        {
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Dig;
            Item.consumable = true;
            IsTunnelWidthEven = (TunnelWidth % 2) == 0;
        }
        private static bool RemoveOneLayer(int startX, int endX, int layerY, int offset = 0)
        {
            bool tileDestroyedInRow = false;

            for (int x = startX - offset; x < endX + offset; ++x)
            {
                Tile tile = Main.tile[x, layerY];

                bool isTileProtected = TileUtils.IsTileProtected(tile);
                if (isTileProtected) continue;

                if (tile.LiquidAmount > 0)
                {
                    tile.LiquidAmount = 0;
                    WorldGen.SquareTileFrame(x, layerY, true);
                }

                if (WorldGen.InWorld(x, layerY))
                {
                    WorldGen.KillTile(x, layerY);
                    tileDestroyedInRow = true;
                }
            }

            return tileDestroyedInRow;
        }
        protected virtual void BuildTunnelWallLayer(int startX, int endX, int layerY)
        {
            if (TunnelWallID == null) return;
            for (int x = startX; x < endX; x++)
            {
                WorldGen.KillWall(x, layerY);
                WorldGen.PlaceWall(x, layerY, (int)TunnelWallID);
            }
        }
        protected virtual void BuildTunnelLayer(int startX, int endX, int midX, int layerY, bool placeLightSource)
        {

            for (int x = startX - TunnelWallWidth; x < startX; ++x)
            {
                TileUtils.PlaceTile(x, layerY, TunnelBlockID, true);
            }
            for (int x = endX; x < endX + TunnelWallWidth; ++x)
            {
                TileUtils.PlaceTile(x, layerY, TunnelBlockID, true);
            }


            if (PlaceRope) TileUtils.PlaceTile(midX, layerY, TileID.Rope);

            if (placeLightSource)
            {
                int offset = 1;
                TileUtils.PlaceTile(startX, layerY, LightSourceID, false, LightSourceStyleID);
                TileUtils.PlaceTile(endX - offset, layerY, LightSourceID, false, LightSourceStyleID);
            }

            BuildTunnelWallLayer(startX, endX, layerY + 1);
        }

        protected virtual void BuildTunnelFloor(int startX, int endX, int startY)
        {
            RemoveOneLayer(startX, endX, startY);
            for (int x = startX; x < endX; x++)
            {
                TileUtils.PlaceTile(x, startY, FloorID, true, FloorStyleID);
            }

        }
        protected virtual void BuildTunnelTop(int startX, int endX, int startY)
        {

        }
        public override bool? UseItem(Player player)
        {

            int centerX = (int)((player.position.X + player.width / 2f) / 16f);
            int centerY = (int)((player.position.Y + player.height) / 16f);

            // Check if player is on the ground
            Tile tile = Framing.GetTileSafely(centerX, centerY);
            if (tile == null || !tile.HasTile || tile.TileType == TileID.Trees)
            {
                Main.NewText("You need to stand on the ground in order to use this item!", Color.Yellow);
                return false;
            }

            int midOffset = (int)Math.Floor(TunnelWidth / 2.0);
            int startX = centerX - midOffset;
            int endX = centerX + midOffset;

            if (!IsTunnelWidthEven)
            {
                endX += 1;
            }

            GameUtils.PrintDebugMessage($"startX: {startX} endX: {endX} midOffset: {midOffset} centerX: {centerX}");

            int layerY = centerY;
            int lightSourceLayerY = centerY + LightSourceSpacing;

            int tilesDestroyedCount = 0;
            int failCount = 0;

            while (layerY < Main.maxTilesY && tilesDestroyedCount < MaxDepth && failCount < MaxFailsToStop)
            {
                bool tileDestroyedInRow = RemoveOneLayer(startX, endX, layerY, TunnelWallWidth);
                if (tileDestroyedInRow)
                {
                    bool placeLightSource = lightSourceLayerY == layerY;

                    BuildTunnelLayer(startX, endX, centerX, layerY, placeLightSource);

                    if (placeLightSource)
                    {
                        lightSourceLayerY += LightSourceSpacing;
                    }
                }

                if (tileDestroyedInRow)
                    failCount = 0;
                else
                    failCount += 1;

                layerY += 1;
                tilesDestroyedCount += 1;
            }

            if (failCount < MaxFailsToStop)
            {
                BuildTunnelFloor(startX, endX, centerY);
                BuildTunnelTop(startX, endX, centerY - 1);
            }

            if (tilesDestroyedCount > 0 && failCount < MaxFailsToStop)
                Main.NewText($"Welcome to Hell! You've broken through {tilesDestroyedCount} tiles!", Color.OrangeRed);
            else
                Main.NewText("Hmm, nothing to dig here, or maybe you hit something too tough!", Color.Yellow);

            return true;
        }
    }

}