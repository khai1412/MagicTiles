namespace MagicTiles.Scripts.Themes
{
    using UnityEngine;

    public class ThemeController : MonoBehaviour
    {
        [SerializeField] private BackgroundThemeController  backgroundThemeController;
        [SerializeField] private CharacterThemeController[] characterThemeControllers;
        [SerializeField] private NoteThemeController[]      noteThemeControllers;
    }
}