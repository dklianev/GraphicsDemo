/*
 * ============================================================================
 * Клас: AnimatedShape - Анимирана форма
 * ============================================================================
 * Описание: Този клас представлява анимирана геометрична фигура, която се
 *           движи в определена област и отскача от границите й.
 *           Поддържа различни видове форми: кръг, квадрат, триъгълник.
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

namespace GraphicsDemo
{
    // Изброим тип, дефиниращ възможните видове форми за анимация.
    //
    // Студент: Dimitar Klianev, F112194
    public enum ShapeType
    {
        // Кръгла форма (елипса)
        Circle,
        // Квадратна форма (правоъгълник)
        Square,
        // Триъгълна форма
        Triangle,
        // Звезда
        Star
    }

    // Клас, представляващ анимирана геометрична фигура.
    // Фигурата се движи в определена област и отскача от границите.
    //
    // Този клас демонстрира:
    // - Работа с Graphics класа за рисуване
    // - Използване на SolidBrush и LinearGradientBrush
    // - Анимация чрез промяна на позиция
    // - Collision detection с границите на контейнера
    // 
    // Студент: Dimitar Klianev
    // Факултетен номер: F112194
    public class AnimatedShape
    {
        #region Полета (Private Fields)

        // Текуща X позиция на фигурата
        private float posX;

        // Текуща Y позиция на фигурата
        private float posY;

        // Скорост на движение по X ос (може да е отрицателна)
        private float velocityX;

        // Скорост на движение по Y ос (може да е отрицателна)
        private float velocityY;

        // Размер на фигурата (ширина и височина)
        private float size;

        // Основен цвят на фигурата
        private Color primaryColor;

        // Вторичен цвят за градиент
        private Color secondaryColor;

        // Тип на фигурата (кръг, квадрат, триъгълник, звезда)
        private ShapeType shapeType;

        // Генератор на случайни числа за вариации
        private static Random random = new Random();

        #endregion

        #region Свойства (Properties)

        // Получава или задава X позицията на фигурата.
        public float X
        {
            get { return posX; }
            set { posX = value; }
        }

        // Получава или задава Y позицията на фигурата.
        public float Y
        {
            get { return posY; }
            set { posY = value; }
        }

        // Получава или задава размера на фигурата.
        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        // Получава или задава типа на фигурата.
        public ShapeType Shape
        {
            get { return shapeType; }
            set { shapeType = value; }
        }

        #endregion

        #region Конструктори (Constructors)

        // Създава нова анимирана фигура с определени параметри.
        // x: Начална X позиция
        // y: Начална Y позиция
        // size: Размер на фигурата
        // velocityX: Начална скорост по X
        // velocityY: Начална скорост по Y
        // primary: Основен цвят
        // secondary: Вторичен цвят за градиент
        // shape: Тип на фигурата
        //
        // Конструкторът инициализира всички полета на фигурата.
        // Скоростите определят посоката и бързината на движение.
        public AnimatedShape(float x, float y, float size, float velocityX, float velocityY,
                            Color primary, Color secondary, ShapeType shape)
        {
            this.posX = x;
            this.posY = y;
            this.size = size;
            this.velocityX = velocityX;
            this.velocityY = velocityY;
            this.primaryColor = primary;
            this.secondaryColor = secondary;
            this.shapeType = shape;
        }

        // Създава случайна анимирана фигура в определени граници.
        // maxWidth: Максимална ширина на контейнера
        // maxHeight: Максимална височина на контейнера
        // връща: Нова случайно генерирана фигура
        //
        // Статичен фабричен метод, който генерира фигура със случайни:
        // - Позиция в границите на контейнера
        // - Размер между 20 и 60 пиксела
        // - Скорост между 1 и 4 пиксела на тик
        // - Цветове от предефинирана палитра
        // - Случаен тип форма
        public static AnimatedShape CreateRandom(int maxWidth, int maxHeight)
        {
            // Масив с атрактивни цветови комбинации за градиенти
            Color[][] colorPairs = new Color[][]
            {
                new Color[] { Color.FromArgb(255, 99, 71), Color.FromArgb(255, 165, 0) },    // Червено-оранжево
                new Color[] { Color.FromArgb(0, 191, 255), Color.FromArgb(30, 144, 255) },   // Светло синьо
                new Color[] { Color.FromArgb(50, 205, 50), Color.FromArgb(34, 139, 34) },    // Зелено
                new Color[] { Color.FromArgb(255, 20, 147), Color.FromArgb(199, 21, 133) },  // Розово
                new Color[] { Color.FromArgb(138, 43, 226), Color.FromArgb(75, 0, 130) },    // Лилаво
                new Color[] { Color.FromArgb(255, 215, 0), Color.FromArgb(255, 140, 0) }     // Златисто
            };

            // Избор на случайна цветова комбинация
            int colorIndex = random.Next(colorPairs.Length);
            Color primary = colorPairs[colorIndex][0];
            Color secondary = colorPairs[colorIndex][1];

            // Генериране на случаен размер
            float size = random.Next(25, 55);

            // Генериране на случайна позиция (в рамките на контейнера)
            float x = random.Next(10, Math.Max(11, maxWidth - (int)size - 10));
            float y = random.Next(10, Math.Max(11, maxHeight - (int)size - 10));

            // Генериране на случайна скорост (избягваме нулева скорост)
            float velX = (random.Next(1, 4)) * (random.Next(2) == 0 ? 1 : -1);
            float velY = (random.Next(1, 4)) * (random.Next(2) == 0 ? 1 : -1);

            // Избор на случаен тип форма
            ShapeType[] shapes = (ShapeType[])Enum.GetValues(typeof(ShapeType));
            ShapeType shape = shapes[random.Next(shapes.Length)];

            return new AnimatedShape(x, y, size, velX, velY, primary, secondary, shape);
        }

        #endregion

        #region Методи за движение (Movement Methods)

        // Актуализира позицията на фигурата и проверява за сблъсък с границите.
        // containerWidth: Ширина на контейнера
        // containerHeight: Височина на контейнера
        //
        // Този метод:
        // 1. Добавя скоростта към текущата позиция
        // 2. Проверява дали фигурата е достигнала граница
        // 3. Ако да - обръща посоката на движение (отскачане)
        // 
        // Отскачането се симулира чрез смяна на знака на скоростта.
        public void Move(int containerWidth, int containerHeight)
        {
            // Актуализиране на позицията
            posX += velocityX;
            posY += velocityY;

            // Проверка за сблъсък с лява или дясна граница
            if (posX <= 0 || posX >= containerWidth - size)
            {
                // Обръщане на посоката по X ос
                velocityX = -velocityX;

                // Коригиране на позицията, ако е извън границите
                if (posX < 0) posX = 0;
                if (posX > containerWidth - size) posX = containerWidth - size;
            }

            // Проверка за сблъсък с горна или долна граница
            if (posY <= 0 || posY >= containerHeight - size)
            {
                // Обръщане на посоката по Y ос
                velocityY = -velocityY;

                // Коригиране на позицията, ако е извън границите
                if (posY < 0) posY = 0;
                if (posY > containerHeight - size) posY = containerHeight - size;
            }
        }

        #endregion

        #region Методи за рисуване (Drawing Methods)

        // Рисува фигурата върху посочения Graphics обект.
        // g: Graphics обект за рисуване
        //
        // Методът използва LinearGradientBrush за създаване на градиентен ефект.
        // В зависимост от типа на фигурата се извиква съответният метод за рисуване.
        // 
        // Поддържани форми:
        // - Circle: Рисува се с FillEllipse
        // - Square: Рисува се с FillRectangle
        // - Triangle: Рисува се с FillPolygon
        // - Star: Рисува се с FillPolygon (5-точкова звезда)
        public void Draw(Graphics g)
        {
            // Създаване на правоъгълник, ограничаващ фигурата
            RectangleF bounds = new RectangleF(posX, posY, size, size);

            // Създаване на градиентна четка за по-атрактивен вид
            using (LinearGradientBrush brush = new LinearGradientBrush(
                bounds, primaryColor, secondaryColor, LinearGradientMode.ForwardDiagonal))
            {
                // Рисуване според типа на фигурата
                switch (shapeType)
                {
                    case ShapeType.Circle:
                        DrawCircle(g, brush);
                        break;
                    case ShapeType.Square:
                        DrawSquare(g, brush);
                        break;
                    case ShapeType.Triangle:
                        DrawTriangle(g, brush);
                        break;
                    case ShapeType.Star:
                        DrawStar(g, brush);
                        break;
                }
            }
        }

        // Рисува кръгла форма (елипса).
        // g: Graphics обект
        // brush: Четка за запълване
        private void DrawCircle(Graphics g, Brush brush)
        {
            g.FillEllipse(brush, posX, posY, size, size);

            // Добавяне на блясък ефект
            using (SolidBrush highlightBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255)))
            {
                g.FillEllipse(highlightBrush, posX + size * 0.2f, posY + size * 0.1f,
                             size * 0.3f, size * 0.3f);
            }
        }

        // Рисува квадратна форма.
        // g: Graphics обект
        // brush: Четка за запълване
        private void DrawSquare(Graphics g, Brush brush)
        {
            g.FillRectangle(brush, posX, posY, size, size);

            // Добавяне на рамка за дълбочина
            using (Pen borderPen = new Pen(Color.FromArgb(100, 0, 0, 0), 2))
            {
                g.DrawRectangle(borderPen, posX, posY, size, size);
            }
        }

        // Рисува триъгълна форма.
        // g: Graphics обект
        // brush: Четка за запълване
        //
        // Триъгълникът се дефинира чрез три точки:
        // - Връх в средата отгоре
        // - Долен ляв ъгъл
        // - Долен десен ъгъл
        private void DrawTriangle(Graphics g, Brush brush)
        {
            // Дефиниране на трите върха на триъгълника
            PointF[] points = new PointF[]
            {
                new PointF(posX + size / 2, posY),           // Връх
                new PointF(posX, posY + size),               // Долен ляв
                new PointF(posX + size, posY + size)         // Долен десен
            };

            g.FillPolygon(brush, points);
        }

        // Рисува петолъчна звезда.
        // g: Graphics обект
        // brush: Четка за запълване
        //
        // Звездата се създава чрез изчисляване на 10 точки -
        // 5 външни върха и 5 вътрешни точки.
        private void DrawStar(Graphics g, Brush brush)
        {
            // Център на звездата
            float centerX = posX + size / 2;
            float centerY = posY + size / 2;

            // Радиуси на външните и вътрешните върхове
            float outerRadius = size / 2;
            float innerRadius = size / 4;

            // Създаване на точките на звездата
            PointF[] starPoints = new PointF[10];

            for (int i = 0; i < 10; i++)
            {
                // Ъгъл за текущата точка (започваме от върха)
                double angle = Math.PI / 2 + i * Math.PI / 5;

                // Редуваме външен и вътрешен радиус
                float radius = (i % 2 == 0) ? outerRadius : innerRadius;

                starPoints[i] = new PointF(
                    centerX + (float)(radius * Math.Cos(angle)),
                    centerY - (float)(radius * Math.Sin(angle))
                );
            }

            g.FillPolygon(brush, starPoints);
        }

        #endregion
    }
}
