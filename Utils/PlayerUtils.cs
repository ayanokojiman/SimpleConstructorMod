using Microsoft.Xna.Framework;
using Terraria;

namespace SimpleConstructor.Utils
{
    public static class PlayerUtils
    {
        public static Player GetPlayer()
        {
            return Main.LocalPlayer;
        }
        public static string GetFacingDirection(Player playerInstance)
        {
            return playerInstance.direction == 1
                ? "right"
                : (playerInstance.direction == -1 ? "left" : "neutral");
        }
        public static Vector2 GetPlayerPos()
        {
            Player p = GetPlayer();
            int centerX = (int)((p.position.X + p.width / 2f) / 16f);
            int centerY = (int)((p.position.Y + p.height) / 16f);
            return new Vector2(centerX, centerY);
        }
    }
}