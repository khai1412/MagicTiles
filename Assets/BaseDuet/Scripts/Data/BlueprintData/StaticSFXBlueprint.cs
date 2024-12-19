using System;

namespace BaseDuet.Scripts.Data.BlueprintData
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "StaticSFXBlueprint", menuName = "ScriptableObjects/StaticSFXBlueprint")]
    public class StaticSFXBlueprint : ScriptableObject
    {
        public string BtnBack;
        public string MusicSplash;
        public string OptionShop;
        public string PlaySong;
        public string PurchaseShop;
        public string ScrollShopItem;
        public string BtnSelect;
        public string BtnSelectShop;
        public string BtnSelectFood;
        public string SlideShop;
        public string SlideBackShop;
        public string SlideStuckShop;
        public string Food;
        public string FoodBig;
        public string FoodLong;
        public string FoodStar;
        public string Obstacle;
    }
}