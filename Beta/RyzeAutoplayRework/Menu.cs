using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Autoplay
{
    class RyzeMenu
    {
        public static Menu Menu, Laneclear, Agressive, SummonerSpells;
        public void Init()
        {
            Menu = MainMenu.AddMenu("RyzeFollow", "ryzefollow");
            Menu.Add("sliderdist", new Slider("Distance to ally", 70, 50, 300));
            Menu.Add("recall", new CheckBox("Recall if ally is recalling"));
            Agressive = Menu.AddSubMenu("Agressive mode", "agm");
            Agressive.Add("kill", new CheckBox("Orbwalk to target if enemy is killable"));
            Laneclear = Menu.AddSubMenu("Laneclear", "laneclear");
            Laneclear.Add("QLaneclear", new CheckBox("Use Q in laneclear"));
            Laneclear.Add("QSlider", new Slider("Use Q in laneclear only if mana > than", 40, 0, 100));
            SummonerSpells = Menu.AddSubMenu("Summoner spells", "summs");
            SummonerSpells.Add("heal", new Slider("Use heal at % health", 40, 0, 100));
            SummonerSpells.Add("mana", new Slider("Use clarity at % mana", 40, 0, 100));
        }
    }
}