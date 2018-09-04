using System;
using System.IO;
using SkiaSharp;

namespace transkart_captcha_noise_remover
{
    class Program
    {
        static void Main(string[] args)
        {
            string PathToCaptcha = @"C:\Users\thedc\Desktop\Капча Транспортная карта\Пример.jpg";
            string OutputPath = @"C:\Users\thedc\Desktop\Капча Транспортная карта\output.jpg";
            int RGBComponentThresholdValue = 127;
            int MaxBlotArea = 35;

            HandleCaptcha(PathToCaptcha, OutputPath, RGBComponentThresholdValue, MaxBlotArea);
        }

        static void HandleCaptcha(string Original, string Output, int Threshold, int MaxBlot)
        {
            // Loading image using path
            SKBitmap bitmap = SKBitmap.Decode(Original);
            // Converting to black/white format
            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                    if (bitmap.GetPixel(i, j).Red > Threshold)
                        bitmap.SetPixel(i, j, SKColors.White);
                    else
                        bitmap.SetPixel(i, j, SKColors.Black);
            // Cleaning

            // Saving
            SKImage image = SKImage.FromBitmap(bitmap);
            SKData data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
            FileStream stream = new FileStream(Output, FileMode.Create, FileAccess.Write);
            data.SaveTo(stream);
            stream.Close();
            // The end
            Console.Write("The work has been completed. Press any key to exit..");
            Console.ReadKey(true);
        }
    }
}
