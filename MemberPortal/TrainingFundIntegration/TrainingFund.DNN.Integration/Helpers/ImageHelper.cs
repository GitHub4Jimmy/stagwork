using System;
using System.IO;

namespace TrainingFund.DNN.Integration.Helpers
{
    public static class ImageHelper
    {
        public enum ImageType
        {
            JPEG,
            PNG,
            GIF
        }

        public static String GetImageAsBase64String(byte[] imageBytes, ImageType imageType)
        {
            String imageBase64Src = Convert.ToBase64String(imageBytes);
            String type = imageType.ToString().ToLower();

            return $"data:image/{type};base64,{imageBase64Src}";
        }

        public static byte[] GetImageAsByteArray(String path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
