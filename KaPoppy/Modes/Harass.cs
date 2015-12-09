using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;
namespace Modes
{
    using Menu = Settings.HarassSettings;
    class Harass : Helper
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(Lib.E.Range, DamageType.Physical);
            if (target == null) return;
            if (!Orbwalker.IsAutoAttacking)
            {
                if (Menu.UseQ)
                {
                    if (Lib.Q.IsReady())
                    {
                        if (target.IsValidTarget(Lib.Q.Range))
                        {
                            var pred = Lib.Q.GetPrediction(target);
                            if (pred.HitChance >= EloBuddy.SDK.Enumerations.HitChance.Medium)
                                Lib.Q.Cast(pred.CastPosition);
                        }
                    }
                }

                if (Menu.UseE)
                {
                    if (Lib.E.IsReady())
                    {
                        if (target.IsValidTarget(Lib.E.Range))
                        {
                            if (!Menu.UseEs)
                            {
                                Lib.E.Cast(target);
                            }
                            else if (Lib.CanStun(target))
                            {
                                Lib.E.Cast(target);
                            }
                        }
                    }
                }
            }
            if (Menu.UseW)
            {
                if (Menu.Whealth)
                    if (myHero.HealthPercent >= 40)
                        return;

                Lib.W.Cast();
            }
        }
    }
}