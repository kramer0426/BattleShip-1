using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    //-------------------------------------------------------------------------------
    // SoundInfo
    //-------------------------------------------------------------------------------
    public class SoundInfo
    {
        public int id;
        public int type;
        public string resourceName;

        public SoundInfo()
        {
            id = 0;
            type = 0;
        }
    }


    //-------------------------------------------------------------------------------
    // SoundMgr
    //-------------------------------------------------------------------------------
    public class SoundMgr
    {

        private static SoundMgr instance = null;

        public static SoundMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundMgr();
                }
                return instance;
            }
        }

        private SoundMgr()
        {

        }

        //
        public Dictionary<int, SoundInfo> g_soundTable = new Dictionary<int, SoundInfo>();

        //
        public SoundInfo GetSoundInfo(int soundId)
        {
            if (g_soundTable.ContainsKey(soundId))
            {
                return g_soundTable[soundId];
            }

            return null;
        }
    }
}