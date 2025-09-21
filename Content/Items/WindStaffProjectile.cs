using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerrariaMode.Content.Projectiles
{
    public class WindStaffProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60; 
        }

        public override void AI()
        {
            
            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror, 
                Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, default, 1f);
            Main.dust[dustIndex].noGravity = true;

            
            Lighting.AddLight(Projectile.Center, 0.5f, 0.8f, 1f);
        }
    }
}
