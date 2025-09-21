using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TerrariaMode.Content.Projectile;

namespace TerrariaMode.Content.NPCs
{
    public class Raphael : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.BossBestiaryPriority.Add(Type);
        }

        public override void SetDefaults()
        {
            NPC.width = 192;
            NPC.height = 192;
            NPC.lifeMax = 35000;
            NPC.damage = 50;
            NPC.defense = 18;
            NPC.knockBackResist = 0f;

            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.aiStyle = -1;
            NPC.value = Item.buyPrice(0, 5);
            NPC.boss = true;
            NPC.npcSlots = 10f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.lavaImmune = true;

            Music = MusicID.Boss2;

            if (Main.expertMode)
            {
                NPC.lifeMax = (int)(NPC.lifeMax * 1.2f);
                NPC.damage = (int)(NPC.damage * 1.1f);
            }
            if (Main.masterMode)
            {
                NPC.lifeMax = (int)(NPC.lifeMax * 1.15f);
                NPC.damage = (int)(NPC.damage * 1.1f);
            }
        }

        public override void AI()
        {
            if (!NPC.HasValidTarget)
                NPC.TargetClosest();

            Player target = Main.player[NPC.target];
            if (!target.active || target.dead)
            {
                NPC.velocity.Y -= 0.1f;
                if (NPC.timeLeft > 60) NPC.timeLeft = 60;
                return;
            }

            float desiredRange = 420f;
            Vector2 toPlayer = target.Center - NPC.Center;
            float distance = toPlayer.Length();
            Vector2 dirToPlayer = distance == 0 ? Vector2.Zero : toPlayer / distance;

            float side = Math.Sign((float)Math.Sin(Main.GameUpdateCount / 90f));
            Vector2 desiredOffset = new Vector2(side * 220f, -120f * (float)Math.Cos(Main.GameUpdateCount / 60f));
            Vector2 desiredPos = target.Center - dirToPlayer * desiredRange + desiredOffset;

            float maxSpeed = 4f;
            float accel = 0.110f;
            Vector2 goalVel = (desiredPos - NPC.Center);
            float len = goalVel.Length();
            if (len > maxSpeed) goalVel = goalVel * (maxSpeed / len);
            NPC.velocity = Vector2.Lerp(NPC.velocity, goalVel, accel);

            NPC.spriteDirection = (NPC.Center.X < target.Center.X) ? 1 : -1;

            ref float state = ref NPC.ai[1];
            ref float timer = ref NPC.ai[0];
            timer++;

            bool telegraph = false;

            if (state == 0f)
            {
                float fireRate = 90f;
                if (timer % fireRate == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    FireBolt(target, 12f, spreadDegrees: 6f, damageScale: 1f);
                }
                if (timer > 600f)
                {
                    timer = 0f;
                    state = 1f;
                    NPC.netUpdate = true;
                }
            }
            else if (state == 1f)
            {
                telegraph = true;

                if (timer == 60f && Main.netMode != NetmodeID.MultiplayerClient)
                    FireSpreadVolley(target, bolts: 5, speed: 13f, spreadDegrees: 25f, damageScale: 1.1f);

                if (timer == 116f && Main.netMode != NetmodeID.MultiplayerClient)
                    FireSpreadVolley(target, bolts: 5, speed: 13.5f, spreadDegrees: 30f, damageScale: 1.1f);

                if (timer == 180f && Main.netMode != NetmodeID.MultiplayerClient)
                    FireSpreadVolley(target, bolts: 5, speed: 14f, spreadDegrees: 35f, damageScale: 1.15f);

                if (timer == 150f)
                {
                    Vector2 sidestep = dirToPlayer.RotatedBy(MathHelper.ToRadians(90f)) * 12f * (Main.rand.NextBool() ? 1 : -1);
                    NPC.velocity = Vector2.Lerp(NPC.velocity, sidestep, 0.7f);
                }

                if (timer > 240f)
                {
                    timer = 0f;
                    state = 0f;
                    NPC.netUpdate = true;
                }
            }

            if (telegraph && Main.rand.NextBool(4))
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GoldFlame, Scale: 1.1f);
                Main.dust[d].velocity *= 0.2f;
            }

            if (NPC.timeLeft < 1800) NPC.timeLeft = 1800;

            NPC.ShowNameOnHover = true;
            NPC.dontTakeDamageFromHostiles = true;
            NPC.netAlways = true;
        }

        private void FireBolt(Player target, float speed, float spreadDegrees, float damageScale)
        {
            Vector2 pos = NPC.Center;
            Vector2 dir = (target.Center - pos).SafeNormalize(Vector2.UnitX);
            float spread = MathHelper.ToRadians(spreadDegrees);
            dir = dir.RotatedBy(Main.rand.NextFloat(-spread, spread));

            int dmg = (int)(NPC.GetAttackDamage_ForProjectiles(35, 45) * damageScale);
            float knockback = 2f;

            Terraria.Projectile.NewProjectile(
                NPC.GetSource_FromAI(),
                pos,
                dir * speed,
                ModContent.ProjectileType<RaphaelProjectile>(),
                dmg,
                knockback,
                Main.myPlayer
            );

            SoundEngine.PlaySound(SoundID.Item20, pos);
        }

        private void FireSpreadVolley(Player target, int bolts, float speed, float spreadDegrees, float damageScale)
        {
            Vector2 pos = NPC.Center;
            Vector2 to = (target.Center - pos).SafeNormalize(Vector2.UnitX);
            float totalSpread = MathHelper.ToRadians(spreadDegrees);
            for (int i = 0; i < bolts; i++)
            {
                float t = bolts == 1 ? 0f : MathHelper.Lerp(-totalSpread, totalSpread, i / (float)(bolts - 1));
                Vector2 dir = to.RotatedBy(t);
                int dmg = (int)(NPC.GetAttackDamage_ForProjectiles(28, 38) * damageScale);

                Terraria.Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    pos,
                    dir * speed,
                    ModContent.ProjectileType<RaphaelProjectile>(),
                    dmg,
                    1.5f,
                    Main.myPlayer
                );
            }
            SoundEngine.PlaySound(SoundID.Item33, pos);
        }

        public override void FindFrame(int frameHeight) { }

        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.GoldCoin, Main.rand.Next(5, 11));
            }
        }

        [Obsolete]
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.SoulofLight, 1, 20, 40));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.TerrariaMode.NPCs.Raphael.Bestiary"))
            });
        }
    }
}
