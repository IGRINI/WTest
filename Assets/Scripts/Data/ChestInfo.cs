using System;

namespace Data
{
    [Serializable]
    public struct ChestInfo
    {
        public string chest;
        public ChestItemInfo[] chest_items;
        
        [Serializable]
        public struct ChestItemInfo
        {
            public string itemkey;
            public string type;
            public string slottype;
            public string rarity;
            public int level;
            public int level_wmin;
            public int value;
        }
    }
}