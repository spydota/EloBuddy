using EloBuddy;
using EloBuddy.SDK;
using System;

namespace RyzeAutoplay
{
    class Items
    {
        public static Item SapphireCrystal, Tear, NeedlesslyLargeRod, ArchangelsStaff, RubyCrystal, Catalyst, BlastingWand, ROA, SeraphEmbrace, Boots, MercuryTreads, trinket;
        public void Init()
        {
            trinket = new Item(ItemId.Warding_Totem_Trinket);
            Boots = new Item(ItemId.Boots_of_Speed);
            MercuryTreads = new Item(ItemId.Mercurys_Treads);
            SapphireCrystal = new Item(ItemId.Sapphire_Crystal);
            RubyCrystal = new Item(ItemId.Ruby_Crystal);
            SapphireCrystal = new Item(ItemId.Sapphire_Crystal);
            BlastingWand = new Item(ItemId.Blasting_Wand);
            ROA = new Item(ItemId.Rod_of_Ages);
            Catalyst = new Item(ItemId.Catalyst_the_Protector);
            ArchangelsStaff = new Item(ItemId.Archangels_Staff);
            Tear = new Item(ItemId.Tear_of_the_Goddess);
            NeedlesslyLargeRod = new Item(ItemId.Needlessly_Large_Rod);
            SeraphEmbrace = new Item(3040);

            Game.OnUpdate += ShopManager;
        }

        private void ShopManager(EventArgs args)
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
        }
    }
}
