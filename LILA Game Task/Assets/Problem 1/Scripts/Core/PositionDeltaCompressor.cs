using UnityEngine;

public static class PositionDeltaCompressor
{
    private const float Precision = 100f;   // two decimal places
    private const int MaxAbsValue = 100000; // safety clamp to avoid overflow

    // Compress to ints (32-bit) to avoid short overflow
    public static (int x, int y, int z, int dataSizeBits) Compress(Vector3 pos)
    {
        // clamp to reasonable range to avoid negative wrap / overflow
        float cx = Mathf.Clamp(pos.x, -MaxAbsValue, MaxAbsValue);
        float cy = Mathf.Clamp(pos.y, -MaxAbsValue, MaxAbsValue);
        float cz = Mathf.Clamp(pos.z, -MaxAbsValue, MaxAbsValue);

        int xi = Mathf.RoundToInt(cx * Precision);
        int yi = Mathf.RoundToInt(cy * Precision);
        int zi = Mathf.RoundToInt(cz * Precision);

        // dataSize: 3 * 32 bits = 96 bits (still safe)
        return (xi, yi, zi, 3 * 32);
    }

    public static Vector3 Decompress(int x, int y, int z)
    {
        return new Vector3(x / Precision, y / Precision, z / Precision);
    }
}
