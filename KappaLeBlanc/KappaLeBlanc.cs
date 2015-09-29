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
        public const string LeBlanc = "LeBlanc";
        public static Menu Menu,
            DrawMenu;

        public static Spell.Targeted Q;
        public static Spell.Skillshot W;
        public static Spell.Active W2;
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
            if (Player.Instance.ChampionName != LeBlanc)
            {
                Chat.Print("Champion not supported");
            }

            if (Player.Instance.ChampionName == LeBlanc)
            {
                Chat.Print("Leblanc loaded!");
            }
            Q = new Spell.Targeted(SpellSlot.Q, 700);
            W2 = new Spell.Active(SpellSlot.W);
            W = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Circular);
            E = new Spell.Skillshot(SpellSlot.E, 950, SkillShotType.Linear);
            R = new Spell.Targeted(SpellSlot.R, 700);
            


            Menu = MainMenu.AddMenu("LeBlanc", "leblanc");
            Menu.AddSeparator();
            Menu.AddLabel("Raise your Kappa");

            DrawMenu = Menu.AddSubMenu("Draw", "draw");
            DrawMenu.Add("drawDisable", new CheckBox("Disable draws", true));

        }

        public static void Game_OnDraw(EventArgs args)
        {
            if (!DrawMenu["drawDisable"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.White, Radius = 700, BorderWidth = 2f }.Draw(ObjectManager.Player.Position);
            }
        }  
            public static void Game_OnUpdate(EventArgs args)
        {
           var alvo = TargetSelector.GetTarget(720, DamageType.Magical);

            if (!alvo.IsValid()) return;

            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
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

            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass)
            {
                if (Q.IsReady() && W.IsReady() && _Player.Distance(alvo) <= Q.Range)
                {
                    Q.Cast(alvo);
                    W.Cast(alvo);
                    
                }
                

            }
        }
    }
}