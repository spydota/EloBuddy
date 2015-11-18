using EloBuddy;
using EloBuddy.SDK;
using Autoplay;
using EloBuddy.SDK.Enumerations;
using System.Linq;

namespace Plugins
{
    public class Ryze : Helper
    {
        private static Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1700, 100);
        private static Spell.Targeted W = new Spell.Targeted(SpellSlot.W, 600);
        private static Spell.Targeted E = new Spell.Targeted(SpellSlot.E, 600);
        private static Spell.Active R = new Spell.Active(SpellSlot.R);

        public static void Init()
        {
            Chat.Print(Player.Instance.ChampionName + "Loaded");
        }
        public static void Farm()
        {

            var minion = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.Name.ToLower().Contains("minion") && x.IsValidTarget(Q.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (minion == null || !minion.IsValid || Orbwalker.IsAutoAttacking) return;
            bool Pasive = myHero.HasBuff("ryzepassivecharged");
            if (Orbwalker.CanAutoAttack && myHero.IsInAutoAttackRange(minion))
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
            }
            if (!Pasive)
            {
                if (Q.IsReady() && myHero.ManaPercent >= 15)
                {
                    Q.Cast(minion);
                }
                if (!Q.IsReady() && E.IsReady() && myHero.ManaPercent >= 50)
                {
                    E.Cast(minion);
                }
                if (!E.IsReady() && W.IsReady() && myHero.ManaPercent >= 50)
                {
                    W.Cast(minion);
                }
                if (R.IsReady()  && myHero.ManaPercent >= 50)
                {
                    if (Pasive || myHero.GetBuffCount("ryzepassivestack") == 4 && !Q.IsReady() | !W.IsReady() | !E.IsReady())
                    {
                        R.Cast();
                    }
                }
            }
            if (Pasive)
            {
                if (Q.IsReady() && myHero.ManaPercent >= 15)
                {
                    Q.Cast(minion);
                }
                if (!Q.IsReady() && E.IsReady() && myHero.ManaPercent >= 40)
                {
                    E.Cast(minion);
                }
                if (!E.IsReady() && W.IsReady() && myHero.ManaPercent >= 40)
                {
                    W.Cast(minion);
                }
                if (R.IsReady() && myHero.ManaPercent >= 50)
                {
                    R.Cast();
                }
            }
        }
        public static void Harass()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);
            if (target != null)
            {
                var qpred = Q.GetPrediction(target);
                if (Q.IsReady())
                {
                    Q.Cast(target);
                }
                if (myHero.ManaPercent > 70)
                {
                    if (E.IsReady() && E.IsInRange(target))
                    {
                        E.Cast(target);
                    }
                }
            }
        }
        public static void Combo()
        {
            //Lazy ;-;

            var target = TargetSelector.GetTarget(900, DamageType.Magical);
            if (target == null) return;

            var QDmg = myHero.GetSpellDamage(target, SpellSlot.Q);
            var WDmg = myHero.GetSpellDamage(target, SpellSlot.W);
            var EDmg = myHero.GetSpellDamage(target, SpellSlot.E);
            
            var Stacks = myHero.GetBuffCount("ryzepassivestack");
            if (QDmg + WDmg + EDmg > target.Health && (target.CountEnemiesInRange(500) <= 3 || Stacks >= 4) && target.IsValidTarget(900) && !target.IsDead && target.IsVisible)
            {
                ComboPLS = true;
               Core.DelayAction(() =>ComboPLS = false, 5000);
            } 
            else if(ComboPLS) { ComboPLS = false; }
            var QPred = Q.GetPrediction(target);
            bool StacksBuff = myHero.HasBuff("ryzepassivestack");
            bool Pasive = myHero.HasBuff("ryzepassivecharged");
            if (target.IsValidTarget(Q.Range))
            {
                if (Stacks <= 2 || !StacksBuff)
                {
                    if (target.IsValidTarget(Q.Range) && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(W.Range) && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (target.IsValidTarget(E.Range) && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (R.IsReady())
                    {
                        if (target.IsValidTarget(W.Range)
                                && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            R.Cast();
                        }
                    }
                }
                if (Stacks == 3)
                {
                    if (target.IsValidTarget(Q.Range) && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(E.Range) && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (target.IsValidTarget(W.Range) && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (R.IsReady())
                    {
                        if (target.IsValidTarget(W.Range)
                                && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            if (!E.IsReady() && !Q.IsReady() && !W.IsReady())
                                R.Cast();
                        }
                    }
                }
                if (Stacks == 4)
                {
                    if (target.IsValidTarget(W.Range) && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (target.IsValidTarget(Q.Range) && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(E.Range) && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (R.IsReady())
                    {
                        if (target.IsValidTarget(W.Range)
                                && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            R.Cast();
                        }
                    }
                }
                if (Pasive)
                {
                    if (target.IsValidTarget(W.Range) && W.IsReady())
                    {
                        W.Cast(target);
                    }
                    if (target.IsValidTarget(Q.Range) && Q.IsReady())
                    {
                        Q.Cast(QPred.UnitPosition);
                    }
                    if (target.IsValidTarget(E.Range) && E.IsReady())
                    {
                        E.Cast(target);
                    }
                    if (R.IsReady())
                    {
                        if (target.IsValidTarget(W.Range)
                            && target.Health > (myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.E)))
                        {
                            R.Cast();
                        }
                    }
                }
            }
            else
            {
                if (W.IsReady()
                    && target.IsValidTarget(W.Range))
                    W.Cast(target);

                if (Q.IsReady()
                    && target.IsValidTarget(Q.Range))
                    Q.Cast(QPred.UnitPosition);

                if (E.IsReady()
                    && target.IsValidTarget(E.Range))
                    E.Cast(target);
            }
            if (!R.IsReady() || Stacks != 4) return;

            if (Q.IsReady() || W.IsReady() || E.IsReady()) return;

            R.Cast();
        }

    }
}
