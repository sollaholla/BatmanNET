namespace BatmanNET.Utilities
{
    public static class Mathf
    {
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Mathf.Clamp01(t);
        }

        public static float Clamp01(float value)
        {
            float result;
            if (value < 0f)
            {
                result = 0f;
            }
            else if (value > 1f)
            {
                result = 1f;
            }
            else
            {
                result = value;
            }
            return result;
        }
    }
}
