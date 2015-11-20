using EloBuddy;
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
        }

        private static void Player_OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            if (sender.IsEnemy)
            {
                if (sender.Type == GameObjectType.obj_AI_Turret || sender.Type == GameObjectType.obj_Turret)
                {
                    LeaveTowerPls = true;
                    Pos = Spawn;
                    tower = (Obj_AI_Turret)sender;
                    Write("y u do dis tower");
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
            if (tower != null)
            {
                if (myHero.Distance(tower) > 1000)
                {
                    LeaveTowerPls = false;
                }
            }

            if (myHero.IsInShopRange())
            {
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
                }
                else if (myHero.MaxMana > 100 && myHero.ManaPercent < 80)
                {
                    WaitingHealth = true;
                    Orbwalker.DisableMovement = true;
                }
                else
                {
                    WaitingHealth = false;
                    if (Orbwalker.DisableMovement) { Orbwalker.DisableMovement = false; }
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
            else if (Orbwalker.DisableMovement) {
                Core.DelayAction(() => Orbwalker.DisableMovement = false, 500);
                Core.DelayAction(() => Orbwalker.DisableAttacking = false, 500);
            }
            Orbwalker.OrbwalkTo(Pos);
            if (!myHero.IsRecalling())
            {
                OrbwalkManager();
                if (!ComboPLS) { Farm(); }
                Combo();
                RecallManager();
            }
            if (Environment.TickCount - RandomCheck > 50000)
            {
                random = GetRandompos(50, 150);
                RandomCheck = Environment.TickCount;
            }
        }
        private static void OrbwalkManager()
        {
            if (WaitingHealth || LeaveTowerPls) return;

            var enemy = TargetSelector.GetTarget(900, DamageType.Magical);
            if (enemy != null)
            {
                if (ComboPLS)
                {
                    if (myHero.Distance(enemy) > 500)
                    {
                        Pos = enemy.ServerPosition.Extend(myHero, 450).To3D();
                    }
                }
                else if (enemy.Distance(myHero) < enemy.GetAutoAttackRange() + 50)
                {
                    Pos = GetTopAllyTurret().Position;
                }
            }
            var turret = GetTopAllyTurret();
            var minion = GetClosestMinion(2000);
            var allyminion = GetClosestAllyMinion(2000);
            var ally = GetNearestAlly();
            if (myHero.CountEnemiesInRange(3000) <= 3 && !ChangedToAllies)
            {
                if (minion != null)
                {
                    if (turret != null)
                    {
                        Pos = minion.ServerPosition.Extend(GetTopAllyTurret(), 500).To3D();
                    }
                    else
                    {
                        Pos = minion.ServerPosition.Extend(Spawn, 500).To3D();
                    }
                }
                else if (allyminion != null)
                {
                    Pos = allyminion.Position - random;
                }
                else if (turret != null && myHero.Distance(turret) > 500)
                {
                    Pos = turret.ServerPosition;
                }
                else { Write("Waiting minions"); }
            }
            else if (ally != null)
            {
                if (!Once)
                {
                    Write("Following allies...");
                    ChangedToAllies = true;
                    Tick = Environment.TickCount;
                    Once = true;
                }
                if (ally.IsMelee)
                {
                    Pos = ally.Position - random;
                }
                else
                {
                    Pos = ally.Position + random;
                }
                if (Environment.TickCount - Tick > 50000)
                {
                    Write("Can follow now");
                    ChangedToAllies = false;
                    Once = false;
                }
            }
        }
        
        private static void Recall()
        {
            if (myHero.IsInShopRange() || myHero.IsDead) return;
            if (!Checked)
            {
                recall = EntityManager.Turrets.Allies.Where(k => !k.IsDead && k != null && k.BaseSkinName.Contains("Turret")).OrderBy(k => k.Distance(myHero)).First().ServerPosition.Extend(Spawn, 300).To3D();
                Checked = true;
            }
            if (myHero.Distance(recall) > 200)
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
            if (myHero.HealthPercent < 35)
            {
                Recall();
            }
            else if (myHero.MaxMana > 100)
            {
                if (myHero.ManaPercent <= 20)
                {
                    Recall();
                }
            }
        }
    }
}