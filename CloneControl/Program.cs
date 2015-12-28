using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Drawing;
using System.Linq;

namespace MasterySpammer
{
    class Program
    {
        //Requested by mklimek03

        static readonly string[] Modes = new string[] { "Move to nearest ally tower", "Move to mouse", "Move to enemy" };
        static CheckBox Control;
        static Slider Mode;
        static Slider Mode2;
        static Slider Range;
        static Menu CloneMenu;
        static GameObject sClone = null;
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }
        private static void Game_OnStart(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Shaco && Player.Instance.Hero != Champion.Leblanc)
                return;

            CloneMenu = MainMenu.AddMenu("Clone control", "yodagodEOQ");
            Chat.Print(Player.Instance.ChampionName + " clone controller loaded", Color.White);

            Control = CloneMenu.Add("Clone", new CheckBox("Control clone"));
            CloneMenu.AddGroupLabel("You can select a target with left click");
            Mode = CloneMenu.Add("Mode", new Slider("", 2, 0, 2));
            Mode.DisplayName = "If health > 40%:" + Modes[Mode.CurrentValue];
            Mode.OnValueChange += delegate
                (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs Args)
            {
                sender.DisplayName = "If health > 40%:" + Modes[Args.NewValue];
            };

            Mode2 = CloneMenu.Add("Mode2", new Slider("", 0, 0, 2));
            Mode2.DisplayName = "If health < 40%:" + Modes[Mode2.CurrentValue];
            Mode2.OnValueChange += delegate
                (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs Args)
            {
                sender.DisplayName = "If health < 40%:" + Modes[Args.NewValue];
            };
            Range = CloneMenu.Add("Range", new Slider("Get targets within {0} range", 2000, 1, 10000));
            

            Game.OnUpdate += ControlClone;

            GameObject.OnCreate += CreateClone;
            GameObject.OnDelete += DeleteClone;
        }

        private static void CreateClone(GameObject sender, EventArgs args)
        {
            if (sender.Name == Player.Instance.Name)
            {
                sClone = sender;
            }
        }

        private static void DeleteClone(GameObject sender, EventArgs args)
        {
            if (sender.Name == Player.Instance.Name)
            {
                sClone = null;
            }
        }

        private static void ControlClone(EventArgs args)
        {
            if (sClone == null || !Control.CurrentValue) return;
            if (Player.Instance.HealthPercent > 40)
            {
                switch (Mode.CurrentValue)
                {
                    case 0:
                        MoveToTurret();
                        break;
                    case 1:
                        MoveToMouse();
                        break;
                    case 2:
                        MoveToEnemy();
                        break;
                }
            }
            else
            {
                switch (Mode2.CurrentValue)
                {
                    case 0:
                        MoveToTurret();
                        break;
                    case 1:
                        MoveToMouse();
                        break;
                    case 2:
                        MoveToEnemy();
                        break;
                }
            }
        }

        private static void MoveToEnemy()
        {
            var target = TargetSelector.SelectedTarget ??
                TargetSelector.GetTarget(Range.CurrentValue, DamageType.Magical);
            if (target == null)
                MoveToMouse();

            Player.IssueOrder(GameObjectOrder.MovePet, target.Position);
        }

        private static void MoveToMouse()
        {
            Player.IssueOrder(GameObjectOrder.MovePet, Game.CursorPos);
        }

        private static void MoveToTurret()
        {
            Obj_AI_Turret turret = EntityManager.Turrets.Allies.Where(x => !x.IsDead).OrderBy(x => x.Distance(sClone)).FirstOrDefault();
            if (turret == null)
                MoveToMouse();
            Player.IssueOrder(GameObjectOrder.MovePet, turret.Position.Extend(Player.Instance, new Random().Next((int)turret.BoundingRadius, 400)).To3D());
        }
    }
}