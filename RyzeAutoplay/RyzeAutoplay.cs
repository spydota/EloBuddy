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
        public static Menu Menu, Laneclear, Agressive, SummonerSpells;
        public static Spell.Active Heal;
        public static Spell.Active Clarity;
        public static AIHeroClient myHero { get { return ObjectManager.Player; } }
        public static Spell.Skillshot Q;
        public static Spell.Targeted W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static Item SapphireCrystal, Tear, NeedlesslyLargeRod, ArchangelsStaff, RubyCrystal, Catalyst, BlastingWand, ROA , SeraphEmbrace, Boots, MercuryTreads, trinket;
        public static bool keybind { get { return Menu["keybind"].Cast<KeyBind>().CurrentValue; } }
        public static int sliderdist { get { return Menu["sliderdist"].Cast<Slider>().CurrentValue; } }
        public static bool QLaneclear { get { return Laneclear["QLaneclear"].Cast<CheckBox>().CurrentValue; } }
        public static int QSlider { get { return Laneclear["QSlider"].Cast<Slider>().CurrentValue; } }
        public static bool kill { get { return Agressive["kill"].Cast<CheckBox>().CurrentValue; } }
        public static int healslider { get { return SummonerSpells["heal"].Cast<Slider>().CurrentValue; } }
        public static int manaslider { get { return SummonerSpells["mana"].Cast<Slider>().CurrentValue; } }
        public static double needheal;
        private static string[] SmiteNames = new[] { "s5_summonersmiteplayerganker", "itemsmiteaoe", "s5_summonersmitequick", "s5_summonersmiteduel", "summonersmite" };

        public static double killing;
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

            trinket = new Item((int)ItemId.Warding_Totem_Trinket);
            Boots = new Item((int)ItemId.Boots_of_Speed);
            MercuryTreads = new Item((int)ItemId.Mercurys_Treads);
            SapphireCrystal = new Item((int)ItemId.Sapphire_Crystal);
            RubyCrystal = new Item((int)ItemId.Ruby_Crystal);
            SapphireCrystal = new Item((int)ItemId.Sapphire_Crystal);
            BlastingWand = new Item((int)ItemId.Blasting_Wand);
            ROA = new Item((int)ItemId.Rod_of_Ages);
            Catalyst = new Item((int)ItemId.Catalyst_the_Protector);
            ArchangelsStaff = new Item((int)ItemId.Archangels_Staff);
            Tear = new Item((int)ItemId.Tear_of_the_Goddess);
            NeedlesslyLargeRod = new Item((int)ItemId.Needlessly_Large_Rod);
            SeraphEmbrace = new Item(3040);
            Menu = MainMenu.AddMenu("RyzeFollow", "ryzefollow");
            Menu.Add("keybind", new KeyBind("FollowAlly", true, KeyBind.BindTypes.PressToggle, 'L'));
            Menu.Add("sliderdist", new Slider("Distance to ally", 70, 50, 300));
            Menu.Add("recall", new CheckBox("Recall if ally is recalling"));
            Laneclear = Menu.AddSubMenu("Laneclear", "laneclear");
            Laneclear.Add("QLaneclear", new CheckBox("Use Q in laneclear"));
            Laneclear.Add("QSlider", new Slider("Use Q in laneclear only if mana > than", 40, 0, 100));
            Agressive = Menu.AddSubMenu("Agressive mode", "agm");
            Agressive.Add("kill", new CheckBox("Orbwalk to target if enemy is killable"));
            SummonerSpells = Menu.AddSubMenu("Summoner spells", "summs");
            SummonerSpells.Add("heal", new Slider("Use heal at % health", 40, 0, 100));
            SummonerSpells.Add("mana", new Slider("Use clarity at % mana", 40, 0, 100));
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
            if (kill && myHero.ChampionName == "Ryze") { Killable(); }
            if (myHero.HealthPercent < healslider && Heal != null && !myHero.IsInShopRange()) { Heal.Cast(); }
            if (myHero.ManaPercent < manaslider && Clarity != null && !myHero.IsInShopRange()) { Clarity.Cast(); }
            var allyturret = EntityManager.Turrets.Allies.Where(k => !k.IsDead && k != null).OrderBy(k => k.Distance(myHero)).First();
            var enemyturret = EntityManager.Turrets.Enemies.Where(k => !k.IsDead && k != null).OrderBy(k => k.Distance(myHero)).First();
            var ally = EntityManager.Heroes.Allies.Where(x => !x.IsMe && !x.IsRecalling() && !x.IsInShopRange() && !x.IsDead && !SmiteNames.Contains(x.Spellbook.GetSpell(SpellSlot.Summoner1).Name) &&
            !SmiteNames.Contains(x.Spellbook.GetSpell(SpellSlot.Summoner2).Name)).OrderBy(n => n.TotalAttackDamage).Last();

            if (ally == null) return;
            if (Menu["recall"].Cast<CheckBox>().CurrentValue && ally.IsRecalling() && myHero.Distance(ally) <= 400)
            {
                Player.CastSpell(SpellSlot.Recall);
            }
            if (keybind && needheal == 0 && myHero.Distance(ally) >= 100 && killing == 0)
            {
                Orbwalker.MoveTo(ally.Position - sliderdist);               
            }
            if (myHero.Distance(enemyturret) < 500)
            {
                Player.IssueOrder(GameObjectOrder.AutoAttack, enemyturret);
            }

            if (myHero.Distance(allyturret) <= 250 && needheal == 1 && !myHero.IsInShopRange() ) { Player.CastSpell(SpellSlot.Recall); }
            if (needheal == 1 && myHero.Distance(allyturret) >= 250) { Orbwalker.MoveTo(allyturret.Position); }
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
                if (!trinket.IsOwned()) { trinket.Buy();}
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
                if (Gold >= 325 && !Boots.IsOwned() && !MercuryTreads.IsOwned())
                {
                    Boots.Buy();
                }
                if (Gold >= 875 && Boots.IsOwned() && !MercuryTreads.IsOwned())
                {
                    MercuryTreads.Buy();
                }
            }
            if (myHero.IsRecalling() || myHero.ChampionName != "Ryze") { return; }
            if (killing == 0) { LastHit(); }
            if (myHero.Distance(enemyturret) > 700) { SluttyCombo(); }
        }
        private static void Killable()
        {
            var enemy = EntityManager.Heroes.Enemies.Where(b => !b.HasBuffOfType(BuffType.Invulnerability)).OrderBy(b => b.Health).FirstOrDefault();
            var enemyturret = EntityManager.Turrets.Enemies.Where(k => !k.IsDead).OrderBy(k => k.Distance(enemy)).First();
            if (enemy.IsDead || !enemy.IsVisible || myHero.IsDead || enemy == null || myHero.Distance(enemy) > 2500) { killing = 0; }
            if (enemy != null && myHero.Distance(enemy) < 1300)
            {
                var damageQ = (Q.IsReady() ? myHero.GetSpellDamage(enemy, SpellSlot.Q) : 0);
                var damageW = (W.IsReady() ? myHero.GetSpellDamage(enemy, SpellSlot.W) : 0);
                var damageE = (E.IsReady() ? myHero.GetSpellDamage(enemy, SpellSlot.E) : 0);
                var damage = damageQ + damageW + damageE;

                if (damage > enemy.Health && !myHero.IsDead && !enemy.IsDead && enemy.Distance(enemyturret) > 500 && enemy.IsVisible)
                {
                    killing = 1;
                    if (myHero.Distance(enemy) >= 150)
                    { 
                        Orbwalker.MoveTo(enemy.Position);
                    }
                    
                }
            }
        }
        private static void LastHit()
        {
            var minion = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsEnemy && x.IsValidTarget(Q.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (minion == null) return;
            if (myHero.GetAutoAttackDamage(minion) >= minion.Health && !minion.IsDead && myHero.Distance(minion) <= 700)
            {
                Player.IssueOrder(GameObjectOrder.AutoAttack, minion);
            }
            if (QLaneclear)
            {
                if (myHero.GetSpellDamage(minion, SpellSlot.Q) >= minion.Health && !minion.IsDead && myHero.ManaPercent > QSlider)
                {
                    Q.Cast(minion);
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