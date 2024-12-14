namespace BaseDuet.Scripts.Helpers
{
    using BaseDuet.Scripts.Base;
    using Spine.Unity;

    public class BaseDuetCharacterViewHelper
    {
        public virtual void BindTopDownCharacterSkin(BaseDuetItemView  characterView,   int    index) { }
        public virtual void BindTopDownSkeletonGraphic(SkeletonGraphic skeletonGraphic, string skin) { }
        public virtual void BindCharacterSkin(BaseDuetItemView  characterView,   string image) { }
        public  virtual  void BindSkeletonGraphic(SkeletonGraphic skeletonGraphic, string skeletonAsset, string skinName){}
        public virtual void BindNoteSkin(BaseDuetItemView       view)               { }
        public virtual void BindNoteSkin(BaseDuetItemView       view, string image) { }
    }
}