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
            var target = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(x => x.MaxHealth).FirstOrDefault(x => x.IsValidTarget(Lib.E.Range + 300));
            if (target == null) return;

            if (Menu.UseQ && Lib.Q.IsReady())
            {
                if (target.IsValidTarget(Lib.Q.Range))
                {
                    Lib.Q.Cast(target);
                }
            }
            if (Menu.UseW && Lib.W.IsReady())
            {
                if (Menu.Whealth)
                    if (myHero.HealthPercent >= 40)
                        return;

                Lib.W.Cast();
            }
            if (Menu.UseE && Lib.E.IsReady())
            {
                if (target.IsValidTarget(Lib.E.Range))
                {
                    Lib.E.Cast(target);
                }
            }
        }
    }
}