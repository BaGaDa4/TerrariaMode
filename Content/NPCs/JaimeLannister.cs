using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using TerrariaMode.Content.Items;

namespace TerrariaMode.Content.NPCs
{

    public class MyPlayer : ModPlayer
    {
        private bool spawnedMerchant = false;
        public override void OnEnterWorld()
        {
            bool alreadyExists = NPC.AnyNPCs(ModContent.NPCType<JaimeLannister>());
            if (!alreadyExists)
            {
                Vector2 spawnPos = Player.Center + new Vector2(60, 0);
                NPC.NewNPC(
                    NPC.GetSource_NaturalSpawn(),
                    (int)spawnPos.X,
                    (int)spawnPos.Y,
                    ModContent.NPCType<JaimeLannister>()
                );
            }
        }

    }

    public class JaimeLannister : ModNPC
    {
        public override string Texture => "Terraria/Images/NPC_" + NPCID.Merchant;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Merchant];
        }

        public override string GetChat()
        {
            int choice = Main.rand.Next(3);
            switch (choice)
            {
                case 0:
                    return Language.GetTextValue("Mods.TerrariaMode.NPC.JaimeLannister.Chat1");
                case 1:
                    return Language.GetTextValue("Mods.TerrariaMode.NPC.JaimeLannister.Chat2");
                default:
                    return Language.GetTextValue("Mods.TerrariaMode.NPC.JaimeLannister.Chat3");
            }
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Merchant);
            NPC.damage = 0;
            NPC.defense = 15;
            NPC.lifeMax = 300;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = 7;
            NPC.friendly = true;
            NPC.townNPC = true;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("Mods.TerrariaMode.NPC.JaimeLannister.ShopButton");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = "Shop";
            }
        }

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, "Shop")
                .Add(ItemID.Sake)
                .Add(ItemID.Feather)
                .Add(ItemID.MiningHelmet)

                .Add(ItemID.HealingPotion)
                .Add(ItemID.ManaPotion)
                .Add(ItemID.IronskinPotion)
                .Add(ItemID.SwiftnessPotion)
                .Add(ItemID.RegenerationPotion)
                .Add(ItemID.EndurancePotion)
                .Add(ItemID.MagicPowerPotion)
                .Add(ItemID.RagePotion)
                .Add(ItemID.WrathPotion)
                .Add(ItemID.LifeforcePotion)
                .Add(ItemID.InvisibilityPotion)
                .Add(ItemID.ShinePotion)
                .Add(ItemID.NightOwlPotion)

                .Add(ItemID.Blinkroot)
                .Add(ItemID.Deathweed)
                .Add(ItemID.Fireblossom)
                .Add(ItemID.Waterleaf)
                .Add(ItemID.Moonglow)
                .Add(ItemID.Daybloom)
                .Add(ItemID.BottledWater)
                .Add(ItemID.AlchemyTable)

                .Add(ModContent.ItemType<DawnSword>());
            npcShop.Register();
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return true;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;

            if (NPC.velocity.Y != 0f)
            {
                NPC.frame.Y = frameHeight * 2;
            }
            else if (NPC.velocity.X != 0f)
            {
                NPC.frameCounter++;
                if (NPC.frameCounter >= 6)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y += frameHeight;
                    if (NPC.frame.Y >= frameHeight * 6)
                        NPC.frame.Y = 0;
                }
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }
    }
}
