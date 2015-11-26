using EloBuddy;
using EloBuddy.SDK;
using System.Linq;
using System;

namespace Autoplay
{
    class Orbwalk : Helper
    {
        public static void OrbwalkManager()
        {
            if (WaitingHealth || LeaveTowerPls) return;

            var turret = GetTopAllyTurret();
            var minion = GetClosestMinion(2000);
            var allyminion = GetClosestAllyMinion(2000);
            var ally = GetNearestAlly(3000);
            var enemy = TargetSelector.GetTarget(900, DamageType.Magical);
            if (enemy != null && ComboPLS)
            {
                if (myHero.Distance(enemy) > 400)
                {
                    Pos = enemy.ServerPosition.Extend(myHero.ServerPosition, 350).To3D();
                }
            }
            else if (enemy != null && myHero.Distance(enemy) < enemy.GetAutoAttackRange() + 50)
            {         
                if (enemy.Distance(myHero) < enemy.GetAutoAttackRange() + 100)
                {
                    Pos = GetTopAllyTurret().Position;
                }
            }
            else if (myHero.CountEnemiesInRange(3000) < 3 && !ChangedToAllies)
            {
                if (minion != null)
                {
                    if (turret != null)
                    {
                        Pos = minion.ServerPosition.Extend(turret, 500).To3D();
                    }
                    else
                    {
                        Pos = minion.ServerPosition.Extend(Spawn, 500).To3D();
                    }
                }
                else if (allyminion != null)
                {
                    if (myHero.Distance(allyminion.Position) > 100)
                    {
                        Pos = allyminion.Position;
                    }
                }
                else if (turret != null && myHero.Distance(turret) > 500)
                {
                    Pos = turret.ServerPosition.Extend(Spawn, 500 + random).To3D();
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
                    Pos = ally.Position;
                }
                else
                {
                    Pos = ally.Position;
                }
                if (Environment.TickCount - Tick > 50000)
                {
                    Write("Can follow now");
                    ChangedToAllies = false;
                    Once = false;
                }
            }
            else
            {
                Pos = ClosestAllyTurret(int.MaxValue).Position.Extend(Spawn, 300).To3D();
                ChangedToAllies = false;
            }

            if (myHero.Distance(Pos + random) <= myHero.BoundingRadius + 10)
            {
                random = GetRandompos(-150, 150);
            }
        }
    }
}