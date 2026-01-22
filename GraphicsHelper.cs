/*
 * ============================================================================
 * Клас: GraphicsHelper - Помощни функции за графика
 * ============================================================================
 * Описание: Статичен клас, предоставящ помощни методи за работа с графика.
 *           Включва функции за създаване на градиенти, рисуване на фигури
 *           и обработка на изображения.
 * 
 * Студент: Димитър Клянев
 * Факултетен номер: F112194
 * 
 * Курс: CSCB579 Програмиране на приложения с Microsoft Visual C# .NET
 * ============================================================================
 */

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace GraphicsDemo
{
    // Статичен помощен клас с методи за графични операции.
    // Предоставя функционалност за създаване на градиенти, стилизиран текст
    // и различни визуални ефекти.
    //
    // Този клас демонстрира:
    // - Работа с LinearGradientBrush за градиенти
    // - Използване на GraphicsPath за сложни форми
    // - Създаване на сенки и контури за текст
    // - Манипулация на изображения
    // 
    // Студент: Dimitar Klianev
    // Факултетен номер: F112194
    public static class GraphicsHelper
    {
        #region Цветови палитри (Color Palettes)

        // Предефинирана палитра с пастелни цветове.
        // Използва се за създаване на приятни за окото цветови комбинации.
        public static readonly Color[] PastelPalette = new Color[]
        {
            Color.FromArgb(255, 179, 186),   // Розово
            Color.FromArgb(255, 223, 186),   // Праскова
            Color.FromArgb(255, 255, 186),   // Светло жълто
            Color.FromArgb(186, 255, 201),   // Мента
            Color.FromArgb(186, 225, 255),   // Светло синьо
            Color.FromArgb(219, 186, 255)    // Лавандула
        };

        // Предефинирана палитра с ярки, наситени цветове.
        // Подходяща за акценти и важни елементи.
        public static readonly Color[] VibrantPalette = new Color[]
        {
            Color.FromArgb(255, 87, 51),     // Ярко оранжево
            Color.FromArgb(255, 189, 51),    // Златисто
            Color.FromArgb(51, 255, 87),     // Неоново зелено
            Color.FromArgb(51, 181, 255),    // Ярко синьо
            Color.FromArgb(189, 51, 255),    // Виолетово
            Color.FromArgb(255, 51, 161)     // Магента
        };

        #endregion

        #region Методи за градиенти (Gradient Methods)

        // Създава линеен градиент между два цвята.
        //
        // rect: Правоъгълник, в който да се приложи градиентът
        // startColor: Начален цвят
        // endColor: Краен цвят
        // angle: Ъгъл на градиента в градуси
        //
        // Градиентът започва от startColor и постепенно преминава към endColor.
        // Ъгълът определя посоката: 0° = хоризонтално, 90° = вертикално.
        // 
        // ВАЖНО: Извикващият код трябва да освободи ресурса с Dispose()!
        public static LinearGradientBrush CreateLinearGradient(Rectangle rect,
            Color startColor, Color endColor, float angle = 45f)
        {
            return new LinearGradientBrush(rect, startColor, endColor, angle);
        }

        // Създава многоцветен градиент (дъга).
        //
        // rect: Правоъгълник за градиента
        // colors: Масив от цветове
        //
        // Този метод позволява създаване на сложни градиенти с повече от два цвята.
        // Цветовете се разпределят равномерно по дължината на градиента.
        public static LinearGradientBrush CreateRainbowGradient(Rectangle rect, Color[] colors = null)
        {
            // Ако не са подадени цветове, използваме дъгата
            if (colors == null)
            {
                colors = new Color[]
                {
                    Color.Red, Color.Orange, Color.Yellow,
                    Color.Green, Color.Blue, Color.Purple
                };
            }

            LinearGradientBrush brush = new LinearGradientBrush(
                rect, colors[0], colors[colors.Length - 1], 0f);

            // Създаване на ColorBlend за множество цветове
            ColorBlend blend = new ColorBlend(colors.Length);
            blend.Colors = colors;

            // Разпределяне на позициите равномерно
            float[] positions = new float[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                positions[i] = (float)i / (colors.Length - 1);
            }
            blend.Positions = positions;

            brush.InterpolationColors = blend;

            return brush;
        }

        #endregion

        #region Методи за текст (Text Methods)

        // Рисува текст със сянка за по-добра четимост.
        //
        // g: Graphics обект
        // text: Текстът за рисуване
        // font: Шрифт за текста
        // textColor: Цвят на текста
        // shadowColor: Цвят на сянката
        // location: Позиция на текста
        // shadowOffset: Отместване на сянката (по подразбиране 2 пиксела)
        //
        // Сянката се рисува първа, след което текстът се рисува отгоре.
        // Това създава илюзия за дълбочина и подобрява четимостта.
        public static void DrawTextWithShadow(Graphics g, string text, Font font,
            Color textColor, Color shadowColor, PointF location, int shadowOffset = 2)
        {
            // Настройка за качествено рендиране на текст
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Рисуване на сянката (отместена надолу и надясно)
            using (SolidBrush shadowBrush = new SolidBrush(shadowColor))
            {
                g.DrawString(text, font, shadowBrush,
                    location.X + shadowOffset, location.Y + shadowOffset);
            }

            // Рисуване на основния текст
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                g.DrawString(text, font, textBrush, location);
            }
        }

        // Рисува текст с контур (outline).
        //
        // g: Graphics обект
        // text: Текстът за рисуване
        // font: Шрифт за текста
        // fillColor: Цвят за запълване
        // outlineColor: Цвят на контура
        // location: Позиция на текста
        // outlineWidth: Дебелина на контура
        //
        // Използва GraphicsPath за създаване на текстов път,
        // който след това се запълва и очертава.
        public static void DrawOutlinedText(Graphics g, string text, Font font,
            Color fillColor, Color outlineColor, PointF location, float outlineWidth = 2f)
        {
            // Настройка за висококачествено рендиране
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Създаване на GraphicsPath за текста
            using (GraphicsPath path = new GraphicsPath())
            {
                // Добавяне на текста към пътя
                path.AddString(text, font.FontFamily, (int)font.Style, font.Size,
                    location, StringFormat.GenericDefault);

                // Рисуване на контура
                using (Pen outlinePen = new Pen(outlineColor, outlineWidth))
                {
                    outlinePen.LineJoin = LineJoin.Round;
                    g.DrawPath(outlinePen, path);
                }

                // Запълване на текста
                using (SolidBrush fillBrush = new SolidBrush(fillColor))
                {
                    g.FillPath(fillBrush, path);
                }
            }
        }

        // Рисува текст с градиентно запълване.
        //
        // g: Graphics обект
        // text: Текстът за рисуване
        // font: Шрифт за текста
        // startColor: Начален цвят на градиента
        // endColor: Краен цвят на градиента
        // location: Позиция на текста
        //
        // Създава атрактивен ефект на преливащи се цветове в текста.
        public static void DrawGradientText(Graphics g, string text, Font font,
            Color startColor, Color endColor, PointF location)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Създаване на GraphicsPath за текста
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddString(text, font.FontFamily, (int)font.Style, font.Size,
                    location, StringFormat.GenericDefault);

                // Получаване на границите на текста за градиента
                RectangleF bounds = path.GetBounds();

                // Създаване на градиентна четка
                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    bounds, startColor, endColor, LinearGradientMode.Vertical))
                {
                    g.FillPath(gradientBrush, path);
                }
            }
        }

        #endregion

        #region Методи за форми (Shape Methods)

        // Рисува заоблен правоъгълник.
        //
        // g: Graphics обект
        // rect: Правоъгълникът за рисуване
        // radius: Радиус на заобляне
        // fillColor: Цвят за запълване
        // borderColor: Цвят на рамката (null за без рамка)
        // borderWidth: Дебелина на рамката
        //
        // Заоблените правоъгълници се използват често в модерни UI дизайни.
        // Методът създава GraphicsPath с дъги в ъглите.
        public static void DrawRoundedRectangle(Graphics g, Rectangle rect, int radius,
            Color fillColor, Color? borderColor = null, float borderWidth = 1f)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = CreateRoundedRectanglePath(rect, radius))
            {
                // Запълване
                using (SolidBrush brush = new SolidBrush(fillColor))
                {
                    g.FillPath(brush, path);
                }

                // Рамка (ако е зададена)
                if (borderColor.HasValue)
                {
                    using (Pen pen = new Pen(borderColor.Value, borderWidth))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        // Създава GraphicsPath за заоблен правоъгълник.
        //
        // rect: Правоъгълникът
        // radius: Радиус на заобляне
        //
        // Помощен метод, използван от DrawRoundedRectangle.
        // ВАЖНО: Извикващият трябва да освободи ресурса с Dispose()!
        private static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Горен ляв ъгъл
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // Горна линия
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);
            // Горен десен ъгъл
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            // Дясна линия
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);
            // Долен десен ъгъл
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            // Долна линия
            path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);
            // Долен ляв ъгъл
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            // Лява линия
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);

            path.CloseFigure();

            return path;
        }

        #endregion

        #region Методи за изображения (Image Methods)

        // Прилага ефект на избледняване към изображение.
        //
        // original: Оригиналното изображение
        // opacity: Прозрачност (0.0 - 1.0)
        //
        // Използва ColorMatrix за промяна на алфа канала на изображението.
        // Полезно за fade-in/fade-out ефекти.
        public static Bitmap ApplyOpacity(Image original, float opacity)
        {
            Bitmap result = new Bitmap(original.Width, original.Height);

            using (Graphics g = Graphics.FromImage(result))
            {
                // Създаване на матрица за цветова трансформация
                ColorMatrix colorMatrix = new ColorMatrix();
                colorMatrix.Matrix33 = opacity;  // Алфа канал

                // Прилагане на атрибутите
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);

                    g.DrawImage(original,
                        new Rectangle(0, 0, result.Width, result.Height),
                        0, 0, original.Width, original.Height,
                        GraphicsUnit.Pixel, attributes);
                }
            }

            return result;
        }

        // Преоразмерява изображение, запазвайки пропорциите.
        //
        // original: Оригиналното изображение
        // maxWidth: Максимална ширина
        // maxHeight: Максимална височина
        //
        // Изчислява новите размери така, че изображението да се побере
        // в зададените граници, без да се деформира.
        public static Bitmap ResizeImage(Image original, int maxWidth, int maxHeight)
        {
            // Изчисляване на съотношението
            double ratioX = (double)maxWidth / original.Width;
            double ratioY = (double)maxHeight / original.Height;
            double ratio = Math.Min(ratioX, ratioY);

            // Нови размери
            int newWidth = (int)(original.Width * ratio);
            int newHeight = (int)(original.Height * ratio);

            Bitmap result = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.DrawImage(original, 0, 0, newWidth, newHeight);
            }

            return result;
        }

        #endregion
    }
}
