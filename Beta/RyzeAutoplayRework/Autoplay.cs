﻿using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using SharpDX;
using System;
using Plugins;
using System.Linq;

namespace Autoplay
{
    class Program : Helper
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnGameLoad;
        }
        private static void Game_OnGameLoad(EventArgs args)
        {
            if (myHero.Team == GameObjectTeam.Order)
            {
                Spawn = new Vector3(422, 433, 183);
            }
            else
            {
                Spawn = new Vector3(14314, 14395, 172);
            }
            new PluginLoader().LoadPlugin();
            new SummSpells().Init();
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Player.OnDamage += Player_OnDamage;
            Write("Ryze injected");   
        }

        private static void Player_OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            if (sender != null)
            {
                if (sender.IsEnemy)
                {
                    if (sender.Type == GameObjectType.obj_AI_Turret)
                    {
                        LeaveTowerPls = true;
                        Pos = sender.Position.Extend(myHero.Position, 1050).To3D();
                        tower = (Obj_AI_Turret)sender;
                        Write("y u do dis tower");
                    }
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            Drawing.DrawCircle(Pos, 150, System.Drawing.Color.Yellow);
            if (ComboPLS)
            {
                Drawing.DrawText(836, 481, System.Drawing.Color.White, "Combo ON");
            }
            if (tower != null && myHero.Distance(tower) < 1500)
            {
                if (LeaveTowerPls)
                {
                    Drawing.DrawCircle(tower.Position, 1000, System.Drawing.Color.Red);
                }
            }
        }
        private static void Game_OnUpdate(EventArgs args)
        {         
            if (tower != null && !tower.IsDead)
            {
                if (myHero.Distance(tower) >= 1050)
                {
                    LeaveTowerPls = false;
                }
            }
            if (tower != null && tower.IsDead)
            {
                LeaveTowerPls = false;
            }

            if (myHero.IsInShopRange())
            {
                RecallNoob = false;
                switch (myHero.ChampionName.ToLower())
                {
                    case "ryze":
                       Core.DelayAction(() => Shop.Items.BuyRyzeItems(), 1000);
                        break;
                }
                if (myHero.HealthPercent < 80)
                {
                    WaitingHealth = true;
                    Orbwalker.DisableMovement = true;
                    Orbwalker.DisableAttacking = true;
                }
                else if (myHero.MaxMana > 100 && myHero.ManaPercent < 80)
                {
                    WaitingHealth = true;
                    Orbwalker.DisableMovement = true;
                    Orbwalker.DisableAttacking = true;
                }
                else
                {
                    WaitingHealth = false;
                    Orbwalker.DisableMovement = false;
                    Orbwalker.DisableAttacking = false;
                }
            }
            var turret = GetClosestTurret(AARange());
            if (turret != null)
            {
                if (Orbwalker.CanAutoAttack)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, turret);
                }
            }
            
            if (myHero.IsDead) return;
            if (myHero.IsRecalling()) { Orbwalker.DisableMovement = true; Orbwalker.DisableAttacking = true; }
            else
            {
                Core.DelayAction(() => Orbwalker.DisableMovement = false, 500);
                Core.DelayAction(() => Orbwalker.DisableAttacking = false, 500);
            }
            Orbwalker.OrbwalkTo(Pos);
            if (!myHero.IsRecalling())
            {
                if (!RecallNoob)
                {
                    Orbwalk.OrbwalkManager();
                    if (!ComboPLS) { Farm(); }
                    Combo();
                }
                RecallManager();            
            }

            if (Environment.TickCount - RandomCheck > 10000)
            {
                random = GetRandompos(50, 200);
                RandomCheck = Environment.TickCount;
            }
        }
        
        
        public static void Recall()
        {
            if (myHero.IsInShopRange() || myHero.IsDead) return;
            if (!Checked)
            {
                RecallNoob = true;
                recall = EntityManager.Turrets.Allies.Where(k => !k.IsDead && k != null && k.BaseSkinName.Contains("Turret")).OrderBy(k => k.Distance(myHero)).First().ServerPosition.Extend(Spawn, 300).To3D();
                Checked = true;
            }
            if (myHero.Distance(recall) > 100)
            {
                Write("Walking to recall point");
                Pos = recall;
            }
            else
            {
                if (!myHero.IsRecalling())
                {
                    myHero.Spellbook.CastSpell(SpellSlot.Recall);
                    Write("Recalling");
                    Checked = false;
                }               
            }
        }

        private static void Farm()
        {
            switch (myHero.ChampionName)
            {
                case "Ryze":
                    Ryze.Farm();
                    break;
            }
        }

        private static void Combo()
        {
            switch (myHero.ChampionName)
            {
                case "Ryze":
                    Ryze.Combo();
                    break;
            }
        }
        private static void RecallManager()
        {
            if (RecallNoob)
            {
                Recall();
            }
            if (myHero.HealthPercent < 35)
            {
                Recall();
            }
            else if (myHero.MaxMana > 100)
            {
                if (myHero.ManaPercent <= 15)
                {
                    Recall();
                }
            }
        }
    }
}