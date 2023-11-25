using Sumsum.Internal.Hooking;

namespace Sumsum.Internal.Growtopia.App;

public class BaseApp
{
    /* BaseApp::SetFPSLimit(float) */
    public static SetFpsLimitDelegate? SetFpsLimit;
}