using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;
namespace Modes
{
    using Menu = Settings.HarassSettings;
    class Harass : Helper
    {
        public static void Execute()
        {
            var target = TargetSelector.GetTarget(Lib.E.Range, DamageType.Physical);
            if (target == null) return;
            if (Menu.UseQ)
            {
                if (target.IsValidTarget(Lib.Q.Range))
                {
                    Lib.Q.Cast(target);
                }
            }
            if (Menu.UseW)
            {
                Lib.W.Cast();
            }
            if (Menu.UseE)
            {
                if(!Menu.UseEs)
                {
                    Lib.E.Cast(target);
                }
                else if (Lib.CanStun(target))
                {
                    Lib.E.Cast(target);
                }
            }
        }
    }
}