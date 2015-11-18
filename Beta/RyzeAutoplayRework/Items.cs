using Autoplay;
using EloBuddy;
using EloBuddy.SDK;
using System.Collections.Generic;

class Items : Helper
{
    public static List<CItem> FighterItems = new List<CItem>()
        {
            new CItem(false, 1000, (int)ItemId.Berserkers_Greaves),
            new CItem(false, 2500, (int)ItemId.Statikk_Shiv),
            new CItem(false, 800, (int)ItemId.Vampiric_Scepter),
            new CItem(false, 1550, (int)ItemId.B_F_Sword),
            new CItem(false, 2000, (int)ItemId.The_Bloodthirster),
            new CItem(false, 875, (int)ItemId.Pickaxe),
            new CItem(false, 1550, (int)ItemId.B_F_Sword),
            new CItem(false, 1500, (int)ItemId.Infinity_Edge),
            new CItem(false, 2300, (int)ItemId.Last_Whisper),
            new CItem(false, 2800, (int)ItemId.Phantom_Dancer)
        };

    public static List<CItem> MageItems = new List<CItem>()
        {
            new CItem(false, 1100, (int)ItemId.Sorcerers_Shoes),
            new CItem(false, 1250, (int)ItemId.Needlessly_Large_Rod),
            new CItem(false, 1800, (int)ItemId.Ludens_Echo),
            new CItem(false, 820, (int)ItemId.Fiendish_Codex),
            new CItem(false, 1500, (int)ItemId.Morellonomicon),
            new CItem(false, 2500, (int)ItemId.Void_Staff),
            new CItem(false, 1250, (int)ItemId.Needlessly_Large_Rod),
            new CItem(false, 850, (int)ItemId.Blasting_Wand),
            new CItem(false, 1500, (int)ItemId.Rabadons_Deathcap),
            new CItem(false, 3500, (int)ItemId.Rylais_Crystal_Scepter)
        };

    public static List<CItem> RyzeItems = new List<CItem>()
        {
            new CItem(false, 400, (int)ItemId.Sapphire_Crystal),
            new CItem(false, 320, (int)ItemId.Tear_of_the_Goddess),
            new CItem(false, 1000, (int)ItemId.Ionian_Boots_of_Lucidity), //1
            new CItem(false, 1250, (int)ItemId.Needlessly_Large_Rod),
            new CItem(false, 1200, (int)ItemId.Archangels_Staff), //2
            new CItem(false, 1500, (int)ItemId.Frozen_Heart), //3
            new CItem(false, 1200, (int)ItemId.Catalyst_the_Protector),
            new CItem(false, 1500, (int)ItemId.Rod_of_Ages), //4
            new CItem(false, 3500, (int)ItemId.Void_Staff), //5
            new CItem(false, 1250, (int)ItemId.Needlessly_Large_Rod),
            new CItem(false, 850, (int)ItemId.Blasting_Wand),
            new CItem(false, 1500, (int)ItemId.Rabadons_Deathcap) //6
        };

    public static void BuyFighterItems()
    {
        for (int i = 0; i < FighterItems.Count; i++)
        {
            if (!Item.HasItem(FighterItems[i].ID, myHero) &&
                myHero.Gold >= FighterItems[i].Gold &&
                !FighterItems[i].Bought)
            {
                Item itm = new Item(FighterItems[i].ID);
                itm.Buy();
                FighterItems[i].Bought = true;
            }
        }
    }

    public static void BuyMageItems()
    {
        for (int i = 0; i < MageItems.Count; i++)
        {
            if (!Item.HasItem(MageItems[i].ID, myHero) &&
                myHero.Gold >= MageItems[i].Gold &&
                !MageItems[i].Bought)
            {
                Item itm = new Item(MageItems[i].ID);
                itm.Buy();
                MageItems[i].Bought = true;
            }
        }
    }

    public static void BuyRyzeItems()
    {
        for (int i = 0; i < RyzeItems.Count; i++)
        {
            if (!Item.HasItem(RyzeItems[i].ID, myHero) &&
                myHero.Gold >= RyzeItems[i].Gold &&
                !RyzeItems[i].Bought)
            {
                Item itm = new Item(RyzeItems[i].ID);
                itm.Buy();
                RyzeItems[i].Bought = true;
            }
        }
    }


    public class CItem
    {
        public float Gold;
        public int ID;
        public bool Bought;
        public CItem(bool bought, float g, int id)
        {
            this.Bought = bought;
            this.Gold = g;
            this.ID = id;
        }
    }//CItem
}