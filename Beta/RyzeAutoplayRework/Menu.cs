using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Autoplay
{
    class RyzeMenu
    {
        public static Menu Menu;
        public void Init()
        {
            Menu = MainMenu.AddMenu("RyzeAutoPlay Reworked", "whatever");
            Menu.AddGroupLabel("Loaded");
        }
    }
}