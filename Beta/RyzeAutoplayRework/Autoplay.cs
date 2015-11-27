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
            Write("Ryze injected");
           random = GetRandompos(-150, 150);
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
                        tower = (Obj_AI_Turret)sender;
                        Write("y u do dis tower");
                    }
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            Drawing.DrawCircle(Pos + random, 150, System.Drawing.Color.Yellow);
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
                if (!tower.IsDead)
                {
                    if (myHero.Distance(tower) >= 1000)
                    {
                        LeaveTowerPls = false;
                    }
                }
                else
                {
                    LeaveTowerPls = false;
                }
                if (LeaveTowerPls)
                {
                    Pos = tower.ServerPosition.Extend(myHero.ServerPosition, 1200).To3D();
                }
            }
            
            if (myHero.IsInShopRange())
            {
                RecallNoob = false;
                
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
            if (myHero.IsInShopRange() || myHero.IsDead)
            {
                switch (myHero.ChampionName.ToLower())
                {
                    case "ryze":
                        Core.DelayAction(() => Shop.Items.BuyRyzeItems(), 1000);
                        break;
                }
            }
            
            if (myHero.IsDead) return;

            var turret = GetClosestTurret(AARange());
            if (turret != null)
            {
                if (Orbwalker.CanAutoAttack)
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, turret);
                }
            }
            Orbwalker.OrbwalkTo(Pos + random);
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
            if (RecallNoob)
            {
                switch(myHero.ChampionName)
                {
                    case "Ryze":
                        Ryze.RyzeRecall();
                        break;
                }
                Orbwalker.DisableMovement = true;
                Orbwalker.DisableAttacking = true;
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
            if (myHero.Distance(recall) > 50 + myHero.BoundingRadius)
            {
                Write("Walking to recall point");
                Player.IssueOrder(GameObjectOrder.MoveTo, recall);
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
                RecallNoob = true;
            }
            else if (myHero.MaxMana > 100)
            {
                if (myHero.ManaPercent <= 15)
                {
                    RecallNoob = true;
                }
            }
        }
    }
}