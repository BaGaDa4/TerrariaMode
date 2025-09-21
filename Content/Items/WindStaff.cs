using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TerrariaMode.Content.Projectiles;

namespace TerrariaMode.Content.Items
{
    public class WindStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 50;                        
            Item.DamageType = DamageClass.Magic;     
            Item.width = 40;                         
            Item.height = 40;                        
            Item.useTime = 25;                        
            Item.useAnimation = 25;                  
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;                     
            Item.knockBack = 2;                     
            Item.value = Item.buyPrice(silver: 10);  
            Item.rare = ItemRarityID.Green;          
            Item.UseSound = SoundID.Item20;          
            Item.autoReuse = true;                   
            Item.useStyle = ItemUseStyleID.Swing; 
            Item.noUseGraphic = true;           


           
            Item.shoot = ModContent.ProjectileType<WindStaffProjectile>();
            Item.shootSpeed = 10f;                   
            Item.mana = 1;                          
        }

public override Vector2? HoldoutOffset()
{
    return new Vector2(-2f, 0f); 
}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
