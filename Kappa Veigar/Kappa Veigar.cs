using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Spells;
using System;
using System.Linq;
using Color = System.Drawing.Color;
namespace KappaVeigar
{
    class Veigar : VeigarSpells
    {
        private static void Main(string[] args) { Loading.OnLoadingComplete += Game_OnStart; }
        public static Menu Menu, ComboM, HarassM,LaneclearM, KS, Draw, RMenu;
        public static bool CastCheckbox(Menu obj, string value) { return obj[value].Cast<CheckBox>().CurrentValue; }
        public static int CastSlider(Menu obj, string value) { return obj[value].Cast<Slider>().CurrentValue; }
        public static AIHeroClient myHero { get { return ObjectManager.Player; } }
        private static void Game_OnStart(EventArgs args)
        {
            if (myHero.Hero != Champion.Veigar) return;
            Q.AllowedCollisionCount = 2;
            Chat.Print("Kappa Veigar Loaded", Color.AliceBlue);
            Chat.Print("By Capitao Addon",Color.WhiteSmoke);
            InitMenu();
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnDraw(EventArgs args)
        {
            if (CastCheckbox(Draw, "drawq"))
            {
                new Circle() { Color = Q.IsReady() ? Color.White : Color.Tomato, Radius = Q.Range, BorderWidth = 2f }.Draw(myHero.Position);
            }
            if (CastCheckbox(Draw, "draww"))
            {
                new Circle() { Color = W.IsReady() ? Color.White : Color.Tomato, Radius = W.Range, BorderWidth = 2f }.Draw(myHero.Position);
            }
            if (CastCheckbox(Draw, "drawe"))
            {
                new Circle() { Color = E.IsReady() ? Color.White : Color.Tomato, Radius = E.Range, BorderWidth = 2f }.Draw(myHero.Position);
            }
            if (CastCheckbox(Draw, "drawr"))
            {
                new Circle() { Color = R.IsReady() ? Color.White : Color.Tomato, Radius = R.Range, BorderWidth = 2f }.Draw(myHero.Position);
            }
            var autoQ = HarassM["autoQ"].Cast<KeyBind>().CurrentValue;
            var pos = Drawing.WorldToScreen(Player.Instance.Position);
            Drawing.DrawText(pos.X - 35, pos.Y + 30, autoQ ? Color.White : Color.Red, autoQ ? "AutoQ ON" : "AutoQ OFF");
        }
        private static void OnUpdate(EventArgs args)
        {
            if (myHero.IsDead) return;
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                Combo();
            }
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LaneClear)
            {
                LaneClear();
            }
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass || HarassM["autoQ"].Cast<KeyBind>().CurrentValue) { Harass(); }

