using EloBuddy.SDK;
using KaPoppy;
using System.Linq;

namespace Modes
{
    using Menu = Settings.JungleclearSettings;
    class Jungleclear : Helper
    {
        public static void Execute()
        {
            var target = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(minion => !minion.IsDead && minion.IsValidTarget(Lib.E.Range)).First();
            if (target == null) return;

            if (Menu.UseQ)
            {
                if (Lib.Q.IsInRange(target) && Lib.Q.IsReady())
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
                if (Lib.E.IsInRange(target))
                {
                    Lib.E.Cast(target);
                }
            }
        }
    }
}