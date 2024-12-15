namespace BaseDuet.Scripts.Data.BlueprintData
{
    using UnityEngine;

    public class StaticSFXBlueprint : ScriptableObject
    {
        public string BtnBack        { get; set; }
        public string MusicSplash    { get; set; }
        public string OptionShop     { get; set; }
        public string PlaySong       { get; set; }
        public string PurchaseShop   { get; set; }
        public string ScrollShopItem { get; set; }
        public string BtnSelect      { get; set; }
        public string BtnSelectShop  { get; set; }
        public string BtnSelectFood  { get; set; }
        public string SlideShop      { get; set; }
        public string SlideBackShop  { get; set; }
        public string SlideStuckShop { get; set; }
        public string Food { get; set; }
        public string FoodBig { get; set; }
        public string FoodLong { get; set; }
        public string FoodStar { get; set; }
        public string Obstacle { get; set; }

        public static StaticSFXBlueprint Instance;
        public StaticSFXBlueprint() { Instance = this; }
    }
}