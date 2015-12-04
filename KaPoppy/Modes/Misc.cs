using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;

namespace Modes
{
    using System;
    using System.Linq;
    using DashSpells;
    using Menu = Settings.MiscSettings;
    class Misc : Helper
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(Lib.R.MaximumRange, DamageType.Physical);
            if (target == null) return;

            var QDMG = myHero.GetSpellDamage(target, SpellSlot.Q);
            var EDMG = myHero.GetSpellDamage(target, SpellSlot.E);
            var RDMG = myHero.GetSpellDamage(target, SpellSlot.R);

            if (Menu.UseQ && QDMG > target.Health && Lib.Q.IsReady() && target.IsValidTarget(Lib.Q.Range))
            {
                Lib.Q.Cast(target);
            }
            else if (Menu.UseE && EDMG > target.Health && Lib.E.IsReady() && target.IsValidTarget(Lib.E.Range))
            {
                Lib.E.Cast(target);
            }
            else if (Menu.UseR && RDMG > target.Health && Lib.R.IsReady() && target.IsValidTarget(Lib.R.MaximumRange))
            {
                if (!Lib.R.IsCharging)
                    Lib.R.StartCharging();
                else
                {
                    if (Lib.R.Range == Lib.R.MaximumRange)
                    {
                        Lib.R.Cast(target);
                    }
                    else if (target.IsValidTarget(Lib.R.Range))
                    {
                        var pred = Lib.R.GetPrediction(target);
                        if(pred.HitChance >= EloBuddy.SDK.Enumerations.HitChance.High)
                        {
                            Lib.R.Cast(target);
                        }
                    }
                }
            }
        }

        internal static void AntiRengar(GameObject sender, EventArgs args)
        {
            if (sender.IsAlly || Lib.W.IsReady()) return;
            AIHeroClient rengo = EntityManager.Heroes.Enemies.First(x => x.Hero == Champion.Rengar);
            if (rengo != null)
            {
                if (Menu.AntiRengo)
                {
                    if (sender.Name == "Rengar_LeapSound.troy")
                    {
                        if (rengo.IsValidTarget(Lib.W.Range))
                        {
                            Lib.W.Cast();
                        }
                    }
                }
            }
        }

        internal static void SpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!Lib.W.IsReady() || !Menu.AntiGapcloser) return;
            if (sender == null || sender.IsAlly) return;
            var enemy = (AIHeroClient)sender;

            if (Lib.W.IsInRange(args.End))
            {
                foreach (var dash in DashSpells.Dashes)
                {
                    if (enemy.Hero == dash.champ)
                    {
                        if (args.Slot == dash.spellKey)
                        {
                            if (Menu.WEnabled(dash.champ, dash.spellKey))
                            {
                                if (dash.spellname == string.Empty)
                                {
                                    Lib.W.Cast();
                                }
                                else
                                {
                                    if (args.SData.Name == dash.spellname)
                                    {
                                        Lib.W.Cast();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}



