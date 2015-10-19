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
        public static Menu Menu, Laneclear;
        public static AIHeroClient myHero { get { return ObjectManager.Player; } }
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static Item SapphireCrystal, Tear, NeedlesslyLargeRod, ArchangelsStaff, RubyCrystal, Catalyst, BlastingWand, ROA;
        public static bool keybind { get { return Menu["keybind"].Cast<KeyBind>().CurrentValue; } }
        public static int sliderdist { get { return Menu["sliderdist"].Cast<Slider>().CurrentValue; } }
        public static bool QLaneclear { get { return Laneclear["QLaneclear"].Cast<CheckBox>().CurrentValue; } }
        public static int QSlider { get { return Laneclear["QSlider"].Cast<Slider>().CurrentValue; } }
        public static double needheal;
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }
        private static void Game_OnStart(EventArgs args)
        {
            if (myHero.ChampionName != "Ryze") return;
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
            Menu.Add("sliderdist", new Slider("Distance to ally", 70, 50, 300));
            Laneclear = Menu.AddSubMenu("Laneclear", "laneclear");
            Laneclear.Add("QLaneclear", new CheckBox("Use Q in laneclear"));
            Laneclear.Add("QSlider", new Slider("Use Q in laneclear only if mana > than", 40, 0, 100));

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
            if (keybind && needheal == 0 && myHero.Distance(ally) >= sliderdist)
            {
                Orbwalker.MoveTo(ally.Position - sliderdist);               
            }
            if (myHero.Distance(allyturret) <= 450 && needheal == 1 && !myHero.IsInShopRange() ) { Player.CastSpell(SpellSlot.Recall); }
            if (needheal == 1) { Orbwalker.MoveTo(allyturret.Position); }
            if (myHero.HealthPercent < 20 || myHero.ManaPercent < 10) { needheal = 1; }
            if (myHero.HealthPercent > 90 && myHero.ManaPercent > 90) { needheal = 0; }

            //Needs Rework
            if (myHero.IsInShopRange() || myHero.IsDead)
            {
                var Gold = myHero.Gold;
                if (ROA.IsOwned())
                {
                    if (Gold >= 400 && !SapphireCrystal.IsOwned() && !Catalyst.IsOwned())
                    {
                        SapphireCrystal.Buy();
                    }
                    if (Gold >= 400 && !RubyCrystal.IsOwned() && !Catalyst.IsOwned())
                    {
                        RubyCrystal.Buy();
                    }
                    if (Gold >= 400 && !Catalyst.IsOwned() && SapphireCrystal.IsOwned() && RubyCrystal.IsOwned())
                    {
                        Catalyst.Buy();
                    }
                    if (Gold >= 850 && !BlastingWand.IsOwned() && Catalyst.IsOwned())
                    {
                        BlastingWand.Buy();
                    }
                    if (Gold >= 650 && BlastingWand.IsOwned() && Catalyst.IsOwned())
                    { ROA.Buy(); }
                }
                if (Gold >= 475 && !SapphireCrystal.IsOwned() && !Tear.IsOwned() && !ArchangelsStaff.IsOwned())
                {
                    SapphireCrystal.Buy();
                }                
                if (Gold >= 320 && !Tear.IsOwned() && !ArchangelsStaff.IsOwned() && SapphireCrystal.IsOwned())
                {
                    Tear.Buy();
                }
                if (Gold >= 400 && !Catalyst.IsOwned() && !RubyCrystal.IsOwned())
                {
                    RubyCrystal.Buy();
                }
                if (Gold >= 800 && !Catalyst.IsOwned() && RubyCrystal.IsOwned())
                {
                    Catalyst.Buy();
                }
                if (Gold >= 1250 && !ArchangelsStaff.IsOwned() && Tear.IsOwned() && !NeedlesslyLargeRod.IsOwned())
                {
                    NeedlesslyLargeRod.Buy();
                }
                if (Gold >= 1030 && !ArchangelsStaff.IsOwned() && Tear.IsOwned() && NeedlesslyLargeRod.IsOwned())
                {
                    ArchangelsStaff.Buy();
                }
                if (Gold >= 850 && !BlastingWand.IsOwned() && !ROA.IsOwned())
                {
                    BlastingWand.Buy();
                }
                if (Gold >= 650 && BlastingWand.IsOwned() && Catalyst.IsOwned() && !ROA.IsOwned())
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

            if (myHero.GetAutoAttackDamage(minion) >= minion.Health && !minion.IsDead && myHero.Distance(minion) <= 500)
            {
                Orbwalker.ForcedTarget = minion;
            }
            if (QLaneclear)
            {
                if (myHero.GetSpellDamage(minion, SpellSlot.Q) >= minion.Health && !minion.IsDead && myHero.ManaPercent > QSlider)
                {
                    Q.Cast(minion);
                }

                if (myHero.GetSpellDamage(minion, SpellSlot.E) >= minion.Health && minion.CountEnemiesInRange(100) >= 3)
                {
                    E.Cast(minion);
                }
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
