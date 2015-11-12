using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using KappaLeBlanc;
namespace Modes
{
    class Harass : Helper
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(Lib.W.Range * 2, DamageType.Magical);
            if (target == null) return;
            var _Q = CastCheckbox(LBMenu.HSM, "Q") && Lib.Q.IsReady(); var manaQ = CastSlider(LBMenu.HSM, "QMana") < myHero.ManaPercent;
            var _W = CastCheckbox(LBMenu.HSM, "W") && Lib.W.IsReady(); var manaW = CastSlider(LBMenu.HSM, "WMana") < myHero.ManaPercent;
            var _E = CastCheckbox(LBMenu.HSM, "E") && Lib.E.IsReady(); var manaE = CastSlider(LBMenu.HSM, "EMana") < myHero.ManaPercent;
            var extW = CastCheckbox(LBMenu.HSM, "extW");

            if (CastCheckbox(LBMenu.HSM, "AutoW"))
            {
                if (Lib.W.Name == "leblancslidereturn" && !Lib.Q.IsReady() && !Lib.E.IsReady())
                {
                    myHero.Spellbook.CastSpell(SpellSlot.W);
                }
            }

            if (_W && extW && manaW)
            {
                var wpos = myHero.Position.Extend(target, Lib.W.Range).To3D();
                if (_Q && manaQ)
                {
                    if (Lib.W.Range + Lib.Q.Range - 10 > myHero.Distance(target))
                    {
                        Lib.CastW(wpos);

                        if (Lib.Q.IsInRange(target))
                        {
                            Lib.Q.Cast(target);
                        }
                    }
                }
                if (_E && manaE)
                {
                    if (Lib.W.Range + Lib.E.Range - 10 > myHero.Distance(target))
                    {
                        Lib.CastW(wpos);

                        if (Lib.E.IsInRange(target))
                        {
                            var epred = Lib.E.GetPrediction(target);
                            if (epred.HitChance >= HitChance.Medium)
                            {
                                Lib.E.Cast(epred.CastPosition);
                            }
                        }
                    }
                }
            }
            if (_Q && manaQ)
            {
                if (Lib.Q.IsInRange(target))
                {
                    Lib.Q.Cast(target);
                }
            }
            if (_W && manaW)
            {
                var wpred = Lib.W.GetPrediction(target);
                Lib.W.Cast(wpred.CastPosition);
            }
            if (_E && manaE)
            {
                var epred = Lib.E.GetPrediction(target);
                if (epred.HitChance >= HitChance.Medium)
                {
                    Lib.E.Cast(epred.CastPosition);
                }
            }
        }
    }
}