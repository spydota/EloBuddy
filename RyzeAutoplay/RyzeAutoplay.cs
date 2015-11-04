using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Linq;

namespace RyzeAutoplay
{
    class Program
    {
        //Made for own use
        public static Spell.Active Heal;
        public static bool kill { get { return RyzeMenu.Agressive["kill"].Cast<CheckBox>().CurrentValue; } }
        public static Spell.Active Clarity;
        public static AIHeroClient myHero { get { return ObjectManager.Player; } }
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        private static readonly Vector3 Toplane = new Vector3(1117, 10878, 53);
        public static int sliderdist { get { return RyzeMenu.Menu["sliderdist"].Cast<Slider>().CurrentValue; } }
        public static bool QLaneclear { get { return RyzeMenu.Laneclear["QLaneclear"].Cast<CheckBox>().CurrentValue; } }
        public static int QSlider { get { return RyzeMenu.Laneclear["QSlider"].Cast<Slider>().CurrentValue; } }
        public static int healslider { get { return RyzeMenu.SummonerSpells["heal"].Cast<Slider>().CurrentValue; } }
        public static int manaslider { get { return RyzeMenu.SummonerSpells["mana"].Cast<Slider>().CurrentValue; } }
        public static double needheal;
        private static string[] SmiteNames = new[] { "s5_summonersmiteplayerganker", "itemsmiteaoe", "s5_summonersmitequick", "s5_summonersmiteduel", "summonersmite" };
        private static double killing;
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }
        private static void Game_OnStart(EventArgs args)
        {
            if (myHero.Spellbook.GetSpell(SpellSlot.Summoner1).Name == "summonerheal") { Heal = new Spell.Active(SpellSlot.Summoner1); }
            if (myHero.Spellbook.GetSpell(SpellSlot.Summoner2).Name == "summonerheal") { Heal = new Spell.Active(SpellSlot.Summoner2); }
            if (myHero.Spellbook.GetSpell(SpellSlot.Summoner1).Name == "summonermana") { Clarity = new Spell.Active(SpellSlot.Summoner1); }
            if (myHero.Spellbook.GetSpell(SpellSlot.Summoner2).Name == "summonermana") { Clarity = new Spell.Active(SpellSlot.Summoner2); }
            new RyzeMenu().Init();
            new Items().Init();
            if (myHero.ChampionName == "Ryze")
            {
                Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1700, 100);
                W = new Spell.Targeted(SpellSlot.W, 600);
                E = new Spell.Targeted(SpellSlot.E, 600);
                R = new Spell.Active(SpellSlot.R);
            }
            Game.OnUpdate += Game_OnUpdate;
            //1095
        }
        private static void Game_OnUpdate(EventArgs args)
        {
            if (myHero.HealthPercent < healslider && Heal != null && !myHero.IsInShopRange() && !myHero.IsRecalling() && myHero.CountEnemiesInRange(900) > 0) { Heal.Cast(); }
            if (myHero.ManaPercent < manaslider && Clarity != null && !myHero.IsInShopRange() && !myHero.IsRecalling() && myHero.CountEnemiesInRange(900) > 0) { Clarity.Cast(); }
            var allyturret = EntityManager.Turrets.Allies.Where(k => !k.IsDead && k != null).OrderBy(k => k.Distance(myHero)).First();
            var enemyturret = EntityManager.Turrets.Enemies.Where(k => !k.IsDead && k != null).OrderBy(k => k.Distance(myHero)).First();
            var ally = EntityManager.Heroes.Allies.Where(x => !x.IsMe && !x.IsInShopRange() && !x.IsDead && !SmiteNames.Contains(x.Spellbook.GetSpell(SpellSlot.Summoner1).Name) &&
            !SmiteNames.Contains(x.Spellbook.GetSpell(SpellSlot.Summoner2).Name)).OrderBy(n => n.TotalAttackDamage).Last();
            if (RyzeMenu.Menu["recall"].Cast<CheckBox>().CurrentValue && ally.IsRecalling() && myHero.Distance(ally) <= 300)
            {
                Player.CastSpell(SpellSlot.Recall);
            }
            if (needheal == 0 && myHero.Distance(ally) >= 100 && killing == 0 && !myHero.IsRecalling() && ally != null)
            {
                Orbwalker.MoveTo(ally.Position - sliderdist);
            }
            if (myHero.Distance(allyturret) <= 250 && needheal == 1 && !myHero.IsInShopRange()) { Player.CastSpell(SpellSlot.Recall); }
            if (needheal == 1 && myHero.Distance(allyturret) >= 250) { Orbwalker.MoveTo(allyturret.Position); }
            if (myHero.HealthPercent < 20)
            {
                needheal = 1;
            }
            if (myHero.MaxMana > 100)
            {
                if (myHero.ManaPercent < 20)
                {
                    needheal = 1;
                }
                if (myHero.ManaPercent > 90)
                {
                    needheal = 0;
                }
            }
                                 
            if (myHero.IsRecalling() || myHero.ChampionName != "Ryze") return;
            if (killing == 0) { LastHit(); }
            if (kill) { Killable(); }
            if (myHero.Distance(enemyturret) > 700) { SluttyCombo(); }
        }
        private static void LastHit()
        {
            var minion = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsEnemy && x.IsValidTarget(Q.Range) && !x.BaseSkinName.ToLower().Contains("gangplankbarrel")).
                OrderBy(
                x => x.Health).FirstOrDefault();
            
            if (minion != null)
            {
                if (QLaneclear && myHero.GetBuffCount("ryzepassivestack") <= 2)
                {
                    if (myHero.GetSpellDamage(minion, SpellSlot.Q) >= minion.Health && !minion.IsDead && myHero.ManaPercent > QSlider)
                    {
                        Q.Cast(minion);
                    }
                }
                if (myHero.GetAutoAttackDamage(minion) > minion.Health && !minion.IsDead)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
                }
            }
        }
        private static void Killable()
        {
            var enemy = EntityManager.Heroes.Enemies.Where(b => !b.HasBuffOfType(BuffType.Invulnerability) && !b.IsDead).OrderBy(b => b.Health).FirstOrDefault();
            if (enemy == null) return;
            var enemyturret = EntityManager.Turrets.Enemies.Where(x => !x.IsDead).OrderBy(x => x.Distance(enemy)).First();
            if (!enemy.IsVisible || myHero.IsDead || myHero.Distance(enemy) > 2000 || enemy.Distance(enemyturret) < 1500) { killing = 0; return; }
            if (myHero.Distance(enemy) < 1800)
            {
                var damageQ = (Q.IsReady() ? myHero.GetSpellDamage(enemy, SpellSlot.Q) : 0);
                var damageW = (W.IsReady() ? myHero.GetSpellDamage(enemy, SpellSlot.W) : 0);
                var damageE = (E.IsReady() ? myHero.GetSpellDamage(enemy, SpellSlot.E) : 0);
                var damage = damageQ + damageW + damageE;

                if (damage > enemy.Health)
                {
                    killing = 1;                   
                }
                if (killing == 1)
                {
                    if (myHero.Distance(enemy) >= 150)
                    {
                        Orbwalker.MoveTo(enemy.Position);
                    }
                }
            }
        }
        private static void SluttyCombo()
        {
            var target = TargetSelector.GetTarget(900, DamageType.Magical);
            if (target == null) return;
            var Stacks = myHero.GetBuffCount("ryzepassivestack");
            var QPred = Prediction.Position.PredictLinearMissile(target, Q.Range, Q.Width, Q.CastDelay, Q.Speed, int.MinValue, myHero.Position);
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
