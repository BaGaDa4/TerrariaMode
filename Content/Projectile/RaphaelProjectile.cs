using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaMode.Content.Projectile
{
    public class RaphaelProjectile : ModProjectile
    {
        public override void SetStaticDefaults(){}

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 0;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.9f, 0.8f, 0.3f);

            if (Projectile.timeLeft % 6 == 0 && Main.rand.NextBool())
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame, 0f, 0f, 150, default, 1.1f);
                Main.dust[d].velocity *= 0.8f;
            }

            int targetIndex = Player.FindClosest(Projectile.Center, 1, 1);
            if (targetIndex >= 0)
            {
                Player p = Main.player[targetIndex];
                Vector2 to = p.Center - Projectile.Center;
                float dist = to.Length();
                if (dist > 12f)
                {
                    to.Normalize();
                    float keepSpeed = Projectile.velocity.Length();
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, to * keepSpeed, 0.04f);
                }
            }

            float max = 16f;
            if (Projectile.velocity.Length() > max)
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * max;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            for (int i = 0; i < 10; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame);
                Main.dust[d].velocity *= 1.8f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
                return true;

            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;

            return false;
        }
    }
}
