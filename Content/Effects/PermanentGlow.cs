using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaMode.Content.Players
{
    public class GlowPlayer : ModPlayer
    {
        private readonly Color[] glowColors = new Color[]
        {
            new Color(255, 200, 50),
            new Color(255, 100, 100),
            new Color(100, 200, 255),
            new Color(180, 100, 255)
        };

        private int colorIndex = 0;
        private int colorTimer = 0;
        private const int switchDelay = 600;

        public override void PostUpdateMiscEffects()
        {
            if (Player.active && !Player.dead)
            {
                colorTimer++;
                if (colorTimer >= switchDelay)
                {
                    colorTimer = 0;
                    colorIndex = (colorIndex + 1) % glowColors.Length;
                }

                Vector3 glowVec = glowColors[colorIndex].ToVector3() * 2f;

                Lighting.AddLight(Player.Center, glowVec);
            }
        }
    }
}
