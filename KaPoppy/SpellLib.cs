using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;
using System;
using System.Collections.Generic;

namespace KaPoppy
{
    class Lib
    {
        public static Spell.Skillshot Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Chargeable R;
        public static Spell.Targeted Flash;
        public static GameObject Passive = null;
        static Lib()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 430, SkillShotType.Linear, 250, null, 100);
            Q.AllowedCollisionCount = int.MaxValue;
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Targeted(SpellSlot.E, 525);
            R = new Spell.Chargeable(SpellSlot.R, 500, 1200, 4000, 250, int.MaxValue, 90);
            R.AllowedCollisionCount = int.MaxValue;
            Flash = null;
        }
        public static List<Vector3> PointsAroundTheTarget(AttackableUnit target, float dist)
        {
            if (target == null)
            {
                return new List<Vector3>();
            }
            List<Vector3> list = new List<Vector3>();
            var newPos = new Vector3();
            var prec = 15;
            if (dist > 1)
            {
                prec = 30;
            }
            var k = (float)((2 * dist * Math.PI) / prec);
            for (int i = 1; i < prec + 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var perimeter =
                        target.Position.Extend(
                            new Vector3(target.Direction.X, target.Direction.Y, target.Position.Z), dist);
                    newPos = new Vector3(perimeter.X + 65 * j, perimeter.Y + 65 * j, target.Position.Z);
                    var rotated = newPos.To2D().RotateAroundPoint(target.Position.To2D(), k * i).To3D();
                    list.Add(rotated);
                }
            }
            return list;
        }

        public static bool CanStun(AIHeroClient unit)
        {
            if (unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield) || Player.Instance.IsDashing()) return false;
            var prediction = Prediction.Position.PredictUnitPosition(unit, 400);
            var predictionsList = new List<Vector3>
                        {
                            unit.ServerPosition,
                            unit.Position,
                            prediction.To3D(),
                        };

            var wallsFound = 0;
            foreach (var position in predictionsList)
            {
                for (var i = 0; i < 300; i += (int) unit.BoundingRadius)
                {
                    var cPos = Player.Instance.Position.Extend(position, Player.Instance.Distance(position) + i).To3D();
                    if (Helper.IsWall(cPos))
                    {
                        wallsFound++;
                        break;
                    }
                }
            }
            if ((wallsFound / predictionsList.Count) >= Settings.MiscSettings.StunPercent / 100f)
            {
                return true;
            }

            return false;
        }
        public static bool CanStun(AIHeroClient unit, Vector2 pos)
        {
            if (unit.HasBuffOfType(BuffType.SpellImmunity) || unit.HasBuffOfType(BuffType.SpellShield)) return false;
            var prediction = Prediction.Position.PredictUnitPosition(unit, 400);
            var predictionsList = new List<Vector3>
                        {
                            unit.ServerPosition,
                            unit.Position,
                            prediction.To3D(),
                        };

            var wallsFound = 0;
            foreach (var position in predictionsList)
            {
                for (var i = 0; i < 300; i += (int) unit.BoundingRadius)
                {
                    var cPos = pos.Extend(position, pos.Distance(position) + i).To3D();
                    if (Helper.IsWall(cPos))
                    {
                        wallsFound++;
                        break;
                    }
                }
            }
            if ((wallsFound / predictionsList.Count) >= Settings.MiscSettings.StunPercent / 100f)
            {
                return true;
            }

            return false;
        }
        public static float GetPassiveDamage(Obj_AI_Base unit)
        {
            return Player.Instance.CalculateDamageOnUnit(unit, DamageType.Magical, 9 * Player.Instance.Level + 20);
        }
    }
}