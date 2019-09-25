using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutTimer.Classes
{
    // Stores all the user prefs in one class
    public class Preferences
    {
        public int PauseMinutes { get; set; }
        public int PauseSeconds { get; set; }
        public int SetMinutes { get; set; }
        public int SetSeconds { get; set; }
        public int SetCounter { get; set; }

        private IDictionary<string, object> prefDict;

        public Preferences(IDictionary<string, object> dict)
        {
            prefDict = dict;
            Load();
        }

        // load prefs from local application storage
        public void Load()
        {
            if (prefDict.ContainsKey("setCount"))
            {
                PauseMinutes = Convert.ToInt16(prefDict["pauseMinutes"].ToString());
                PauseSeconds = Convert.ToInt16(prefDict["pauseSeconds"].ToString());
                SetMinutes = Convert.ToInt16(prefDict["setMinutes"].ToString());
                SetSeconds = Convert.ToInt16(prefDict["setSeconds"].ToString());
                SetCounter = Convert.ToInt16(prefDict["setCount"].ToString());
            }
            else
            {
                prefDict["pauseMinutes"] = "00";
                prefDict["pauseSeconds"] = "00";
                prefDict["setMinutes"] = "00";
                prefDict["setSeconds"] = "00";
                prefDict["setCount"] = "00";
            }
        }

        public bool Save(string[] prefs)
        {
            PauseMinutes = Convert.ToInt16(prefs[0]);
            prefDict["pauseMinutes"] = prefs[0];
            PauseSeconds = Convert.ToInt16(prefs[1]);
            prefDict["pauseSeconds"] = prefs[1];
            SetMinutes = Convert.ToInt16(prefs[2]);
            prefDict["setMinutes"] = prefs[2];
            SetSeconds = Convert.ToInt16(prefs[3]);
            prefDict["setSeconds"] = prefs[3];
            SetCounter = Convert.ToInt16(prefs[4]);
            prefDict["setCount"] = prefs[4];
            return true;
        }
    }
}
