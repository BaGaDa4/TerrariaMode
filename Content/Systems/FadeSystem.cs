using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerrariaMode.Content.Players;

namespace TerrariaMode.Content.Systems
{
    public class FadeSystem : ModSystem
    {
        private Texture2D blackPixel;

        private void EnsurePixelTexture()
        {
            if (blackPixel == null || blackPixel.IsDisposed)
            {
                blackPixel = new Texture2D(Main.instance.GraphicsDevice, 1, 1);
                blackPixel.SetData(new[] { Color.Black });
            }
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            foreach (Player player in Main.player)
            {
                if (player.active)
                {
                    var fade = player.GetModPlayer<FadePlayer>();
                    if (fade.fadingOut)
                    {
                        float alpha = MathHelper.Lerp(0f, fade.maxAlpha, fade.fadeTimer / 120f);

                        EnsurePixelTexture();

                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

                        Rectangle fullscreen = new Rectangle(0, 0, (int)(Main.screenWidth * 1.5f), (int)(Main.screenHeight * 1.5f));
                        spriteBatch.Draw(blackPixel, fullscreen, Color.Black * alpha);

                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
                    }
                }
            }
        }
    }
}