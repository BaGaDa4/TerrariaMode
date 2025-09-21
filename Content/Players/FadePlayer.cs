using Terraria;
using Terraria.ModLoader;
using TerrariaMode.Content.NPCs;

namespace TerrariaMode.Content.Players
{
    public class FadePlayer : ModPlayer
    {
        public int fadeTimer;
        public bool fadingOut;
        public float maxAlpha = 1f;

        public void StartFadeOut()
        {
            fadingOut = true;
            fadeTimer = 0;
        }

        public override void PostUpdate()
        {
            if (fadingOut)
            {
                fadeTimer++;
                if (fadeTimer == 60)
                {
                    NPC.SpawnOnPlayer(Player.whoAmI, ModContent.NPCType<CustomBoss>());
                }
                if (fadeTimer > 120)
                {
                    fadingOut = false;
                    fadeTimer = 0;
                }
            }
        }
    }
}
