using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using System;
using Color = System.Drawing.Color;
namespace KappaLeBlanc
{
    class Program
    {

        public const string Leblanc = "Leblanc";
        public static Menu Menu,
            DrawMenu,
            ComboMenu,
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

            else if (Player.Instance.ChampionName != Leblanc)
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

        }

        public static void Game_OnDraw(EventArgs args)
        {
            if (DrawMenu["drawQ"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (Q.IsReady()))
            {
                Drawing.DrawCircle(_Player.Position, Q.Range, Color.Yellow);
            }

            if (DrawMenu["drawW"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (W.IsReady()))
            {
                Drawing.DrawCircle(_Player.Position, W.Range, Color.Yellow);
            }

            if (DrawMenu["drawE"].Cast<CheckBox>().CurrentValue && !DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue && (E.IsReady()))
            {
                Drawing.DrawCircle(_Player.Position, E.Range, Color.Yellow);
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


            var alvo = TargetSelector.GetTarget(720, DamageType.Magical);

            if (!alvo.IsValid()) return;

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


                if (R.IsReady() && _Player.Distance(alvo) <= R.Range && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancChaosOrbM")
                {
                    R.Cast(alvo);
                }

                if (W.IsReady() && _Player.Distance(alvo) <= W.Range && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "LeblancSlideM")
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

                if (R.IsReady() && _Player.Distance(alvo) <= R.Range && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancSoulShackleM")
                {
                    R.Cast(alvo);
                }

                if (W.IsReady() && _Player.Distance(alvo) <= W.Range && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "LeblancSlideM")
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

               else if (R.IsReady() && _Player.Distance(alvo) <= R.Range && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Name == "LeblancSoulShackleM")
                {
                    R.Cast(alvo);
                }

                if (Q.IsReady() && _Player.Distance(alvo) <= Q.Range)
                {
                    R.Cast(alvo);
                }

                if (W.IsReady() && _Player.Distance(alvo) <= W.Range && ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "LeblancSlideM")
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