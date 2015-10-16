using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using System;
using System.Drawing;

namespace MasterySpamemr
{
    class Program
    {
        //Made for own use
        public static Menu Menu;
        public static Text textDraw = new Text("", new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold));
        public static bool mspamm { get { return Menu["mspamm"].Cast<KeyBind>().CurrentValue; } }
        public static bool drawk { get { return Menu["dk"].Cast<CheckBox>().CurrentValue; } }
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Game_OnDraw;
        }
        private static void Game_OnStart(EventArgs args)
        {
            Menu = MainMenu.AddMenu("Mastery Spammer", "masteryspm");
            Menu.Add("mspamm", new KeyBind("Spamm mastery", false, KeyBind.BindTypes.PressToggle, 'L'));
            Menu.Add("dk", new CheckBox("Draw Keybind"));
        }
        private static void Game_OnUpdate(EventArgs args)
        {
            if (mspamm)
            {
                Chat.Say("/masterybadge");
            }
        }
        private static void Game_OnDraw(EventArgs args)
        {
            if (!drawk) return;
            var pos = Drawing.WorldToScreen(Player.Instance.Position);
            textDraw.Draw(mspamm ? "MasterySpamm ON" : "MasterySpamm OFF", SharpDX.Color.White, (int)pos.X - 45,
                   (int)pos.Y + 40);
        }
    }   
}