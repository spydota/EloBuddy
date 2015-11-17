using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using Plugins;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                Spawn = new Vector3(465, 670, 183);
            }
            else
            {
                Spawn = new Vector3(14090, 14248, 172);
            }
            Drawing.OnDraw += Drawing_OnDraw;
           //AttackableUnit.OnDamage += OnDamage;
            Game.OnUpdate += Game_OnUpdate;
        }
        private static void Drawing_OnDraw(EventArgs args)
        {
            Drawing.DrawCircle(Pos, 150, System.Drawing.Color.Yellow);
        }
      /*  private static void OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            if (sender != null &&
                sender.Type == GameObjectType.obj_AI_Turret
                && args.Target.NetworkId == Player.Instance.NetworkId)
            {
                LeaveTowerPls = true;
                lastTurret = sender.NetworkId;
                Pos = myHero.ServerPosition.Extend(GetClosestMinion(int.MaxValue), myHero.Distance(GetClosestMinion(int.MaxValue)) + 30).To3D();
            }
            Write("Tower hiting me!");
        }  Broken */
        private static void Game_OnUpdate(EventArgs args)
        {
            var turret = GetClosestTurret(AARange());
            if (turret != null)
            {
                if (Orbwalker.CanAutoAttack)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, turret);
                }
            }
            if (myHero.IsInShopRange())
            {
                WalkingToRecall = false;
                if (myHero.HealthPercent < 90 )
                {
                    WaitingHealth = true;
                    Player.IssueOrder(GameObjectOrder.Stop, myHero.Position);
                }
                else
                {
                    WaitingHealth = false;
                }
            }
            if (myHero.IsDead) return;
            if (myHero.IsRecalling()) { Orbwalker.DisableMovement = true; }
            else if (Orbwalker.DisableMovement) { Orbwalker.DisableMovement = false; }
            switch (myHero.ChampionName)
            {
                case "Ryze":
                    Items.BuyRyzeItems();
                    break;
            }
            Orbwalker.OrbwalkTo(Pos);
            if (!myHero.IsRecalling())
            {
                OrbwalkManager();
                Harass();
                Farm();
                RecallManager();
            }
            if (Environment.TickCount - RandomCheck > 50000)
            {
                random = GetRandompos(50, 150);
                RandomCheck = Environment.TickCount;
            }

          /*  var turret = EntityManager.Turrets.Enemies.Where(x => x.NetworkId == lastTurret);
            if (turret.Count() > 0)
            {
                if (myHero.Distance(turret.First()) > 1000)
                {
                    LeaveTowerPls = false;
                    lastTurret = 0;
                }
            } */
        }
        private static void OrbwalkManager()
        {
            if (LeaveTowerPls || WalkingToRecall || WaitingHealth) return;
            var enemy = TargetSelector.GetTarget(500, DamageType.Magical);
            if (enemy != null && myHero.Distance(enemy) < 300)
            {
                Write("Enemy near, walking to turret");
                Pos = myHero.ServerPosition.Extend(ClosestAllyTurret(int.MaxValue), 450).To3D();
            }

            var turret = GetTopAllyTurret();
            var minion = GetClosestMinion(2000);
            var allyminion = GetClosestAllyMinion(5000);
            var ally = GetNearestAlly();
            if (Toplane.CountEnemiesInRange(6000) <= 2 && !ChangedToAllies)
            {
                if (minion != null)
                {
                    var pos = minion.ServerPosition.Extend(Spawn, 500);
                    Pos = (pos).To3D();
                }
                else if (allyminion != null)
                {
                    Pos = allyminion.Position - random - 10;
                }
                else if (turret != null && myHero.Distance(turret) > 500)
                {
                    Pos = turret.ServerPosition;
                }
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
                recall = EntityManager.Turrets.Allies.Where(k => !k.IsDead && k != null).OrderBy(k => k.Distance(myHero)).First().ServerPosition.Extend(Spawn, 300).To3D();

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
            minion = GetClosestMinion(8000);
            if (minion == null) return;
            if (myHero.IsInAutoAttackRange(minion))
            {
                switch (myHero.ChampionName)
                {
                    case "Ryze":
                        Ryze.Farm(minion);
                        break;
                }
            }
            else if (myHero.Distance(minion) < 2000)
            {
                Pos = minion.ServerPosition.Extend(GetClosestAllyMinion(8000).Position + new Vector3(10, 50, 0), AARange()).To3D();
            }
        }

        private static void Harass()
        {
            switch (myHero.ChampionName)
            {
                case "Ryze":
                    Ryze.Harass();
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
            if (myHero.HealthPercent < 20)
            {
                Recall();
            }
            else if (myHero.MaxMana > 100)
            {
                if (myHero.ManaPercent < 20)
                {
                    Recall();
                }
            }
        }
    }
}