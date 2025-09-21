using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaMode.Content.NPCs;
using TerrariaMode.Content.Players;



namespace TerrariaMode.Content.Items
{
    public class CustomBossItem : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {

            return !NPC.AnyNPCs(ModContent.NPCType<CustomBoss>());
        }

        public override bool? UseItem(Player player)
{
    if (Main.netMode != NetmodeID.MultiplayerClient)
    {
       player.GetModPlayer<FadePlayer>().StartFadeOut();


    }
    return true;
}

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 1)     
                .AddTile(TileID.WorkBenches)            
                .Register();
        }
    }
}
