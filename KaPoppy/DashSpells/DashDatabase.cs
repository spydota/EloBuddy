using EloBuddy;
using EloBuddy.SDK;
using KaPoppy;
using System.Collections.Generic;

namespace DashSpells
{
    class DashSpells
    {
        public static List<SpellData> Dashes = new List<SpellData>();
        static DashSpells()
        {
            #region Aatrox
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Aatrox,
                champname = "Aatrox",
                spellKey = SpellSlot.Q,
                enabled = true,
            });
            #endregion
            #region Ahri
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Ahri,
                champname = "Ahri",
                spellKey = SpellSlot.R,
                enabled = true,
            });
            #endregion
            #region Akali
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Akali,
                champname = "Akali",
                spellKey = SpellSlot.R,
                enabled = true,
            });
            #endregion
            #region Corki
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Corki,
                champname = "Corki",
                spellKey = SpellSlot.W,
                enabled = true,
            });
            #endregion
            #region Diana
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Diana,
                champname = "Diana",
                spellKey = SpellSlot.R,
                enabled = true,
            });
            #endregion
            #region Ekko
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Ekko,
                champname = "Ekko",
                spellKey = SpellSlot.E,
            });
            #endregion
            #region Fiora
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Fiora,
                champname = "Fiora",
                spellKey = SpellSlot.Q,
                enabled = true,
            });
            #endregion
            #region Fizz
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Fizz,
                champname = "Fizz",
                spellKey = SpellSlot.Q,
                enabled = true,
            });
            #endregion
            #region Gragas
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Gragas,
                champname = "Gragas",
                spellKey = SpellSlot.E,
                enabled = true,
            });
            #endregion
            #region Graves
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Graves,
                champname = "Graves",
                spellKey = SpellSlot.E,
                enabled = true,
            });
            #endregion
            #region Irelia
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Irelia,
                champname = "Irelia",
                spellKey = SpellSlot.Q,
                enabled = true,
            });
            #endregion
            #region JarvanIV
            Dashes.Add(
            new SpellData
            {
                champ = Champion.JarvanIV,
                champname = "JarvanIV",
                spellKey = SpellSlot.Q,
                enabled = true,
            });
            #endregion
            #region Jax
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Jax,
                champname = "Jax",
                spellKey = SpellSlot.Q,
                enabled = true,
            });
            #endregion
            #region Kha'Zix
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Khazix,
                champname = "Kha'Zix",
                spellKey = SpellSlot.E,
                enabled = true,
            });
            #endregion
            #region Kindred
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Kindred,
                champname = "Kindred",
                spellKey = SpellSlot.Q,
                enabled = true,
            });
            #endregion
            #region Leblanc
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Leblanc,
                champname = "Leblanc",
                spellKey = SpellSlot.W,
                enabled = true,
                spellname = "LeblancSlide"
            });
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Leblanc,
                champname = "Leblanc",
                spellKey = SpellSlot.R,
                enabled = true,
                spellname = "LeblancSlideM"
            });
            #endregion
            #region Lee Sin
            Dashes.Add(
            new SpellData
            {
                champ = Champion.LeeSin,
                champname = "LeeSin",
                spellKey = SpellSlot.W,
                enabled = true,
                spellname = "BlindMonkWOne"
            });
            Dashes.Add(
            new SpellData
            {
                champ = Champion.LeeSin,
                champname = "LeeSin",
                spellKey = SpellSlot.Q,
                enabled = true,
                spellname = "blindmonkqtwo"
            });
            #endregion
            #region Leona
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Leona,
                champname = "Leona",
                spellKey = SpellSlot.E,
                enabled = true,
            });
            #endregion
            #region Lucian
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Lucian,
                champname = "Lucian",
                spellKey = SpellSlot.E,
                enabled = true,
            });
            #endregion
            #region Pantheon
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Pantheon,
                champname = "Pantheon",
                spellKey = SpellSlot.W,
                enabled = true,
            });
            #endregion
            #region Poppy
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Poppy,
                champname = "Poppy",
                spellKey = SpellSlot.E,
                enabled = true,
            });
            #endregion
            #region Quinn
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Quinn,
                champname = "Quinn",
                spellKey = SpellSlot.E,
            });
            #endregion
            //Change Name
            #region Rek'Sai 
            Dashes.Add(
            new SpellData
            {
                champ = Champion.RekSai,
                champname = "Rek'Sai",
                spellKey = SpellSlot.E,
                enabled = true,
                spellname = ""
            });
            #endregion
            #region Riven
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Riven,
                champname = "Riven",
                spellKey = SpellSlot.Q,
                enabled = false,
            });
            #endregion
            #region Shen
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Shen,
                champname = "Shen",
                spellKey = SpellSlot.E,
                enabled = true,
            });
            #endregion
            #region Shyvana
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Shyvana,
                champname = "Shyvana",
                spellKey = SpellSlot.R,
                enabled = true,
            });
            #endregion
            #region Tristana
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Tristana,
                champname = "Tristana",
                spellKey = SpellSlot.W,
                enabled = true,
            });
            #endregion
            //Change Name
            #region Vi
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Vi,
                champname = "Vi",
                spellKey = SpellSlot.Q,
                enabled = true,
                spellname = ""
            });
            #endregion   
            #region Yasuo
            Dashes.Add(
            new SpellData
            {
                champ = Champion.Yasuo,
                champname = "Yasuo",
                spellKey = SpellSlot.E,
            });
            #endregion

        }
    }
}

