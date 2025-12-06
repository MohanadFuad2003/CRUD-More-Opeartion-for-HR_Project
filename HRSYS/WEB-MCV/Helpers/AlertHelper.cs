using Microsoft.AspNetCore.Mvc;

namespace WEB_MCV.Helpers
{
    public static class AlertHelper
    {
        public static void Success(Controller controller, string message)
        {
            controller.TempData["Success"] = message;
        }

        public static void Error(Controller controller, string message)
        {
            controller.TempData["Error"] = message;
        }

        public static void Warning(Controller controller, string message)
        {
            controller.TempData["Warning"] = message;
        }
    }
}
