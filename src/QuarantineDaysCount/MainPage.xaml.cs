using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QuarantineDaysCount
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public string CuantosDias { get; private set; }
        private DateTime quarantineStaredOn = new DateTime(2020, 03, 15);
        SKPaint blackFillPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.DarkRed,
            StrokeWidth = 7
        };

        SKPaint textFill = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.DarkRed,
            TextSize = 70
        };
        public MainPage()
        {
            InitializeComponent();
            SetText();
        }

        private string GetElapsedDays()
        {
            return CalculateDays().ToString();
        }

        private int CalculateDays()
        {
            var diff = DateTime.Today.Subtract(quarantineStaredOn);
            return diff.Days;
        }

        private void SetText()
        {
            CuantosDias = GetElapsedDays();
            OnPropertyChanged(nameof(CuantosDias));
        }

        private void PaintArea_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            int x = 100;
            int padding = 25;
            int y = 100;
            int height = 60;

            canvas.Clear(SKColors.LightGray);
            int cuantosDias = CalculateDays();

            int groups = cuantosDias / 5;
            int resto = cuantosDias % 5;
            int lines = groups / 6;
            int restLines = groups % 6;

            int startingwidth = x;
            for (int l = 0; l < lines; l++)
            {
                y += y * l + 30 + height;
                DrawFullGroup(canvas, startingwidth, padding, y, height);
                for (int i = 1; i < 6; i++)
                {
                    startingwidth += padding * 6;
                    DrawFullGroup(canvas, startingwidth, padding, y, height);
                }

            }

            if (restLines > 0)
            {
                y += y * lines + 30 + height;
                int restGroups = groups - 6;
                startingwidth = x;
                DrawFullGroup(canvas, startingwidth, padding, y, height);
                for (int i = 1; i < restGroups; i++)
                {
                    startingwidth += padding * 6;
                    DrawFullGroup(canvas, startingwidth, padding, y, height);
                }

                if (resto > 0)
                    DrawRestoGroup(canvas, startingwidth + padding * 6, padding, y, height, resto);
            }

            y += y * lines + 30 + height;
            canvas.DrawText(cuantosDias.ToString(), x, y, textFill);

        }

        private void DrawFullGroup(SKCanvas canvas, int x, int padding, int y, int height)
        {
            DrawVerticalLine(x, y, height, canvas);
            DrawVerticalLine(x + padding, y, height, canvas);
            DrawVerticalLine(x + padding * 2, y, height, canvas);
            DrawVerticalLine(x + padding * 3, y, height, canvas);
            DrawCrossLine(x, y, height, padding, canvas);
        }

        private void DrawRestoGroup(SKCanvas canvas, int x, int padding, int y, int height, int resto)
        {
            DrawVerticalLine(x, y, height, canvas);

            for (int i = 1; i < resto; i++)
            {
                DrawVerticalLine(x + padding * i, y, height, canvas);
            }
        }

        private void DrawVerticalLine(int x, int y, int height, SKCanvas canvas)
        {
            canvas.DrawLine(x, y, x, y+height, blackFillPaint);
        }

        private void DrawCrossLine(int x, int y, int height, int padding, SKCanvas canvas)
        {
            int yBottomPosition = y + height -15;
            int xTopPosition = x + (padding * 4) - 7;
            int xleftPosition = x - 15; 
            canvas.DrawLine(xleftPosition, yBottomPosition, xTopPosition, y+15, blackFillPaint);
        }
    }
}
