using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using KappaLeBlanc;
namespace Modes
{
    class Killsteal : Helper
    {
        public static void Execute()
        {
            if (CastSlider(LBMenu.Misc, "AutoW") > myHero.HealthPercent)
            {
                if (Lib.W.Name == "leblancslidereturn")
                {
                    myHero.Spellbook.CastSpell(SpellSlot.W);
                }
            }
            var target = TargetSelector.GetTarget(Lib.W.Range * 2 + Lib.Q.Range, DamageType.Magical);
            if (target == null) return;
            if (target.HasBuffOfType(BuffType.Invulnerability)) return;

            var WReady = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name != "leblancslidereturn" && Lib.W.IsReady();
            var ksm = LBMenu.KSM;

            var QDmg = myHero.GetSpellDamage(target, SpellSlot.Q);
            var WDmg = myHero.GetSpellDamage(target, SpellSlot.W);
            var EDmg = myHero.GetSpellDamage(target, SpellSlot.E);
            var RDmg = myHero.GetSpellDamage(target, SpellSlot.R);

            if (Lib.R.IsReady() && CastCheckbox(ksm, "R") && Lib.R.IsInRange(target))
            {
                if (QDmg < target.Health || !Lib.Q.IsReady())
                {
                    if (RDmg > target.Health)
                    {
                        Lib.CastR(target);
                    }
                }
            }
            if (QDmg + WDmg + EDmg > target.Health)
            {
                var epred = Lib.E.GetPrediction(target);
                if (epred.HitChance >= HitChance.Medium)
                {
                    if (Lib.Q.IsInRange(target) && CastCheckbox(ksm, "Q"))
                    {
                        Lib.Q.Cast(target);
                    }
                    if (Lib.W.IsInRange(target) && CastCheckbox(ksm, "W"))
                    {
                        Lib.CastW(target);
                    }
                    if (Lib.E.IsInRange(target) && CastCheckbox(ksm, "E"))
                    {
                        Lib.E.Cast(epred.CastPosition);
                    }
                }
            }

            if (QDmg > target.Health && CastCheckbox(ksm, "Q"))
            {
                if (Lib.Q.IsReady() && Lib.Q.IsInRange(target))
                {
                    Lib.Q.Cast(target);
                }
                else if (Lib.W.Range + Lib.Q.Range > myHero.Distance(target))
                {
                    if (WReady)
                    {
                        var wpos = myHero.Position.Extend(target, Lib.W.Range).To3D();
                        if (CastCheckbox(ksm, "W") && CastCheckbox(ksm, "extW"))
                        {
                            if (Lib.Q.IsReady())
                            {
                                Lib.CastW(wpos);
                            }
                        }
                    }
                }
            }
            else if (Lib.W.Range * 2 + Lib.Q.Range > myHero.Distance(target) && QDmg > target.Health && Lib.Q.IsReady() && WReady && Lib.R.IsReady() && Lib.R.Name == "LeblancSlideM")
            {
                if (Lib.Q.IsInRange(target))
                {
                    Lib.Q.Cast(target);
                }
                var wpos = myHero.Position.Extend(target, Lib.W.Range).To3D();
                if (CastCheckbox(ksm, "W") && CastCheckbox(ksm, "wr"))
                {
                    Lib.CastW(wpos);

                    if (!Lib.Q.IsInRange(target))
                    {
                        Lib.CastR(wpos);
                    }

                }
            }

            else if (Lib.E.IsReady() && CastCheckbox(ksm, "E"))
            {
                if (EDmg > target.Health)
                {
                    var epred = Lib.E.GetPrediction(target);

                    if (Lib.E.IsInRange(target) && epred.HitChance >= HitChance.High)
                    {
                        Lib.E.Cast(epred.CastPosition);
                    }
                    else if (WReady && CastCheckbox(ksm, "W") && CastCheckbox(ksm, "extW"))
                    {
                        if (Lib.W.Range + Lib.E.Range > myHero.Distance(target))
                        {
                            var wpos = myHero.Position.Extend(target, Lib.W.Range).To3D();
                            Lib.CastW(wpos);
                        }
                    }
                }
            }
            else if (Lib.W.IsReady() && Lib.W.Name != "leblancslidereturn" && CastCheckbox(ksm, "W"))
            {
                if (WDmg > target.Health)
                {
                    if (Lib.W.IsInRange(target))
                    {
                        Lib.CastW(target);
                    }
                }
            }
        }
    }
}

