using System;
using System.Collections.Generic;
using System.Drawing;
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
            int RGBComponentThresholdValue = 70;
            int MaxBlotArea = 45;

            HandleCaptcha(PathToCaptcha, OutputPath, RGBComponentThresholdValue, MaxBlotArea);
        }
        static void HandleCaptcha(string Original, string Output, int Threshold, int MaxBlot)
        {
            // Loading image using path
            SKBitmap bitmap = SKBitmap.Decode(Original);
            // Converting to red/white format
            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                    if (bitmap.GetPixel(i, j).Red > Threshold)
                        bitmap.SetPixel(i, j, SKColors.White);
                    else
                        bitmap.SetPixel(i, j, SKColors.Black);
            // Cleaning
            List<Point> points = new List<Point>(); // Current blot points
            List<Point> digits = new List<Point>(); // All digits points
            int oldReviewPointsCount;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (bitmap.GetPixel(i, j) == SKColors.Black && !digits.Contains(new Point(i, j)))
                    {
                        points.Clear();
                        points.Add(new Point(i, j));
                        ExploreBlot();
                        if (points.Count <= MaxBlot)
                            ClearBlot();
                        else
                            digits.AddRange(points);
                    }
                }
            }
            // Saving
            SKImage image = SKImage.FromBitmap(bitmap);
            SKData data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
            FileStream stream = new FileStream(Output, FileMode.Create, FileAccess.Write);
            data.SaveTo(stream);
            stream.Close();
            // Resources release
            stream.Dispose();
            data.Dispose();
            image.Dispose();
            bitmap.Dispose();
            // The end
            Console.Write("The work has been completed. Press any key to exit..");
            Console.ReadKey(true);

            void ExploreBlot()
            {
                oldReviewPointsCount = points.Count;
                List<Point> staticPoints = new List<Point>(points);
                for (int i = 0; i < staticPoints.Count; i++)
                {
                    Point a = new Point(staticPoints[i].X, staticPoints[i].Y);
                    if (bitmap.GetPixel(a.X - 1, a.Y - 1) == SKColors.Black && !points.Contains(new Point(a.X - 1, a.Y - 1)))
                        points.Add(new Point(a.X - 1, a.Y - 1));
                    if (bitmap.GetPixel(a.X, a.Y - 1) == SKColors.Black && !points.Contains(new Point(a.X, a.Y - 1)))
                        points.Add(new Point(a.X, a.Y - 1));
                    if (bitmap.GetPixel(a.X + 1, a.Y - 1) == SKColors.Black && !points.Contains(new Point(a.X + 1, a.Y - 1)))
                        points.Add(new Point(a.X + 1, a.Y - 1));
                    if (bitmap.GetPixel(a.X - 1, a.Y) == SKColors.Black && !points.Contains(new Point(a.X - 1, a.Y)))
                        points.Add(new Point(a.X - 1, a.Y));
                    if (bitmap.GetPixel(a.X + 1, a.Y) == SKColors.Black && !points.Contains(new Point(a.X + 1, a.Y)))
                        points.Add(new Point(a.X + 1, a.Y));
                    if (bitmap.GetPixel(a.X - 1, a.Y + 1) == SKColors.Black && !points.Contains(new Point(a.X - 1, a.Y + 1)))
                        points.Add(new Point(a.X - 1, a.Y + 1));
                    if (bitmap.GetPixel(a.X, a.Y + 1) == SKColors.Black && !points.Contains(new Point(a.X, a.Y + 1)))
                        points.Add(new Point(a.X, a.Y + 1));
                    if (bitmap.GetPixel(a.X + 1, a.Y + 1) == SKColors.Black && !points.Contains(new Point(a.X + 1, a.Y + 1)))
                        points.Add(new Point(a.X + 1, a.Y + 1));
                }
                if (points.Count != oldReviewPointsCount)
                {
                    ExploreBlot();
                }
            }
            void ClearBlot()
            {
                for (int i = 0; i < points.Count; i++)
                    bitmap.SetPixel(points[i].X, points[i].Y, SKColors.White);
            }
        }
    }
}