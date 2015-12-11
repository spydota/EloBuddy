using System;
using System.Diagnostics;
using System.IO;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.Sandbox;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Collections.Generic;
using EloBuddy.SDK;
using System.Linq;

namespace LegendOfPoroKing
{
    public static class Program
    {
        static void Main(string[] args) { Loading.OnLoadingComplete += Loading_OnLoadingComplete; ; }
        static Menu Menu;
        static CheckBox draw;
        static KeyBind poro, poro1;
        static Spell.Skillshot PoroThrow = null;
        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Game.Type != GameType.KingPoro) return;

            Chat.Print("LegendOfPoroKing loaded", System.Drawing.Color.White);

            Menu = MainMenu.AddMenu("LegendOfPoroKing", "plasma");
            draw = Menu.Add("Draw", new CheckBox("Draw poros"));
            Menu.AddGroupLabel("Select your target with left click first!");
            poro = Menu.Add("Poro", new KeyBind("Throw poros (Hold)", false, KeyBind.BindTypes.HoldActive, 'Z'));
            poro1 = Menu.Add("Poro1", new KeyBind("Throw poros (Toggle)", false, KeyBind.BindTypes.PressToggle,'I'));
            var q = Menu.Add("color", new Slider("Color", 0, 0, 3));
            var Colors = new[]
            {
                "White", "Red", "Green", "Yellow"
            };
            q.DisplayName = "Color: " + Colors[q.CurrentValue];
            
            q.OnValueChange += delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
            {
                q.DisplayName = "Color: " + Colors[changeArgs.NewValue];
            };


            if (Player.GetSpell(SpellSlot.Summoner1).Name == "summonerporothrow")
                PoroThrow = new Spell.Skillshot(SpellSlot.Summoner1, 2500, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 0, 1300, 50);
            else if (Player.GetSpell(SpellSlot.Summoner2).Name == "summonerporothrow")
                PoroThrow = new Spell.Skillshot(SpellSlot.Summoner2, 2500, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 0, 1300, 50);

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnProcessSpellCast;
            GameObject.OnDelete += GameObject_OnDelete;
            GameObject.OnCreate += GameObject_OnCreate;
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name == "Poro_Throw_Mis.troy")
            {
                foreach (var porito in Poros.Where(x => x.start.IsValid() && x.end.IsValid() && x.Obj == null))
                {
                    porito.Obj = sender;
                    porito.NetworkID = sender.NetworkId;
                }
            }
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            if (sender.Name == "Poro_Throw_Mis.troy")
            {
                foreach (var porito in Poros.Where(x => x.start.IsValid() && x.end.IsValid() && x.Obj != null))
                {
                    for (int i = 0; i < Poros.Count; i++)
                    {
                        if (Poros[i].NetworkID == sender.NetworkId)
                        {
                            Poros.RemoveAt(i);
                            return;
                        }
                    }
                }
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {

                if (args.SData.Name == "summonerporothrow")
                {
                    Poros.Add(
                        new Poro
                        {
                            start = args.Start,
                            end = args.End //Start.Extend(args.End, 2500).To3D()
                        });
                }
        }

        //kingporo_porofollower
        private static void Game_OnUpdate(EventArgs args)
        {
            if (PoroThrow.Name == "summonerporothrow")
            {
                if (poro.Cast<KeyBind>().CurrentValue || poro1.Cast<KeyBind>().CurrentValue)
                {
                    var target = TargetSelector.GetTarget(PoroThrow.Range, DamageType.True);
                    if (target == null) return;
                    if (PoroThrow.GetPrediction(target).HitChance >= EloBuddy.SDK.Enumerations.HitChance.High)
                    {
                        PoroThrow.Cast(target);
                    }
                }
            }
        }

        //KINGPORO_PoroFollower
        //summonerporothrow
        public static readonly List<Poro> Poros = new List<Poro>();
        private static void Drawing_OnDraw(EventArgs args)
        {
            if (draw.Cast<CheckBox>().CurrentValue)
            {
                foreach (var porito in Poros)
                {
                    if (porito.start.IsValid() && porito.end.IsValid() && porito.Obj != null)
                    {
                        new Geometry.Polygon.Rectangle(porito.Obj.Position, porito.end, 50).Draw(Color(), 2);
                    }
                }
            }
        }
        static System.Drawing.Color Color()
        {
            switch(Menu["color"].DisplayName)
            {
                case "Color: White":
                    return System.Drawing.Color.White;
                case "Color: Red":
                    return System.Drawing.Color.Red;
                case "Color: Green":
                    return System.Drawing.Color.Green;
                case "Color: Yellow":
                    return System.Drawing.Color.Yellow;
            }
            return System.Drawing.Color.White;
        }
    }
}
