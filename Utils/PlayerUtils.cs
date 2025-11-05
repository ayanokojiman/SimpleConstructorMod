using Terraria;

namespace SimpleConstructor.Utils
{
    public static class PlayerUtils
    {
        public static string GetFacingDirection(Player playerInstance)
        {
            return playerInstance.direction == 1
                ? "right"
                : (playerInstance.direction == -1 ? "left" : "neutral");
        }
    }
}