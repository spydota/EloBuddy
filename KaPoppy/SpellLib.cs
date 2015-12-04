using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
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
        static Lib()
        {
            uint idk = 0;
            Q = new Spell.Skillshot(SpellSlot.Q, idk, SkillShotType.Linear, 250, null, (int)idk);
            { Q.AllowedCollisionCount = int.MaxValue; }
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 525);
            R = new Spell.Chargeable(SpellSlot.R, idk, idk, (int)idk, (int)idk, int.MaxValue, (int)idk);
            Flash = null;
        }

        public static int PassiveCount()
        {
            return ObjectManager.Player.GetBuffCount("poppyparagonstats");
        }
        public static bool CanStun(Obj_AI_Base hero)
        {
            if (hero.HasBuffOfType(BuffType.Invulnerability) || hero.HasBuffOfType(BuffType.Invisibility) || hero.HasBuffOfType(BuffType.SpellImmunity))
                return false;

            var prediction = Prediction.Position.PredictUnitPosition(hero, 500);
            var prediction2 = hero.ServerPosition;
            for (var i = 0; i < 300 - 10; i += 10)
            {
                var flags = NavMesh.GetCollisionFlags
                    (
                        ObjectManager.Player.ServerPosition.Extend(prediction, ObjectManager.Player.Distance(prediction) + i)
                    );
                var flags2 = NavMesh.GetCollisionFlags
                    (
                        ObjectManager.Player.ServerPosition.Extend(prediction2, ObjectManager.Player.Distance(prediction2) + i)
                    );
                if ((flags.HasFlag(CollisionFlags.Wall) || flags.HasFlag(CollisionFlags.Building)) &&
                    (flags2.HasFlag(CollisionFlags.Wall) || flags2.HasFlag(CollisionFlags.Building)))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CanStun(Vector3 source, Obj_AI_Base hero)
        {
            if (hero.HasBuffOfType(BuffType.Invulnerability) || hero.HasBuffOfType(BuffType.Invisibility) || hero.HasBuffOfType(BuffType.SpellImmunity))
                return false;

            var prediction = Prediction.Position.PredictUnitPosition(hero, 500);
            var prediction2 = hero.ServerPosition;
            for (var i = 0; i < 300 - 10; i += 10)
            {
                var flags = NavMesh.GetCollisionFlags
                    (
                        source.Extend(prediction, source.Distance(prediction) + i)
                    );
                var flags2 = NavMesh.GetCollisionFlags
                    (
                        source.Extend(prediction2, source.Distance(prediction2) + i)
                    );
                if ((flags.HasFlag(CollisionFlags.Wall) || flags.HasFlag(CollisionFlags.Building)) &&
                    (flags2.HasFlag(CollisionFlags.Wall) || flags2.HasFlag(CollisionFlags.Building)))
                {
                    return true;
                }
            }
            return false;
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

    }
}