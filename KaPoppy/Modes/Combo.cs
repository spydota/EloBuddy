using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;
namespace Modes
{
    using System;
    using System.Linq;
    using Menu = Settings.ComboSettings;
    class Combo : Helper
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(1200, DamageType.Physical);
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
                    if (target.IsValidTarget(Lib.E.Range) && Lib.E.IsReady())
                    {
                        if (!Menu.UseEs && myHero.Distance(target) > myHero.GetAutoAttackRange())
                        {
                            Lib.E.Cast(target);
                        }
                        else
                        {
                            if (Lib.CanStun(target))
                            {
                                Lib.E.Cast(target);
                            }
                            else if (Menu.UseFlashE)
                            {
                                if (Lib.Flash == null) return;
                                var flashpos = Lib.PointsAroundTheTarget(target, 525).Where(x => Lib.CanStun(target, x.To2D()) && !IsWall(x)).OrderBy(x => x.Distance(myHero));

                                if (flashpos.Count() > 0)
                                {
                                    if (Lib.Flash.IsReady() && Lib.Flash.IsInRange(flashpos.First()))
                                    {
                                        Lib.Flash.Cast(flashpos.First());
                                    }
                                }
                            }
                        }
                    }
                }
               /* if (Menu.UseR)
                {
                    if (Lib.R.IsReady())
                    {
                        if (myHero.CountEnemiesInRange(Lib.R.MinimumRange) >= Menu.RMin)
                        {               
                            Lib.R.StartCharging();
                            Core.DelayAction(() =>
                            Lib.R.Cast(target), 10
                            );
                        }
                    }
                }*/
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

