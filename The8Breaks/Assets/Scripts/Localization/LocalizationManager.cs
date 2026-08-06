using UnityEngine;
using System.Collections.Generic;
using DFTGames.Localization;

namespace MBW.WaST
{
    public class LocalizationManager : MonoBehaviour
    {
        private List<SystemLanguage> _languages = new List<SystemLanguage> { SystemLanguage.English, SystemLanguage.Russian, SystemLanguage.Polish, SystemLanguage.German, SystemLanguage.Unknown };
        private int _languageIndex;

        private void Start() { _languageIndex = PlayerPrefs.GetInt("LangIndex", 0); }
        public void ChangeIndex(int newIndex)
        {
            _languageIndex = newIndex;
        }
        public void ChangeLanguage() { Localize.SetCurrentLanguage(_languages[_languageIndex]); }
        public void CompletelyChangeLanguage()
        {
            PlayerPrefs.SetInt("LangIndex", _languageIndex);
            ChangeLanguage();
        }
    }
}