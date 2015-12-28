using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;
namespace Modes
{
    using System.Linq;
    using Menu = Settings.ComboSettings;
    class Combo : Helper
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(1200, DamageType.Physical);
            if (target == null) return;

            if (Menu.UsePassive && myHero.HasBuff("poppypassivebuff") && target.Health > myHero.GetAutoAttackDamage(target) + Lib.GetPassiveDamage(target))
            {
                var minions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(525)
                && myHero.GetAutoAttackDamage(x) + Lib.GetPassiveDamage(x) > x.Health);
                if (minions != null)
                {
                    if (Orbwalker.CanAutoAttack)
                    {
                        Player.IssueOrder(GameObjectOrder.AttackTo, minions);
                    }
                }
            }

            if (Menu.UseW)
            {
                if (Menu.Whealth)
                {
                    if (myHero.HealthPercent <= 40)
                        Lib.W.Cast();
                }
                else
                    Lib.W.Cast();
            }

            if (Menu.UseQ && Lib.Q.IsReady() && target.IsValidTarget(Lib.Q.Range) && Lib.Q.GetPrediction(target).HitChance >= EloBuddy.SDK.Enumerations.HitChance.Medium)
            {
                Lib.Q.Cast(Lib.Q.GetPrediction(target).CastPosition);
            }
            else if (Menu.UseE && Lib.E.IsReady() && target.IsValidTarget(Lib.E.Range))
            {
                if (Program.rectangle != null)
                {
                    if (!Program.rectangle.IsInside(target))
                    {
                        CastE(target);
                    }
                }
                else
                {
                    CastE(target);
                }
            }

            if (Menu.UseR)
            {
                if (Lib.R.IsReady())
                {
                    if (GetComboDamage(target) < target.Health && !target.HasBuffOfType(BuffType.SpellImmunity))
                    {
                        if (target.IsFacing(myHero) ? myHero.IsInRange(target, Lib.R.MinimumRange - 30) : myHero.IsInRange(target, Lib.R.MinimumRange - 100))
                        {
                            Lib.R.StartCharging();
                            myHero.Spellbook.UpdateChargeableSpell(SpellSlot.R, target.ServerPosition, true);
                        }
                    }
                }
            }
        }

        private static void CastE(AIHeroClient target)
        {
            var turret = EntityManager.Turrets.Allies.Where(x => !x.IsDead && x.Distance(myHero) < 1200).OrderBy(x => x.Distance(myHero));
            var push = target.ServerPosition.Extend(myHero, -300);
            if (
                (Menu.UseEStun && Lib.CanStun(target)) ||
                (Menu.UseEPassive && Lib.Passive != null && push.Distance(Lib.Passive) < myHero.Distance(Lib.Passive) - 100) ||
                (Menu.UseEInsec && turret.Count() > 0 && push.Distance(turret.First()) < target.Distance(turret.First()) && turret.First().IsInRange(push, 1050))
                )
            {
                Lib.E.Cast(target);
            }

            foreach (var ally in EntityManager.Heroes.Allies.Where(x => !x.IsDead && !x.IsMe && !x.IsInAutoAttackRange(target) && x.Distance(myHero) < 1200))
            {
                if (Menu.UseEInsec && push.Distance(ally) < myHero.Distance(ally) - 100)
                {
                    Lib.E.Cast(target);
                }
                else if (Menu.UseFlashE)
                {
                    if (Lib.Flash != null)
                    {
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
    }
}



