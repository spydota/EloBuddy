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
            Hydra = new Item(ItemId.Ravenous_Hydra_Melee_Only, 250);
            Tiamat = new Item(ItemId.Tiamat_Melee_Only, 250);

            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Menu.UseHydra("Combo"))
                {
                    if (Hydra.IsOwned())
                    {
                        if (Hydra.IsInRange(target.Position))
                        {
                            Hydra.Cast();
                        }
                    }
                    else if (Tiamat.IsOwned())
                    {
                        if (Tiamat.IsInRange(target.Position))
                        {
                            Tiamat.Cast();
                        }
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                if (Menu.UseHydra("Harass"))
                {
                    if (Hydra.IsOwned())
                    {
                        if (Hydra.IsInRange(target.Position))
                        {
                            Hydra.Cast();
                        }
                    }
                    else if (Tiamat.IsOwned())
                    {
                        if (Tiamat.IsInRange(target.Position))
                        {
                            Tiamat.Cast();
                        }
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                if (Menu.UseHydra("Laneclear"))
                {
                    if (Hydra.IsOwned())
                    {
                        if (Hydra.IsInRange(target.Position))
                        {
                            Hydra.Cast();
                        }
                    }
                    else if (Tiamat.IsOwned())
                    {
                        if (Tiamat.IsInRange(target.Position))
                        {
                            Tiamat.Cast();
                        }
                    }
                }
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                if (Menu.UseHydra("Jungleclear"))
                {
                    if (Hydra.IsOwned())
                    {
                        if (Hydra.IsInRange(target.Position))
                        {
                            Hydra.Cast();
                        }
                    }
                    else if (Tiamat.IsOwned())
                    {
                        if (Tiamat.IsInRange(target.Position))
                        {
                            Tiamat.Cast();
                        }
                    }
                }
            }
        }
    }
}