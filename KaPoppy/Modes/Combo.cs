using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;
namespace Modes
{
    using System.Linq;
    using Menu = Settings.ComboSettings;
    class Combo : Helper
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(1200, DamageType.Physical);
            if (target == null) return;

            if (Menu.UseW)
            {
                if (!Lib.E.IsInRange(target) || (!Lib.E.IsReady() && !myHero.IsInAutoAttackRange(target)))
                Lib.W.Cast();
            }

            if (Menu.UseE)
            {
                if (target.IsValidTarget(Lib.E.Range) && Lib.E.IsReady())
                {
                    if (!Menu.UseEs)
                    {
                        Lib.E.Cast(target);
                    }
                    else
                    {
                        if (Lib.CanStun(target))
                        {
                            Lib.E.Cast(target);
                        }
                        else if (Menu.UseFlashE)
                        {
                            if (Lib.Flash == null) return;
                            var flashpos = Lib.PointsAroundTheTarget(target, 500).Where(x => Lib.CanStun(x, target)).OrderBy(x => x.Distance(myHero));

                            if (flashpos.Count() > 0)
                            {
                                if (Lib.Flash.IsReady() && Lib.Flash.IsInRange(flashpos.First()))
                                {
                                    Lib.Flash.Cast(flashpos.First());
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

