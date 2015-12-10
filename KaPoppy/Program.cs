using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using SharpDX;
using System;
using Color = System.Drawing.Color;

namespace KaPoppy
{
    using EloBuddy.SDK.Rendering;
    using System.Linq;
    using Config = Settings.MiscSettings;
    class Program : Helper
    {
        private static void Main(string[] args) { Loading.OnLoadingComplete += Game_OnStart; }
        private static void Game_OnStart(EventArgs args)
        {
            if (myHero.Hero != Champion.Poppy) return;
            CheckForUpdates();
            Settings.Init();

            var flash = myHero.Spellbook.Spells.Where(x => x.Name.ToLower().Contains("summonerflash"));
            SpellDataInst Flash = flash.Any() ? flash.First() : null;
            if (Flash != null)
            {
                Lib.Flash = new Spell.Targeted(Flash.Slot, 425);
            }
            Obj_AI_Base.OnProcessSpellCast += Modes.Misc.SpellCast;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawings;
            GameObject.OnCreate += Modes.Misc.AntiRengar;
        }

        private static void Drawings(EventArgs args)
        {
            if (Config.StunTarget)
            {
                var target = TargetSelector.SelectedTarget;
                if (target == null)
                {
                    Drawing.DrawText(Drawing.WorldToScreen(myHero.ServerPosition) - new Vector2(125, 30),
                         Color.Red, "Select a target" + Environment.NewLine + "with left click!", 9);
                }
                else
                {
                    var pos = Lib.PointsAroundTheTarget(target, 525).Where(x => Lib.CanStun(target, x.To2D()) && !IsWall(x)).OrderBy(x => x.Distance(myHero));
                    if (pos.Count() > 0)
                    {
                        var pos1 = Drawing.WorldToScreen(myHero.Position);
                        var pos2 = Drawing.WorldToScreen(pos.First());
                        Line.DrawLine(Color.White, 4,pos1, pos2);
                    }
                }
            }
            if (Config.drawQ)
            {
                Circle.Draw(Lib.Q.IsReady() ? Green : Red, Lib.Q.Range, myHero.Position);
            }
            if (Config.drawE)
            {
                Circle.Draw(Lib.E.IsReady() ? Green : Red, Lib.E.Range, myHero.Position);
            }
            if (Config.drawR)
            {
                Circle.Draw(Lib.R.IsReady() ? Green : Red, Lib.R.MaximumRange, myHero.Position);
            }
            if (Settings.ComboSettings.UseFlashE)
            {
                var pos = Drawing.WorldToScreen(myHero.ServerPosition) - new Vector2(-45, 15);
                Drawing.DrawText(pos, Color.White, "Flash stun!", 9);
            }
        }
        private static void Game_OnUpdate(EventArgs args)
        {
            if (myHero.IsDead) return;
            if (Config.StunTarget)
            {
                Modes.Stun.Execute();
                if (!Orbwalker.DisableAttacking)
                    Orbwalker.DisableAttacking = true;
                if (!Orbwalker.DisableMovement)
                    Orbwalker.DisableMovement = true;
            }
            else
            {
                if (Orbwalker.DisableAttacking)
                    Orbwalker.DisableAttacking = false;
                if (Orbwalker.DisableMovement)
                    Orbwalker.DisableMovement = false;
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                Modes.Combo.Execute();
            
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                Modes.Laneclear.Execute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                Modes.Flee.Execute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                Modes.Harass.Execute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                Modes.Jungleclear.Execute();
            
            Modes.Misc.Execute();
        }
    }    
}