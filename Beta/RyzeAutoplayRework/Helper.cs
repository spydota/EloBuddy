using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autoplay
{
    public class Helper
    {
        public static AIHeroClient myHero { get { return ObjectManager.Player; } }
        public static Obj_AI_Minion minion = null;
        public static readonly Vector3 Toplane = new Vector3(1117, 10878, 53);
        public static Vector3 Spawn;
        public static int random;
        public static int RandomCheck = 0;
        public static bool WalkingToRecall = false;
        public static bool ChangedToAllies = false;
        public static bool WaitingHealth = false;
        public static int Tick = 0;
        public static bool Once = false;
        public static bool Checked = false;
        public static bool CheckedH = false;
        public static bool CheckedC = false;
        public static bool LeaveTowerPls = false;
        public static int lastTurret = 0;
        public static Vector3 recall = new Vector3();
        private static string[] SmiteNames = new[] { "s5_summonersmiteplayerganker", "itemsmiteaoe", "s5_summonersmitequick", "s5_summonersmiteduel", "summonersmite" };
        public static Vector3 Pos;
        //CT-SummonerRift
        #region Turrets
        public static List<Obj_AI_Turret> GetEnemyTurrets()
        {
            return (EntityManager.Turrets.Enemies.ToList());
        }
        public static List<Obj_AI_Turret> GetAllyTurrets()
        {
            return (EntityManager.Turrets.Allies.ToList());
        }
        public static List<Obj_AI_Turret> GetTopEnemyTurrets()
        {

            List<Vector3> TPoints = null;
            if (ObjectManager.Player.Team == GameObjectTeam.Order)
            {
                // Enemy == RED
                TPoints = new List<Vector3>
                {
                    new Vector3(4337, 13817, 52),
                    new Vector3(7950, 13382, 52),
                    new Vector3(10446, 13645, 95)
                };
            }
            else
            {
                // Enemy == BLUE
                TPoints = new List<Vector3>
                {
                    new Vector3(973, 10446, 52),
                    new Vector3(1509, 6705, 52),
                    new Vector3(1155, 4270, 95)
                };
            }


            List<Obj_AI_Turret> Turrets = GetEnemyTurrets();
            List<Obj_AI_Turret> Temp = new List<Obj_AI_Turret>();

            foreach (Obj_AI_Turret t in Turrets)
            {
                foreach (Vector3 p in TPoints)
                {
                    if (t.Distance(p) < 500 && !t.IsDead)
                    {
                        Temp.Add(t);
                    }
                }
            }//foreach

            return (Temp.Count > 0 ? Temp : null);
        }

        public static Obj_AI_Turret GetTopAllyTurret()
        {
            List<Vector3> aTPoints = null;

            if (ObjectManager.Player.Team == GameObjectTeam.Order)
                // Enemy == RED
                aTPoints = new List<Vector3>
                {
                    new Vector3(973, 10446, 52),
                    new Vector3(1509, 6705, 52),
                    new Vector3(1155, 4270, 95)
                };

            else
            {
                // Enemy == BLUE
                aTPoints = new List<Vector3>
                {
                    new Vector3(4337, 13817, 52),
                    new Vector3(7950, 13382, 52),
                    new Vector3(10446, 13645, 95)
                };
            }
            List<Obj_AI_Turret> Turrets = GetEnemyTurrets();
            List<Obj_AI_Turret> Temp = new List<Obj_AI_Turret>();

            foreach (Obj_AI_Turret t in Turrets)
            {
                foreach (Vector3 p in aTPoints)
                {
                    if (t.Distance(p) < 500)
                    {
                        Temp.Add(t);
                    }
                }
            }//foreach

            return (Temp.Count > 0 ? Temp[0] : null);
        }
        public static Obj_AI_Turret GetClosestTurret(float Range)
        {
            List<Obj_AI_Turret> T =
                EntityManager.Turrets.Enemies
                .Where(t => t.Distance(ObjectManager.Player.Position) < Range && !t.IsDead).ToList();

            return (T.Count > 0 ? T[0] : null);
        }
        public static Obj_AI_Turret ClosestAllyTurret(float Range)
        {
            List<Obj_AI_Turret> T =
                EntityManager.Turrets.Allies
                .Where(t => t.Distance(ObjectManager.Player.Position) < Range && !t.IsDead).ToList();

            return (T.Count > 0 ? T.Last() : null);
        }

        #endregion
        #region Minions
        public static List<Obj_AI_Minion> GetEnemyMinions()
        {
            return (EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => !t.IsDead).ToList());
        }
        public static float AARange()
        {
            return myHero.GetAutoAttackRange() - 10;
        }
        public static List<Obj_AI_Minion> GetAllyMinions()
        {
            return (EntityManager.MinionsAndMonsters.AlliedMinions.Where(t => !t.IsDead).ToList());
        }
        public static Obj_AI_Minion GetClosestMinion(float Range)
        {
            List<Obj_AI_Minion> T =
                EntityManager.MinionsAndMonsters.EnemyMinions
                .Where(t => t.Distance(ObjectManager.Player.Position) < Range && !t.IsDead).ToList();


            if (T.Count > 0)
            {
                float Dist = Player.Instance.Distance(T[0].Position);
                int Index = 0;

                for (int i = 0; i < T.Count; i++)
                {
                    float D = Player.Instance.Distance(T[i].Position);
                    if (Dist > D)
                    {
                        Dist = D;
                        Index = i;
                    }

                }
                return T[Index];
            }
            return null;
        }
        public static Obj_AI_Minion GetClosestTopMinion(float Range)
        {
            List<Obj_AI_Minion> T =
                EntityManager.MinionsAndMonsters.EnemyMinions
                .Where(t => t.Distance(ObjectManager.Player.Position) < Range && !t.IsDead).ToList();


            if (T.Count > 0)
            {
                float Dist = Toplane.Distance(T[0].Position);
                int Index = 0;

                for (int i = 0; i < T.Count; i++)
                {
                    float D = Toplane.Distance(T[i].Position);
                    if (Dist > D)
                    {
                        Dist = D;
                        Index = i;
                    }

                }
                return T[Index];
            }
            return null;
        }
        public static Obj_AI_Minion GetClosestAllyMinion(float Range)
        {
            List<Obj_AI_Minion> T =
                EntityManager.MinionsAndMonsters.AlliedMinions
                .Where(t => t.Distance(ObjectManager.Player.Position) < Range && !t.IsDead).ToList();


            if (T.Count > 0)
            {
                float Dist = Player.Instance.Distance(T[0].Position);
                int Index = 0;

                for (int i = 0; i < T.Count; i++)
                {
                    float D = Player.Instance.Distance(T[i].Position);
                    if (Dist > D)
                    {
                        Dist = D;
                        Index = i;
                    }

                }
                return T[Index];
            }
            return null;
        }
        #endregion


        public static AIHeroClient GetNearestAlly()
        {
            //Like old times Kappa
            var ally = EntityManager.Heroes.Allies.Where(x => !x.IsMe && !x.IsInShopRange() && !x.IsDead && !SmiteNames.Contains(x.Spellbook.GetSpell(SpellSlot.Summoner1).Name) &&
            !SmiteNames.Contains(x.Spellbook.GetSpell(SpellSlot.Summoner2).Name)).OrderBy(x => x.Distance(myHero)).First();

            return ally;
        }

        private static int tick = Environment.TickCount;
        private static string Text = "";
        public static void Write(string text)
        {
            if (text == Text)
            {
                if (Environment.TickCount - tick > 5000)
                {
                    Console.WriteLine(text);
                    Text = text;
                    tick = Environment.TickCount;
                }
            }
            else
            {
                Console.WriteLine(text);
                Text = text;
                tick = Environment.TickCount;
            }
        }
        public static int GetRandompos(int min, int max)
        {
            var random = new Random().Next(min, max);
            return random;
        }
    }
}


