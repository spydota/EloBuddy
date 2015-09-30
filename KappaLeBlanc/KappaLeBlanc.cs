using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using System;
using System.Linq;
using Color = System.Drawing.Color;
namespace KappaLeBlanc
{
    class Program
    {
        public static void WLaneClear()
        {
            var minions = EntityManager.GetLaneMinions();
            if (!minions.Any()) return;
            if (LaneClearMenu["lcw"].Cast<CheckBox>().CurrentValue && W.IsReady())
            {
                var pred = Prediction.Position.PredictCircularMissileAoe(minions.ToArray(), 750, 40, 750, 0);
                if (pred.Any())
                {
                    var pred2 = pred.OrderByDescending(a => a.CollisionObjects.Count()).FirstOrDefault();
                    if (pred2 != null && pred2.CollisionObjects.Count() >= 2)
                    {
                        W.Cast(pred2.CastPosition);
                    }
                }

            }
        }
        public static float GetDamage(SpellSlot spell, Obj_AI_Base target)
        {
            float ap = _Player.FlatMagicDamageMod + _Player.BaseAbilityDamage;
            if (spell == SpellSlot.Q)
            {
                if (!Q.IsReady())
                    return 0;
                return _Player.CalculateDamageOnUnit(target, DamageType.Magical, 15f + 35f * (Q.Level - 1) + 100 / 100 * ap);
            }
            return 0;

        }
        private static void KillSteal()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (KillStealMenu["qks"].Cast<CheckBox>().CurrentValue && Q.IsReady() && target.IsValidTarget(Q.Range) && target.Health <= GetDamage(SpellSlot.Q, target))
            {
                Q.Cast(target);
            }

        }
        public const string Leblanc = "Leblanc";
        public static Menu Menu,
            DrawMenu,
            ComboMenu,
            KillStealMenu,
            LaneClearMenu,
            HarassMenu;
        public static Spell.Targeted Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Targeted R;
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnStart(EventArgs args)
        {
            if (Player.Instance.ChampionName == Leblanc)
            {
                Chat.Print("Leblanc loaded!");
            }

            if (Player.Instance.ChampionName != Leblanc)
            {
                Chat.Print("Champion not supported");
            }

            Q = new Spell.Targeted(SpellSlot.Q, 700);
            W = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Circular);
            E = new Spell.Skillshot(SpellSlot.E, 950, SkillShotType.Linear);
            R = new Spell.Targeted(SpellSlot.R, 700);



            Menu = MainMenu.AddMenu("LeBlanc", "leblanc");
            Menu.AddLabel("Raise your Kappa");

            DrawMenu = Menu.AddSubMenu("Draw", "draw");
            DrawMenu.Add("drawDisable", new CheckBox("Disable draws", true));
            DrawMenu.Add("highDraws", new CheckBox("High Quality draws", false));
            DrawMenu.Add("drawQ", new CheckBox("Draw Q", false));
            DrawMenu.Add("drawW", new CheckBox("Draw W", false));
            DrawMenu.Add("drawE", new CheckBox("Draw E", false));
        
            ComboMenu = Menu.AddSubMenu("Combo", "lbcombo");
            ComboMenu.AddLabel("Mark only 1 at once!");
            ComboMenu.Add("comboERQW", new CheckBox("Use ERQW Combo", false));
            ComboMenu.Add("comboQERW", new CheckBox("Use QERW Combo", false));
            ComboMenu.Add("comboEQRW", new CheckBox("Use EQRW Combo", true));

            HarassMenu = Menu.AddSubMenu("Harass", "lbharass");
            HarassMenu.Add("hEQ", new CheckBox("Use EQ Harass", true));
            HarassMenu.Add("hQW", new CheckBox("Use QW Harass", true));
            HarassMenu.Add("hQEW", new CheckBox("Use QEW Harass", false));

            LaneClearMenu = Menu.AddSubMenu("Laneclear", "laneclear");
            LaneClearMenu.Add("lcw", new CheckBox("Use W Laneclear", true));

