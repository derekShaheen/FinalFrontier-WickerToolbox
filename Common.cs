using Il2Cpp;
namespace WickerToolbox
{
    internal static class Common
    {
        public static bool IsInGame()
        {
            if (GameManager.Instance == null)
            {

                return false;
            }

            if (GameManager.Instance != null && GameManager.gameFullyInitialized)
            {
                return true;
            }

            return false;
        }
    }
}
