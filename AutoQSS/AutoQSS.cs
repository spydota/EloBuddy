using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using System;
using System.Drawing;

namespace AutoQSS
{
    class Program
    {      
        public static Menu Menu;
        public static Text textDraw = new Text("", new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold));
        public static Item QSS, MercurialScimitar;
        public static bool keybind { get { return Menu["keybind"].Cast<KeyBind>().CurrentValue; } }
        public static bool drawk { get { return Menu["drawk"].Cast<CheckBox>().CurrentValue; } }
        public static bool Taunt { get { return Menu["Taunt"].Cast<CheckBox>().CurrentValue; } }
        public static bool Stun { get { return Menu["Stun"].Cast<CheckBox>().CurrentValue; } }
        public static bool Snare { get { return Menu["Snare"].Cast<CheckBox>().CurrentValue; } }
        public static bool Polymorph { get { return Menu["Polymorph"].Cast<CheckBox>().CurrentValue; } }
        public static bool Blind { get { return Menu["Blind"].Cast<CheckBox>().CurrentValue; } }
        public static bool Fear { get { return Menu["Fear"].Cast<CheckBox>().CurrentValue; } }
        public static bool Charm { get { return Menu["Charm"].Cast<CheckBox>().CurrentValue; } }
        public static bool Suppression { get { return Menu["Suppression"].Cast<CheckBox>().CurrentValue; } }
        public static bool Silence { get { return Menu["Silence"].Cast<CheckBox>().CurrentValue; } }
        public static bool ZedUlt { get { return Menu["ZedUlt"].Cast<CheckBox>().CurrentValue; } }
        public static bool VladUlt { get { return Menu["VladUlt"].Cast<CheckBox>().CurrentValue; } }
        public static bool FizzUlt { get { return Menu["FizzUlt"].Cast<CheckBox>().CurrentValue; } }
        public static bool MordUlt { get { return Menu["MordUlt"].Cast<CheckBox>().CurrentValue; } }
        public static bool PoppyUlt { get { return Menu["PoppyUlt"].Cast<CheckBox>().CurrentValue; } }
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Game_OnDraw;
        }
        private static void Game_OnStart(EventArgs args)
        {
            QSS = new Item((int)ItemId.Quicksilver_Sash);
            MercurialScimitar = new Item((int)ItemId.Dervish_Blade);            

            Menu = MainMenu.AddMenu("Auto QSS", "autoqss");
            Menu.Add("keybind", new KeyBind("Auto QSS", false, KeyBind.BindTypes.PressToggle, 'L'));
            Menu.Add("drawk", new CheckBox("Draw Keybind"));
            Menu.AddGroupLabel("Auto QSS if :");
            Menu.Add("Taunt", new CheckBox("Taunt"));
            Menu.Add("Stun", new CheckBox("Stun"));
            Menu.Add("Snare", new CheckBox("Snare"));
            Menu.Add("Polymorph", new CheckBox("Polymorph"));
            Menu.Add("Blind", new CheckBox("Blind", false));
            Menu.Add("Fear", new CheckBox("Fear"));
            Menu.Add("Charm", new CheckBox("Charm"));
            Menu.Add("Suppression", new CheckBox("Suppression"));
            Menu.Add("Silence", new CheckBox("Silence", false));
            Menu.AddGroupLabel("Ults");
            Menu.Add("ZedUlt", new CheckBox("Zed Ult"));
            Menu.Add("VladUlt", new CheckBox("Vlad Ult"));
            Menu.Add("FizzUlt", new CheckBox("Fizz Ult"));
            Menu.Add("MordUlt", new CheckBox("Mordekaiser Ult"));
            Menu.Add("PoppyUlt", new CheckBox("Poppy Ult"));

        }
        private static void Game_OnUpdate(EventArgs args)
        {            
            if (!keybind || ObjectManager.Player.IsDead) return;
            if (Player.HasBuffOfType(BuffType.Taunt) && Taunt && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuffOfType(BuffType.Stun) && Stun && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuffOfType(BuffType.Snare) && Snare && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuffOfType(BuffType.Polymorph) && Polymorph && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuffOfType(BuffType.Blind) && Blind && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuffOfType(BuffType.Fear) && Fear && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuffOfType(BuffType.Charm) && Charm && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuffOfType(BuffType.Suppression) && Suppression && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuffOfType(BuffType.Silence) && Silence && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuff("zedulttargetmark") && ZedUlt && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuff("VladimirHemoplague") && VladUlt && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuff("FizzMarinerDoom") && FizzUlt && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuff("MordekaiserChildrenOfTheGrave") && MordUlt && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }
            if (Player.HasBuff("PoppyDiplomaticImmunity") && PoppyUlt && QSS.IsReady() || MercurialScimitar.IsReady())
            {
                QSS.Cast();
                MercurialScimitar.Cast();
            }

        }
        private static void Game_OnDraw(EventArgs args)
        {
            if (!drawk) return;
            var pos = Drawing.WorldToScreen(Player.Instance.Position);
            textDraw.Draw(keybind ? "Auto QSS ON" : "Auto QSS OFF", keybind? SharpDX.Color.White : SharpDX.Color.Red, (int)pos.X - 45,
                   (int)pos.Y + 40);
        }
    }   
}