            KillStealMenu = Menu.AddSubMenu("KS", "ks");
            KillStealMenu.Add("qks", new CheckBox("Use Q to KillSteal"));
        }

        public static void Game_OnDraw(EventArgs args)
        {
            if (DrawMenu["highDraws"].Cast<CheckBox>().CurrentValue && DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (Q.IsReady()))
            {
                Drawing.DrawCircle(_Player.Position, Q.Range, Color.Yellow);
            }

            if (DrawMenu["highDraws"].Cast<CheckBox>().CurrentValue && DrawMenu["drawW"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (W.IsReady()))
            {
                Drawing.DrawCircle(_Player.Position, W.Range, Color.Yellow);
            }

            if (DrawMenu["highDraws"].Cast<CheckBox>().CurrentValue && DrawMenu["drawE"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (E.IsReady()))
            {
                Drawing.DrawCircle(_Player.Position, E.Range, Color.Yellow);
            }

            if (!DrawMenu["highDraws"].Cast<CheckBox>().CurrentValue && DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (Q.IsReady()))
            {
                new Circle() { Color = Color.White, Radius = 700, BorderWidth = 2f }.Draw(_Player.Position);
            }

            if (!DrawMenu["highDraws"].Cast<CheckBox>().CurrentValue && DrawMenu["drawW"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (W.IsReady()))
            {
                new Circle() { Color = Color.White, Radius = 600, BorderWidth = 2f }.Draw(_Player.Position);
            }

            if (!DrawMenu["highDraws"].Cast<CheckBox>().CurrentValue && DrawMenu["drawE"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (E.IsReady()))
            {
                new Circle() { Color = Color.White, Radius = 950, BorderWidth = 2f }.Draw(_Player.Position);
            }






            if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (!Q.IsReady()))
            {
                Drawing.DrawCircle(_Player.Position, Q.Range, Color.Red);
            }

            if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (!W.IsReady()))
            {
                Drawing.DrawCircle(_Player.Position, W.Range, Color.Red);
            }

            if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (!E.IsReady()))
            {
                Drawing.DrawCircle(_Player.Position, E.Range, System.Drawing.Color.Red);
            }

        }
        public static void Game_OnUpdate(EventArgs args)
        {
            KillSteal();

            var alvo = TargetSelector.GetTarget(720, DamageType.Magical);

            if (!alvo.IsValid()) return;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                WLaneClear();
            }

            if (ComboMenu["comboEQRW"].Cast<CheckBox>().CurrentValue && Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                if (E.IsReady() && _Player.Distance(alvo) <= E.Range)
                {
                    E.Cast(alvo);
                }

                if (Q.IsReady() && _Player.Distance(alvo) <= Q.Range)
                {
                    Q.Cast(alvo);
                }


                if (R.IsReady() && _Player.Distance(alvo) <= R.Range)
                {
                    R.Cast(alvo);
                }

                if (W.IsReady() && _Player.Distance(alvo) <= W.Range)
                {
                    W.Cast(alvo);

                }



            }

            if (ComboMenu["comboQERW"].Cast<CheckBox>().CurrentValue && Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                if (Q.IsReady() && _Player.Distance(alvo) <= Q.Range)
                {
                    Q.Cast(alvo);
                }

                if (E.IsReady() && _Player.Distance(alvo) <= E.Range)
                {
                    E.Cast(alvo);
                }

                if (R.IsReady() && _Player.Distance(alvo) <= R.Range)
                {
                    R.Cast(alvo);
                }

                if (W.IsReady() && _Player.Distance(alvo) <= W.Range)
                {
                    W.Cast(alvo);

                }


            }

            if (ComboMenu["comboERQW"].Cast<CheckBox>().CurrentValue && Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                if (E.IsReady() && _Player.Distance(alvo) <= E.Range)
                {
                    E.Cast(alvo);
                }

                if (R.IsReady() && _Player.Distance(alvo) <= R.Range)
                {
                    R.Cast(alvo);
                }

                if (Q.IsReady() && _Player.Distance(alvo) <= Q.Range)
                {
                    R.Cast(alvo);
                }

                if (W.IsReady() && _Player.Distance(alvo) <= W.Range)
                {
                    W.Cast(alvo);

                }


            }

            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass)
            {

                if (HarassMenu["hQEW"].Cast<CheckBox>().CurrentValue && Q.IsReady() && E.IsReady() && W.IsReady() && _Player.Distance(alvo) <= W.Range + E.Range)
                {

                    Q.Cast(alvo);
                    E.Cast(alvo);
                    W.Cast(alvo);

                }

                if (HarassMenu["hQW"].Cast<CheckBox>().CurrentValue && Q.IsReady() && W.IsReady() && _Player.Distance(alvo) <= Q.Range)
                {

                    Q.Cast(alvo);
                    W.Cast(alvo);
                    W.Cast(alvo);

                }
                if (HarassMenu["hEQ"].Cast<CheckBox>().CurrentValue && E.IsReady() && Q.IsReady() && _Player.Distance(alvo) <= E.Range)
                {

                    E.Cast(alvo);
                    Q.Cast(alvo);

                }

            }

        }
        

    }
}