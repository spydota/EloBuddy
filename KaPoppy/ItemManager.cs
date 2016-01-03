using System;
using EloBuddy;
using EloBuddy.SDK;

namespace KaPoppy
{
    using Menu = Settings.ItemsSettings;
    internal class ItemManager
    {
        public static Item Hydra, Tiamat;
        internal static void Init()
        {

            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Menu.UseHydra("Combo"))
                {
                    if (HasHydra())
                    {
                        CastHydra();
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (Menu.UseHydra("Harass"))
                {
                    if (HasHydra())
                    {
                        CastHydra();
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (Menu.UseHydra("Laneclear"))
                {
                    if (HasHydra())
                    {
                        CastHydra();
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                if (Menu.UseHydra("Jungleclear"))
                {
                    if (HasHydra())
                    {
                        CastHydra();
                    }
                }
            }
        }

        public static bool HasHydra()
        {
            return Item.CanUseItem(3077) || Item.CanUseItem(3074) || Item.CanUseItem(3748);
        }
        public static void CastHydra()
        {
            Item.UseItem(3077);
            Item.UseItem(3074);
            Item.UseItem(3748);
        }
    }
}