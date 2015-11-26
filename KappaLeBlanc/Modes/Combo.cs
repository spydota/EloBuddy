using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using KappaLeBlanc;
namespace Modes
{
    class Combo : Helper
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(Lib.W.Range * 2, DamageType.Magical);
            if (target != null)
            {
                var ComboM = LBMenu.ComboM;
                var WReady = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name != "leblancslidereturn" && Lib.W.IsReady();

                if (CastCheckbox(ComboM, "Q"))
                {
                    if (Lib.Q.IsReady())
                    {
                        if (WReady || Lib.E.IsReady() || Lib.R.IsReady() || target.HasBuff("LeblancSoulShackle"))
                        {
                            if (target.IsValidTarget(Lib.Q.Range))
                            {
                                Lib.Q.Cast(target);
                            }
                        }
                    }
                }
                if (CastCheckbox(ComboM, "W"))
                {
                    if (CastCheckbox(ComboM, "extW") && myHero.Distance(target) > Lib.Q.Range)
                    {
                        if (WReady)
                        {
                            var wpos = myHero.Position.Extend(target, Lib.W.Range).To3D();
                            if (Lib.Q.IsReady() && CastCheckbox(ComboM, "Q"))
                            {
                                if (Lib.Q.Range + Lib.W.Range > myHero.Distance(target))
                                {
                                    Lib.CastW(wpos);
                                }
                            }
                            else if (Lib.E.IsReady() && CastCheckbox(ComboM, "E"))
                            {
                                if (Lib.E.Range + Lib.W.Range > myHero.Distance(target))
                                {
                                    Lib.CastW(wpos);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (WReady)
                        {
                            if (Lib.W.IsInRange(target))
                            {
                                if (target.HasBuff("LeblancChaosOrb") || target.HasBuff("LeblancSoulShackle"))
                                {
                                    Lib.CastW(target);
                                }
                            }
                        }
                    }
                }
                if (CastCheckbox(ComboM, "E"))
                {
                    if (Lib.E.IsReady())
                    {
                        var epred = Lib.E.GetPrediction(target);

                        if (epred.HitChance >= HitChance.Medium)
                        {
                            Lib.E.Cast(epred.CastPosition);
                        }
                    }
                }
                if (CastCheckbox(ComboM, "R"))
                {
                    if (Lib.R.IsReady())
                    {
                        if (CastCheckbox(ComboM, "RQ"))
                        {
                            if (Lib.R.Name == "LeblancChaosOrbM") // Q
                            {
                                if (Lib.Q.IsReady() || WReady || Lib.E.IsReady() || target.HasBuff("LeblancSoulShackle") || target.HasBuff("LeblancChaosOrb"))
                                {
                                    Lib.CastR(target);
                                }
                            }
                        }
                        if (CastCheckbox(ComboM, "RW"))
                        {
                            if (Lib.R.Name == "LeblancSlideM") // W
                            {
                                if (target.CountEnemiesInRange(Lib.W.Width) > 1)
                                {
                                    Lib.CastR(target);
                                }
                                else if (!Lib.Q.IsReady() && !Lib.E.IsReady())
                                {
                                    Lib.CastR(target);
                                }
                            }
                        }
                        if (CastCheckbox(ComboM, "RE"))
                        {
                            if (Lib.R.Name == "LeblancSoulShackleM") // E
                            {
                                if (Lib.Q.IsReady() || Lib.E.IsReady())
                                {
                                    Lib.CastR(target);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