            KillSteal();
        }

        private static void LaneClear()
        {
            if (CastCheckbox(LaneclearM, "farmQ"))
            {
                var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, myHero.ServerPosition, Q.Range).
                    Where(
                    x => x.Health <= myHero.GetSpellDamage(x, SpellSlot.Q));

                if (minion != null && CastSlider(LaneclearM, "manaQ") <= myHero.Mana)
                {
                    var pred = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minion, Q.Width, (int)Q.Range);
                    if(CastCheckbox(LaneclearM, "minQ"))
                    {
                        if (pred.HitNumber == 2)
                        {
                            Q.Cast(pred.CastPosition);
                        }
                    }
                    else
                    {
                        Q.Cast(pred.CastPosition);
                    }
                }
            }        

            if (CastCheckbox(LaneclearM, "farmW"))
            {
                var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, myHero.ServerPosition, W.Range);
                if (minion != null && CastSlider(LaneclearM, "manaW") <= myHero.Mana)
                {
                    var pred = EntityManager.MinionsAndMonsters.GetCircularFarmLocation(minion, W.Width, (int)W.Range);
                    if (CastSlider(LaneclearM, "minW") <= pred.HitNumber)
                    {
                        W.Cast(pred.CastPosition);
                    }
                }
            }
        }

        private static void Harass()
        {
            if(CastCheckbox(HarassM, "farm"))
            {
                var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, myHero.ServerPosition, Q.Range).Where(
                    x => x.Health <= myHero.GetSpellDamage(x, SpellSlot.Q));

                if (minion.Count() == 0)
                {
                    if (CastCheckbox(HarassM, "useQ") && CastSlider(HarassM, "manaQ") <= myHero.ManaPercent)
                    {
                        var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                        if (target != null)
                        {
                            var predQ = Q.GetPrediction(target);
                            if (predQ.HitChance >= HitChance.Medium)
                            {
                                Q.Cast(predQ.CastPosition);
                            }
                        }
                    }
                }
            }
            else
            {
                if (CastCheckbox(HarassM, "useQ") && CastSlider(HarassM, "manaQ") <= myHero.ManaPercent)
                {
                    var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
                    if (target != null)
                    {
                        var predQ = Q.GetPrediction(target);
                        if (predQ.HitChance >= HitChance.Medium)
                        {
                            Q.Cast(predQ.CastPosition);
                        }
                    }
                }
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (target != null && !target.HasBuffOfType(BuffType.Invulnerability))
            {
                if (CastCheckbox(ComboM, "useW"))
                {
                    if (CastCheckbox(ComboM, "stunW"))
                    {
                        if (target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Charm) ||
                            target.HasBuffOfType(BuffType.Flee) ||
                            target.HasBuffOfType(BuffType.Suppression))
                            {
                            if (W.IsInRange(target) && W.IsReady())
                            { 
                                W.Cast(target.ServerPosition);
                            }
                        }
                    }
                    else
                    {
                        var Wpred = W.GetPrediction(target);
                        if (Wpred.HitChance >= HitChance.Medium && W.IsInRange(target) && W.IsReady())
                        {
                            W.Cast(Wpred.CastPosition);
                        }
                    }
                }
                
                if (CastCheckbox(ComboM, "useQ"))
                {
                    var Qpred = Q.GetPrediction(target);
                    if (Qpred.HitChance >= HitChance.Medium && Q.IsInRange(target) && Q.IsReady())
                    {
                        Q.Cast(Qpred.CastPosition);
                    }
                }
                if (CastCheckbox(ComboM, "useE"))
                {
                    CastE(target);
                }
                if (CastCheckbox(ComboM, "useR"))
                {
                    if (CastCheckbox(RMenu, target.ChampionName + "R"))
                    {
                        if (ComboDamage(target) > target.Health && myHero.GetSpellDamage(target, SpellSlot.Q) + myHero.GetSpellDamage(target, SpellSlot.W) < target.Health)
                        {
                            CastR(target);
                        }
                    }
                }
            }
        }

        private static void KillSteal()
        {
            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (target != null && !target.HasBuffOfType(BuffType.Invulnerability))
            {
                if (KSDamage(target) > target.Health)
                {
                    if (CastCheckbox(KS, "useQ"))
                    {
                        var Qpred = Q.GetPrediction(target);
                        if (Qpred.HitChance >= HitChance.Medium && Q.IsInRange(target))
                        {
                            Q.Cast(Qpred.CastPosition);
                        }
                    }

                    if (CastCheckbox(KS, "useE"))
                    {
                        CastE(target);

                        if (target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Charm) ||
                                target.HasBuffOfType(BuffType.Flee) ||
                                target.HasBuffOfType(BuffType.Suppression))
                        {
                            if (W.IsInRange(target) && W.IsReady())
                            {
                                W.Cast(target.Position);
                            }
                        }
                    }

                    if (CastCheckbox(KS, "useR"))
                    {
                        if (R.IsReady() && myHero.GetSpellDamage(target, SpellSlot.Q) < target.Health)
                        {
                            CastR(target);
                        }
                    }
                }
            }
        }
        private static void CastE(Obj_AI_Base target)
        {
            var pred = E.GetPrediction(target);
            if (E.IsReady() && E.IsInRange(target))
            {
                E.Cast(pred.CastPosition);
            }
        }
        private static void CastR(Obj_AI_Base enemy)
        {
            if (CastCheckbox(RMenu, enemy.BaseSkinName + "R"))
            {
                R.Cast(enemy);
            }
        }
        public static float ComboDamage(Obj_AI_Base hero)
        {
            double damage = 0;
            var player = ObjectManager.Player;
            if (Q.IsReady() && CastCheckbox(ComboM, "useQ"))
            {
                damage += player.GetSpellDamage(hero, SpellSlot.Q);
            }
            if (W.IsReady() && CastCheckbox(ComboM, "useW"))
            {
                damage += player.GetSpellDamage(hero, SpellSlot.W);
            }
            if (R.IsReady() && CastCheckbox(ComboM, "useR") && CastCheckbox(RMenu, hero.BaseSkinName + "R"))
            {
                damage += player.GetSpellDamage(hero, SpellSlot.R);
            }
            return (float)damage;
        }
        public static float KSDamage(AIHeroClient target)
        {
            double damage = 0;
            var player = ObjectManager.Player;
            if (Q.IsReady() && CastCheckbox(KS, "useQ"))
            {
                damage += player.GetSpellDamage(target, SpellSlot.Q);
            }
            if (W.IsReady() && CastCheckbox(KS, "useE"))
            {
                damage += player.GetSpellDamage(target, SpellSlot.W);
            }
            if (R.IsReady() && CastCheckbox(KS, "useR") && CastCheckbox(RMenu, target.ChampionName + "R"))
            {
                damage += player.GetSpellDamage(target, SpellSlot.R);
            }
            return (float)damage;
        }
        private static void InitMenu()
        {
            Menu = MainMenu.AddMenu("Kappa Vegiar", "menu");
            ComboM = Menu.AddSubMenu("Combo", "combo");
            RMenu = Menu.AddSubMenu("R Manager", "rmng");
            HarassM = Menu.AddSubMenu("Harass", "harass");
            LaneclearM = Menu.AddSubMenu("Laneclear", "laneclear");
            Draw = Menu.AddSubMenu("Drawings", "draw");
            KS = Menu.AddSubMenu("Killsteal", "ks");

            Menu.AddGroupLabel("Kappa Veigar - Made by Capitao Addon");
            Menu.AddSeparator();
            Menu.AddSeparator();
            Menu.AddLabel("Version: 1.0.0.0");

            ComboM.Add("stunW", new CheckBox("Only use W in immobile enemies"));
            ComboM.Add("useQ", new CheckBox("Use Q in Combo"));
            ComboM.Add("useW", new CheckBox("Use W in Combo"));
            ComboM.Add("useE", new CheckBox("Use E in Combo"));
            ComboM.Add("useR", new CheckBox("Use R when you can kill target"));

            KS.Add("useQ", new CheckBox("Q to KS"));
            KS.Add("useE", new CheckBox("Use E + W"));
            KS.Add("useR", new CheckBox("R to KS"));

            HarassM.Add("useQ", new CheckBox("Use in Combo"));
            HarassM.Add("manaQ", new Slider("Min mana to use Q harass", 60, 0, 100));
            HarassM.Add("autoQ", new KeyBind("Auto harass", false, KeyBind.BindTypes.PressToggle, 'X'));
            HarassM.Add("farm", new CheckBox("Priority farm", false));

            LaneclearM.Add("farmQ", new CheckBox("Q to lasthit"));
            LaneclearM.Add("manaQ", new Slider("Min mana to use Q", 30, 0, 100));          
            LaneclearM.Add("minQ", new CheckBox("Use Q only when you can farm 2 minions", false));
            LaneclearM.AddSeparator();
            LaneclearM.Add("farmW", new CheckBox("W to laneclear"));
            LaneclearM.Add("manaW", new Slider("Min mana to use W", 60, 0, 100));
            LaneclearM.Add("minW", new Slider("Min minions to use W", 3, 1, 9));

            Draw.Add("drawq", new CheckBox("Draw Q"));
            Draw.Add("draww", new CheckBox("Draw W"));
            Draw.Add("drawe", new CheckBox("Draw E"));
            Draw.Add("drawr", new CheckBox("Draw R"));

            // R
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy))
            {
                RMenu.Add(enemy.ChampionName + "R", new CheckBox("Use R when you can kill " + enemy.ChampionName));
            }
        } 
    }    
}
