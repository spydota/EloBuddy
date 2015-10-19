using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Linq;

namespace RyzeAutoplay
{
    class Program
    {
        //Made for own use
        public static Menu Menu;
        public static AIHeroClient myHero { get { return ObjectManager.Player; } }
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static Item SapphireCrystal, Hpot, Mpot, Tear, NeedlesslyLargeRod, ArchangelsStaff, RubyCrystal, Catalyst, BlastingWand, ROA;
        public static bool keybind { get { return Menu["keybind"].Cast<KeyBind>().CurrentValue; } }
        public static double needheal;
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }
        private static void Game_OnStart(EventArgs args)
        {
            if (myHero.ChampionName != "Ryze") return;
            Hpot = new Item((int)ItemId.Health_Potion);
            Mpot = new Item((int)ItemId.Mana_Potion);
            SapphireCrystal = new Item((int)ItemId.Sapphire_Crystal);
            RubyCrystal = new Item((int)ItemId.Ruby_Crystal);
            SapphireCrystal = new Item((int)ItemId.Sapphire_Crystal);
            BlastingWand = new Item((int)ItemId.Blasting_Wand);
            ROA = new Item((int)ItemId.Rod_of_Ages);
            Catalyst = new Item((int)ItemId.Catalyst_the_Protector);
            ArchangelsStaff = new Item((int)ItemId.Archangels_Staff);
            Tear = new Item((int)ItemId.Tear_of_the_Goddess);
            NeedlesslyLargeRod = new Item((int)ItemId.Needlessly_Large_Rod);

            Menu = MainMenu.AddMenu("RyzeFollow", "ryzefollow");
            Menu.Add("keybind", new KeyBind("FollowAlly", true, KeyBind.BindTypes.PressToggle, 'L'));
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1700, 100);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R);
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Game.OnUpdate += Game_OnUpdate;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
        }
        private static void Game_OnUpdate(EventArgs args)
        { 
            var allyturret = EntityManager.Turrets.Allies.Where(k => !k.IsDead && k != null).OrderBy(k => k.Distance(myHero)).First();
            var ally = EntityManager.Heroes.Allies.Where(x => !x.IsMe && !x.IsInShopRange() && x != null && !x.IsDead).FirstOrDefault();

            if(ally.IsRecalling() && myHero.Distance(ally) <= 400)
            {
                Player.CastSpell(SpellSlot.Recall);
            }
            if (keybind && needheal == 0)
            {
                Orbwalker.MoveTo(ally.Position - 150);               
            }
            if (myHero.Distance(allyturret) <= 450 && needheal == 1 && !myHero.IsInShopRange() ) { Player.CastSpell(SpellSlot.Recall); }
            if (needheal == 1 && myHero.Distance(allyturret) <= 450) { Orbwalker.MoveTo(allyturret.Position); }
            if (myHero.HealthPercent < 20 || myHero.ManaPercent < 10) { needheal = 1; }
            if (myHero.HealthPercent > 90 && myHero.ManaPercent > 90) { needheal = 0; }

            if (!myHero.IsInShopRange() && !myHero.IsRecalling())
            {
                if (myHero.HealthPercent < 60 && Hpot.IsOwned() && !Player.HasBuff("Health Potion"))
                {
                    Hpot.Cast();
                }
                if (myHero.ManaPercent < 60 && Mpot.IsOwned() && !Player.HasBuff("Mana Potion"))
                {
                    Mpot.Cast();
                }
            }
            //Needs Rework
            if (myHero.IsInShopRange() || myHero.IsDead)
            {
                var Gold = myHero.Gold;
                if (Gold >= 475 && !SapphireCrystal.IsOwned() && !Catalyst.IsOwned())
                {
                    SapphireCrystal.Buy();
                }
                if (myHero.Gold >= 35 && !Hpot.IsOwned())
                {
                    Hpot.Buy();
                }
                if (Gold >= 35 && !Mpot.IsOwned())
                {
                    Mpot.Buy();
                }
                if (Gold >= 320 && !Tear.IsOwned() && !ArchangelsStaff.IsOwned() && SapphireCrystal.IsOwned())
                {
                    Tear.Buy();
                }
                if (Gold >= 400 && !Catalyst.IsOwned() && !RubyCrystal.IsOwned())
                {
                    RubyCrystal.Buy();
                }
                if (Gold >= 400 && !Catalyst.IsOwned() && RubyCrystal.IsOwned() && SapphireCrystal.IsOwned())
                {
                    Catalyst.Buy();
                }
                if (Gold >= 850 && !BlastingWand.IsOwned() && ArchangelsStaff.IsOwned())
                {
                    BlastingWand.Buy();
                }
                if (Gold >= 650 && BlastingWand.IsOwned() && Catalyst.IsOwned() && ArchangelsStaff.IsOwned())
                {
                    ROA.Buy();
                }
            }
            
            if (myHero.IsRecalling()) { return; }
            KS();
            LastHit();
            SluttyCombo();
        }
        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (W.IsReady() && sender != null && sender.IsEnemy)
            {
                W.Cast(sender);
            }
        }
        private static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            if (W.IsReady() && sender.IsEnemy && sender.IsValidTarget(W.Range))
            {
                W.Cast(sender);
            }
        }
        private static void LastHit()
        {
            var minion = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsEnemy && x.IsValidTarget(Q.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (minion == null) return;
            if (myHero.GetSpellDamage(minion, SpellSlot.Q) >= minion.Health && !minion.IsDead)
            {
                Q.Cast(minion);
            }
            if (myHero.GetSpellDamage(minion, SpellSlot.E) >= minion.Health && !minion.IsDead)
            {
                E.Cast(minion);
            }
        }
        private static void KS()
        {
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(any => !any.HasBuffOfType(BuffType.Invulnerability)))
            {
                if (enemy == null) return;
                if (enemy.IsValidTarget(W.Range) && myHero.GetSpellDamage(enemy, SpellSlot.W) > (enemy.Health - 10) && !enemy.IsDead)
                {
                    W.Cast(enemy);
                }
                if (enemy.IsValidTarget(Q.Range) && myHero.GetSpellDamage(enemy, SpellSlot.Q) > (enemy.Health - 10) && !enemy.IsDead)
                {
                    Q.Cast(enemy);
                }
                if (enemy.IsValidTarget(E.Range) && myHero.GetSpellDamage(enemy, SpellSlot.E) > (enemy.Health - 10) && !enemy.IsDead)
                {
                    E.Cast(enemy);
                }
            }
        }
        private static void SluttyCombo()
        {
            var target = TargetSelector.GetTarget(600, DamageType.Magical);
            if (target == null) return;
            var Stacks = myHero.GetBuffCount("ryzepassivestack");
            var QPred = Prediction.Position.PredictLinearMissile(target, Q.Range, Q.Width, Q.CastDelay, Q.Speed, int.MinValue, myHero.ServerPosition);
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
                            if (target.HasBuff("RyzeW"))
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
                            if (target.HasBuff("RyzeW"))
                                R.Cast();
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
                            if (target.HasBuff("RyzeW"))
                                R.Cast();
                            if (!E.IsReady() && !Q.IsReady() && !W.IsReady())
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
                            if (target.HasBuff("RyzeW"))
                                R.Cast();
                            if (!E.IsReady() && !Q.IsReady() && !W.IsReady())
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
