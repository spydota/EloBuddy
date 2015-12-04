using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using SharpDX;
using System;
using Color = System.Drawing.Color;

namespace KaPoppy
{
    using System.Linq;
    using Config = Settings.MiscSettings;
    class Program : Helper
    {
        private static void Main(string[] args) { Loading.OnLoadingComplete += Game_OnStart; }
        private static void Game_OnStart(EventArgs args)
        {
            if (myHero.Hero != Champion.Poppy) return;
            //CheckForUpdates();
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
            if (Config.drawEp)
            {
                foreach (var target in EntityManager.Heroes.Enemies.Where(enemy => !enemy.IsDead && enemy.IsValidTarget(Lib.E.Range * 2)))
                {
                    var flashpos = Lib.PointsAroundTheTarget(target, 500).Where(x => Lib.CanStun(x, target)).OrderBy(x => x.Distance(myHero));
                    if (flashpos.Count() > 0)
                    {
                        Drawing.DrawCircle(flashpos.First(), 50, Color.Red);
                        Drawing.DrawLine(target.Position.WorldToScreen(), flashpos.First().WorldToScreen(), 2f, Color.Red);
                    }
                }
            }
            

            if (Config.drawQ)
            {
                Drawing.DrawCircle(myHero.Position, Lib.Q.Range, Lib.Q.IsReady() ? Color.White : Color.Tomato);
            }
            if (Config.drawE)
            {
                Drawing.DrawCircle(myHero.Position, Lib.E.Range, Lib.E.IsReady() ? Color.White : Color.Tomato);
            }
            if (Config.drawR)
            {
                Drawing.DrawCircle(myHero.Position, Lib.R.Range, Lib.R.IsReady() ? Color.White : Color.Tomato);
            }

            if (Config.AutoHarass)
            {
                var pos = Drawing.WorldToScreen(myHero.ServerPosition) - new Vector2(-45, 30);
                Drawing.DrawText(pos, Color.White, "Harass ON", 9);
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

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                Modes.Combo.Execute();
            
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                Modes.Laneclear.Execute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
                Modes.Flee.Execute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) || Config.AutoHarass)
                Modes.Harass.Execute();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                Modes.Jungleclear.Execute();
            
            Modes.Misc.Execute();
        }

    }    
}