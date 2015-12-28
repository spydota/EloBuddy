using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;
using System.Linq;

namespace Modes
{
    internal class Stun : Helper
    {
        internal static void Execute()
        {
            var target = TargetSelector.SelectedTarget;
            if (target == null)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                return;
            }


            if (!Lib.E.IsReady())
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                return;
            }
            var pos = Lib.PointsAroundTheTarget(target, 500).Where(x => Lib.CanStun(target, x.To2D()) && !IsWall(x)).OrderBy(x => x.Distance(myHero));
            if (pos.Count() == 0)
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            else
                Player.IssueOrder(GameObjectOrder.MoveTo, pos.First());

            if (Lib.CanStun(target))
            {
                Lib.E.Cast(target);
            }
            else if (Settings.ComboSettings.UseFlashE && pos.Count() > 0)
            {
                if (Lib.Flash.IsReady() && Lib.Flash.IsInRange(pos.First()) && myHero.Distance(pos.First()) > 100)
                {
                    Lib.Flash.Cast(pos.First());
                }
            }
        }
    }
}