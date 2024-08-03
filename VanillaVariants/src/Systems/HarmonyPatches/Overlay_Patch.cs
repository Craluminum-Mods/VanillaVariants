using Vintagestory.API.MathTools;

namespace VanillaVariants;

public static class Overlay_Patch
{
    public static bool Prefix(ref int __result, int rgb1, int rgb2)
    {
        VSColor lhs = new(rgb1);
        VSColor rhs = new(rgb2);
        rhs.Rn *= rhs.An;
        rhs.Gn *= rhs.An;
        rhs.Bn *= rhs.An;
        int lhsA = lhs.A;

        if (lhsA < 128)
        {
            __result = 0;
            return false;
        }
        return true;
    }
}