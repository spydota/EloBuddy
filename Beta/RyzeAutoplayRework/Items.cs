using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Collections.Generic;

namespace Autoplay
{
    class Items
    {
       /* private void ShopManager(EventArgs args)
        {
            if (Player.Instance.IsInShopRange() || Player.Instance.IsDead)
            {
                var Gold = Player.Instance.Gold;
                if (ROA.IsOwned())
                {
                    if (Gold >= 400 && !SapphireCrystal.IsOwned() && !Catalyst.IsOwned())
                    {
                        SapphireCrystal.Buy();
                    }
                    if (Gold >= 400 && !RubyCrystal.IsOwned() && !Catalyst.IsOwned())
                    {
                        RubyCrystal.Buy();
                    }
                    if (Gold >= 400 && !Catalyst.IsOwned() && SapphireCrystal.IsOwned() && RubyCrystal.IsOwned())
                    {
                        Catalyst.Buy();
                    }
                    if (Gold >= 850 && !BlastingWand.IsOwned() && Catalyst.IsOwned())
                    {
                        BlastingWand.Buy();
                    }
                    if (Gold >= 650 && BlastingWand.IsOwned() && Catalyst.IsOwned())
                    { ROA.Buy(); }
                }
                if (!trinket.IsOwned()) { trinket.Buy(); }
                if (Gold >= 475 && !SapphireCrystal.IsOwned() && !Tear.IsOwned() && !ArchangelsStaff.IsOwned())
                {
                    SapphireCrystal.Buy();
                }
                if (Gold >= 320 && !Tear.IsOwned() && !ArchangelsStaff.IsOwned() && SapphireCrystal.IsOwned())
                {
                    Tear.Buy();
                }
                if (Gold >= 400 && !Catalyst.IsOwned() && !RubyCrystal.IsOwned())
                {
                    RubyCrystal.Buy();
                }
                if (Gold >= 800 && !Catalyst.IsOwned() && RubyCrystal.IsOwned())
                {
                    Catalyst.Buy();
                }
                if (Gold >= 1250 && !ArchangelsStaff.IsOwned() && Tear.IsOwned() && !NeedlesslyLargeRod.IsOwned())
                {
                    NeedlesslyLargeRod.Buy();
                }
                if (Gold >= 1030 && !ArchangelsStaff.IsOwned() && Tear.IsOwned() && NeedlesslyLargeRod.IsOwned())
                {
                    ArchangelsStaff.Buy();
                }
                if (Gold >= 850 && !BlastingWand.IsOwned() && !ROA.IsOwned())
                {
                    BlastingWand.Buy();
                }
                if (Gold >= 650 && BlastingWand.IsOwned() && Catalyst.IsOwned() && !ROA.IsOwned())
                {
                    ROA.Buy();
                }
                if (Gold >= 325 && !Boots.IsOwned() && !MercuryTreads.IsOwned())
                {
                    Boots.Buy();
                }
                if (Gold >= 875 && Boots.IsOwned() && !MercuryTreads.IsOwned())
                {
                    MercuryTreads.Buy();
                }
            }
        } */
        //Credits CT-SummonersRift for BanSharp
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

        public static void BuyRyzeItems()
        {
            for (int i = 0; i < RyzeItems.Count; i++)
            {
                if (!Item.HasItem(RyzeItems[i].ID, Player.Instance) &&
                   ObjectManager.Player.Gold >= RyzeItems[i].Gold &&
                    !RyzeItems[i].Bought)
                {
                    Item itm = new Item(RyzeItems[i].ID);
                    itm.Buy();
                    RyzeItems[i].Bought = true;
                }
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
    }
}