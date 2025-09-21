using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.ItemDropRules;
using TerrariaMode.Content.Players;



namespace TerrariaMode.Content.NPCs
{
    public class CustomBoss : ModNPC
    {
        public override void SetStaticDefaults()
        {

            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.width = 400;
            NPC.height = 400;
            NPC.damage = 50;
            NPC.defense = 20;
            NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            Music = MusicID.Boss1;
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];

            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];

                if (player.dead)
                {
                    NPC.velocity.Y -= 0.1f;
                    if (NPC.timeLeft > 10) NPC.timeLeft = 10;
                    return;
                }
            }


            Vector2 moveTo = player.Center;
            float speed = 6f;
            Vector2 move = moveTo - NPC.Center;
            float magnitude = move.Length();
            if (magnitude > 0f)
            {
                move *= speed / magnitude;
            }
            else
            {
                move = new Vector2(0f, speed);
            }
            NPC.velocity = (NPC.velocity * 20f + move) / 21f;


            NPC.ai[0]++;
            if (NPC.ai[0] >= 120)
            {
                NPC.ai[0] = 0;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Vector2 velocity = Vector2.Normalize(player.Center - NPC.Center) * 10f;
                    Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        NPC.Center,
                        velocity,
                        ProjectileID.Fireball,
                        20,
                        1f,
                        Main.myPlayer
                    );
                }
            }
        }

        public override void OnKill()
        {
            CombatText.NewText(NPC.Hitbox, Color.Red, "Custom Boss побеждён!");
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {

            npcLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 1, 5, 15));
        }
    }
}