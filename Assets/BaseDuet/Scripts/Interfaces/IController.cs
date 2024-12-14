namespace BaseDuet.Scripts.Interfaces
{
    public interface IController<TModel, TView> where TModel : class where TView : class
    {
        TModel Model { get; }
        TView View { get; }
        
        void BindData(TModel model, TView view);
    }
}