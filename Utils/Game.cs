using Microsoft.Xna.Framework;
using Terraria;

namespace SimpleConstructor.Utils
{
    public static class GameUtils
    {
        public static void PrintDebugMessage(string message)
        {
            if (Constants.ENV == "DEV")
            {
                Main.NewText(message, Color.Purple);
            }
        }
    }
}