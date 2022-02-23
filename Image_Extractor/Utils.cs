namespace Image_Extractor
{
    public static class Utils
    {
        public static int FindRangeInArray(ref byte[] arr, ref byte[] template, int offset, bool for_end = false)
        {
            bool found = false;
            int res = -1;
            if (arr.Length >= template.Length)
            {
                for (int i = offset; i <= arr.Length - template.Length; i++)
                {
                    if (arr[i] == template[0])
                    {
                        for (int j = 1; j < template.Length; j++)
                        {
                            if (arr[i + j] != template[j])
                                break;
                            if (j == template.Length - 1)
                            {
                                if (!for_end)
                                    res = i;
                                else
                                    res = i + j + 1;
                                found = true;
                                break;
                            }
                        }
                    }
                    if (found)
                        break;
                }
            }
            return res;
        }
    }
}
