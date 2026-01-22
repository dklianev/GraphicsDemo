/*
 * ============================================================================
 * Форма: MainForm - Главна форма на приложението
 * ============================================================================
 * Описание: Главната форма на приложението за демонстрация на графика.
 *           Съдържа функционалност за:
 *           - Рисуване на различни геометрични фигури
 *           - Анимация на движещи се обекти
 *           - Многоезичен интерфейс (български/английски)
 *           - Стилизиране на текст и работа с изображения
 * 
 * Студент: Димитър Клянев
 * Факултетен номер: F112194
 * 
 * Курс: CSCB579 Програмиране на приложения с Microsoft Visual C# .NET
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace GraphicsDemo
{
    // Главна форма на приложението, съдържаща цялата логика за
    // графично рисуване, анимации и многоезичен интерфейс.
    //
    // Тази форма демонстрира:
    // - Работа с Graphics класа за 2D графика
    // - Използване на Timer за плавни анимации
    // - Обработка на Mouse събития за интерактивно рисуване
    // - Локализация чрез ResourceManager
    // - Запазване и зареждане на изображения
    // 
    // Студент: Димитър Клянев
    // Факултетен номер: F112194
    public partial class MainForm : Form
    {
        #region Полета (Private Fields)

        // ========================================
        // Полета за рисуване
        // ========================================

        // Bitmap за съхранение на нарисуваното съдържание
        private Bitmap drawingBitmap;

        // Graphics обект за рисуване върху bitmap-а
        private Graphics drawingGraphics;

        // Текущ избран цвят за рисуване
        private Color currentColor = Color.DarkBlue;

        // Текущ избран шрифт за текст
        private Font currentFont = new Font("Segoe UI", 24, FontStyle.Bold);

        // Дебелина на линията при рисуване
        private float lineWidth = 3f;

        // Начална точка при рисуване с мишка
        private Point? startPoint = null;

        // Флаг дали мишката е натисната
        private bool isMouseDown = false;

        // Последна позиция на мишката
        private Point lastMousePosition;

        // Заредено изображение
        private Image loadedImage = null;

        // ========================================
        // Полета за анимация
        // ========================================

        // Списък с анимирани форми
        private List<AnimatedShape> animatedShapes = new List<AnimatedShape>();

        // Генератор на случайни числа
        private Random random = new Random();

        // ========================================
        // Полета за локализация
        // ========================================

        // ResourceManager за достъп до локализирани стрингове
        private ResourceManager resourceManager;

        // Текуща култура за локализация
        private CultureInfo currentCulture;

        #endregion

        #region Конструктор (Constructor)

        // Инициализира нова инстанция на главната форма.
        //
        // Конструкторът извършва следните действия:
        // 1. Инициализира всички компоненти чрез InitializeComponent()
        // 2. Активира двойно буфериране за плавни анимации
        // 3. Инициализира ResourceManager за многоезичен интерфейс
        // 4. Добавя няколко начални анимирани форми
        public MainForm()
        {
            // Инициализация на компонентите от Designer
            InitializeComponent();

            // Активиране на двойно буфериране за избягване на трептене
            // Това е важно за плавни анимации
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

            // ВАЖНО: Активиране на Double Buffering за панелите чрез Reflection!
            // Това предотвратява "примигването" при рисуване.
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelDrawing, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelAnimation, new object[] { true });

            // Инициализация на ResourceManager (по подразбиране български)
            resourceManager = new ResourceManager("GraphicsDemo.Resources.Strings",
                typeof(MainForm).Assembly);
            currentCulture = new CultureInfo("bg-BG");

            // Регистриране на събитието Load за инициализация след създаване на формата
            this.Load += MainForm_Load;
        }

        // Обработва събитието Load на формата.
        // Извиква се след като формата е създадена и всички контроли имат размери.
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Инициализация на битмап за рисуване
            InitializeDrawingBitmap();

            // Добавяне на начални анимирани форми
            InitializeAnimatedShapes();

            // Стартиране на анимацията автоматично
            animationTimer.Start();
        }

        #endregion

        #region Инициализационни методи (Initialization Methods)

        // Инициализира bitmap-а за рисуване с размерите на панела.
        //
        // Bitmap-ът се използва за съхранение на нарисуваното съдържание,
        // което позволява да се запази между преначертаванията.
        private void InitializeDrawingBitmap()
        {
            // Проверка дали panelDrawing е инициализиран
            if (panelDrawing == null) return;

            // Освобождаване на стари ресурси ако има такива
            if (drawingBitmap != null)
            {
                drawingGraphics?.Dispose();
                drawingBitmap.Dispose();
            }

            // Създаване на нов bitmap с размерите на панела (минимум 10x10)
            int width = Math.Max(10, panelDrawing.ClientSize.Width);
            int height = Math.Max(10, panelDrawing.ClientSize.Height);
            
            try
            {
                drawingBitmap = new Bitmap(width, height);

                // Създаване на Graphics обект за рисуване
                drawingGraphics = Graphics.FromImage(drawingBitmap);

                // Настройка за високо качество на рисуване
                drawingGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                drawingGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                drawingGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Изчистване с бял фон
                drawingGraphics.Clear(Color.White);
            }
            catch (Exception)
            {
                // Ако възникне грешка, създаваме минимален bitmap
                drawingBitmap = new Bitmap(100, 100);
                drawingGraphics = Graphics.FromImage(drawingBitmap);
                drawingGraphics.Clear(Color.White);
            }
        }

        // Инициализира начални анимирани форми за демонстрация.
        //
        // Създава няколко форми с различни типове, които ще се движат
        // в областта за анимация.
        private void InitializeAnimatedShapes()
        {
            // Проверка дали panelAnimation е инициализиран
            if (panelAnimation == null) return;

            // Изчистване на съществуващи форми
            animatedShapes.Clear();

            try
            {
                // Добавяне на 5 случайни форми
                int animWidth = Math.Max(100, panelAnimation.ClientSize.Width);
                int animHeight = Math.Max(100, panelAnimation.ClientSize.Height);

                for (int i = 0; i < 5; i++)
                {
                    animatedShapes.Add(AnimatedShape.CreateRandom(animWidth, animHeight));
                }
            }
            catch (Exception)
            {
                // Игнориране на грешки при инициализация
            }
        }

        #endregion

        #region Събития на формата (Form Events)

        // Обработва събитието Paint на панела за рисуване.
        // sender: Обект, пораждащ събитието
        // e: Аргументи на събитието
        //
        // Този метод се извиква когато панелът трябва да се преначертае.
        // Изчертава bitmap-а с нарисуваното съдържание.
        private void panelDrawing_Paint(object sender, PaintEventArgs e)
        {
            // Рисуване на bitmap върху панела
            if (drawingBitmap != null)
            {
                e.Graphics.DrawImage(drawingBitmap, 0, 0);
            }

            // Рисуване на заредено изображение ако има такова
            if (loadedImage != null)
            {
                e.Graphics.DrawImage(loadedImage, 10, 10);
            }
        }

        // Обработва събитието Paint на панела за анимация.
        // sender: Обект, пораждащ събитието
        // e: Аргументи на събитието
        //
        // Изчертава всички анимирани форми върху панела.
        // Използва double-buffering за плавност.
        private void panelAnimation_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Рисуване на всяка анимирана форма
            foreach (AnimatedShape shape in animatedShapes)
            {
                shape.Draw(g);
            }

            // Рисуване на демонстративен текст в центъра
            DrawAnimationTitle(g);
        }

        // Рисува заглавие в панела за анимация.
        // g: Graphics обект
        private void DrawAnimationTitle(Graphics g)
        {
            string title = "Анимация";
            using (Font font = new Font("Segoe UI", 14, FontStyle.Bold))
            {
                SizeF textSize = g.MeasureString(title, font);
                float x = (panelAnimation.Width - textSize.Width) / 2;
                float y = 10;

                // Рисуване с ефект сянка
                GraphicsHelper.DrawTextWithShadow(g, title, font,
                    Color.White, Color.FromArgb(100, 0, 0, 0),
                    new PointF(x, y), 2);
            }
        }

        #endregion

        #region Събития на мишка (Mouse Events)

        // Обработва събитието MouseDown на панела за рисуване.
        // sender: Обект, пораждащ събитието
        // e: Аргументи на събитието
        //
        // Запомня началната позиция при започване на рисуване.
        private void panelDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                startPoint = e.Location;
                lastMousePosition = e.Location;
            }
        }

        // Обработва събитието MouseMove на панела за рисуване.
        // sender: Обект, пораждащ събитието
        // e: Аргументи на събитието
        //
        // При натисната лява мишка рисува свободна линия (free-hand drawing).
        private void panelDrawing_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown && drawingGraphics != null)
            {
                // Рисуване на линия от предишната до текущата позиция
                using (Pen pen = new Pen(currentColor, lineWidth))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    drawingGraphics.DrawLine(pen, lastMousePosition, e.Location);
                }

                lastMousePosition = e.Location;

                // Преначертаване на панела
                panelDrawing.Invalidate();
            }

            // Актуализиране на статус бара с координатите
            statusLabel.Text = $"Позиция: ({e.X}, {e.Y}) | Цвят: {currentColor.Name} | Студент: Димитър Клянев F112194";
        }

        // Обработва събитието MouseUp на панела за рисуване.
        // sender: Обект, пораждащ събитието
        // e: Аргументи на събитието
        //
        // Приключва рисуването с мишка.
        private void panelDrawing_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            startPoint = null;
        }

        #endregion

        #region Събития на бутони за рисуване (Drawing Button Events)

        // Рисува правоъгълник на случайна позиция.
        // sender: Бутонът, породил събитието
        // e: Аргументи на събитието
        //
        // Демонстрира използването на DrawRectangle и FillRectangle методите.
        private void btnDrawRect_Click(object sender, EventArgs e)
        {
            if (drawingGraphics == null) return;

            // Генериране на случайни параметри
            int x = random.Next(10, panelDrawing.Width - 110);
            int y = random.Next(10, panelDrawing.Height - 110);
            int width = random.Next(50, 100);
            int height = random.Next(50, 100);

            // Рисуване на запълнен правоъгълник с градиент
            Rectangle rect = new Rectangle(x, y, width, height);
            using (LinearGradientBrush brush = new LinearGradientBrush(rect,
                currentColor, ControlPaint.Light(currentColor), 45f))
            {
                drawingGraphics.FillRectangle(brush, rect);
            }

            // Рисуване на контур
            using (Pen pen = new Pen(ControlPaint.Dark(currentColor), 2))
            {
                drawingGraphics.DrawRectangle(pen, rect);
            }

            panelDrawing.Invalidate();
        }

        // Рисува елипса на случайна позиция.
        // sender: Бутонът, породил събитието
        // e: Аргументи на събитието
        //
        // Демонстрира използването на FillEllipse и DrawEllipse методите.
        private void btnDrawEllipse_Click(object sender, EventArgs e)
        {
            if (drawingGraphics == null) return;

            // Генериране на случайни параметри
            int x = random.Next(10, panelDrawing.Width - 110);
            int y = random.Next(10, panelDrawing.Height - 110);
            int width = random.Next(50, 100);
            int height = random.Next(50, 100);

            // Рисуване на запълнена елипса с градиент
            Rectangle rect = new Rectangle(x, y, width, height);
            using (LinearGradientBrush brush = new LinearGradientBrush(rect,
                currentColor, ControlPaint.LightLight(currentColor),
                LinearGradientMode.ForwardDiagonal))
            {
                drawingGraphics.FillEllipse(brush, rect);
            }

            // Добавяне на highlight ефект
            using (SolidBrush highlightBrush = new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
            {
                drawingGraphics.FillEllipse(highlightBrush, x + width / 4, y + height / 6,
                    width / 3, height / 3);
            }

            panelDrawing.Invalidate();
        }

        // Рисува линия на случайна позиция.
        // sender: Бутонът, породил събитието
        // e: Аргументи на събитието
        //
        // Демонстрира използването на DrawLine метода с различни стилове.
        private void btnDrawLine_Click(object sender, EventArgs e)
        {
            if (drawingGraphics == null) return;

            // Генериране на случайни точки
            int x1 = random.Next(10, panelDrawing.Width - 10);
            int y1 = random.Next(10, panelDrawing.Height - 10);
            int x2 = random.Next(10, panelDrawing.Width - 10);
            int y2 = random.Next(10, panelDrawing.Height - 10);

            // Рисуване на линия
            using (Pen pen = new Pen(currentColor, lineWidth + 2))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.ArrowAnchor;
                drawingGraphics.DrawLine(pen, x1, y1, x2, y2);
            }

            panelDrawing.Invalidate();
        }

        // Рисува стилизиран текст.
        // sender: Бутонът, породил събитието
        // e: Аргументи на събитието
        //
        // Демонстрира различни техники за стилизиране на текст:
        // - Текст със сянка
        // - Текст с контур
        // - Градиентен текст
        private void btnDrawText_Click(object sender, EventArgs e)
        {
            if (drawingGraphics == null) return;

            // Генериране на случайни позиция
            int x = random.Next(10, Math.Max(20, panelDrawing.Width - 200));
            int y = random.Next(10, Math.Max(20, panelDrawing.Height - 100));

            // Избор на случаен стил
            int style = random.Next(3);

            switch (style)
            {
                case 0:
                    // Текст със сянка
                    GraphicsHelper.DrawTextWithShadow(drawingGraphics, "Hello!",
                        currentFont, currentColor, Color.Gray, new PointF(x, y), 3);
                    break;

                case 1:
                    // Текст с контур
                    GraphicsHelper.DrawOutlinedText(drawingGraphics, "Здравей!",
                        currentFont, currentColor, Color.White, new PointF(x, y), 3);
                    break;

                case 2:
                    // Градиентен текст
                    GraphicsHelper.DrawGradientText(drawingGraphics, "Graphics",
                        currentFont, currentColor, ControlPaint.Light(currentColor),
                        new PointF(x, y));
                    break;
            }

            panelDrawing.Invalidate();
        }

        // Изчиства панела за рисуване.
        // sender: Бутонът, породил събитието
        // e: Аргументи на събитието
        //
        // Запълва bitmap-а с бял цвят и премахва зареденото изображение.
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (drawingGraphics != null)
            {
                drawingGraphics.Clear(Color.White);
            }

            loadedImage?.Dispose();
            loadedImage = null;

            panelDrawing.Invalidate();
        }

        #endregion

        #region Събития на бутони за анимация (Animation Button Events)

        // Стартира анимацията.
        private void btnStart_Click(object sender, EventArgs e)
        {
            animationTimer.Start();
        }

        // Спира анимацията.
        private void btnStop_Click(object sender, EventArgs e)
        {
            animationTimer.Stop();
        }

        // Добавя нова случайна анимирана форма.
        private void btnAddShape_Click(object sender, EventArgs e)
        {
            // Ограничаване на максималния брой форми
            if (animatedShapes.Count < 20)
            {
                animatedShapes.Add(AnimatedShape.CreateRandom(
                    panelAnimation.Width, panelAnimation.Height));
            }
            else
            {
                MessageBox.Show("Максимален брой фигури достигнат (20)!",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Обработва тика на таймера за анимация.
        // sender: Таймерът, породил събитието
        // e: Аргументи на събитието
        //
        // Този метод се извиква периодично (~60 пъти в секунда).
        // Актуализира позициите на всички анимирани форми и
        // преначертава панела за анимация.
        private void animationTimer_Tick(object sender, EventArgs e)
        {
            // Актуализиране на позициите на всички форми
            int width = panelAnimation.Width;
            int height = panelAnimation.Height;

            foreach (AnimatedShape shape in animatedShapes)
            {
                shape.Move(width, height);
            }

            // Преначертаване на панела за анимация
            panelAnimation.Invalidate();
        }

        #endregion

        #region Събития на менюто (Menu Events)

        // ========================================
        // Меню Файл
        // ========================================

        // Създава нов празен документ.
        private void menuFileNew_Click(object sender, EventArgs e)
        {
            // Изчистване на всичко
            btnClear_Click(sender, e);
            animatedShapes.Clear();
            InitializeAnimatedShapes();
        }

        // Отваря изображение от файл.
        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Зареждане на изображението
                    loadedImage?.Dispose();
                    loadedImage = Image.FromFile(openFileDialog.FileName);

                    // Преоразмеряване ако е твърде голямо
                    if (loadedImage.Width > panelDrawing.Width - 20 ||
                        loadedImage.Height > panelDrawing.Height - 20)
                    {
                        Image resized = GraphicsHelper.ResizeImage(loadedImage,
                            panelDrawing.Width - 20, panelDrawing.Height - 20);
                        loadedImage.Dispose();
                        loadedImage = resized;
                    }

                    panelDrawing.Invalidate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Грешка при зареждане на изображението:\n{ex.Message}",
                        "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Запазва рисунката като изображение.
        private void menuFileSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Определяне на формата според разширението
                    ImageFormat format = ImageFormat.Png;
                    string ext = System.IO.Path.GetExtension(saveFileDialog.FileName).ToLower();

                    switch (ext)
                    {
                        case ".jpg":
                        case ".jpeg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                    }

                    // Запазване на bitmap-а
                    drawingBitmap.Save(saveFileDialog.FileName, format);

                    MessageBox.Show("Изображението е запазено успешно!",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Грешка при запазване:\n{ex.Message}",
                        "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Затваря приложението.
        private void menuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ========================================
        // Меню Редактиране
        // ========================================

        // Изчиства цялата рисунка.
        private void menuEditClear_Click(object sender, EventArgs e)
        {
            btnClear_Click(sender, e);
        }

        // Отваря диалог за избор на цвят.
        private void menuEditColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = currentColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                currentColor = colorDialog.Color;
            }
        }

        // Отваря диалог за избор на шрифт.
        private void menuEditFont_Click(object sender, EventArgs e)
        {
            fontDialog.Font = currentFont;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                currentFont = fontDialog.Font;
            }
        }

        // ========================================
        // Меню Изглед
        // ========================================

        // Превключва видимостта на панела за графика.
        private void menuViewGraphics_Click(object sender, EventArgs e)
        {
            splitContainer.Panel1Collapsed = !menuViewGraphics.Checked;
        }

        // Превключва видимостта на панела за анимация.
        private void menuViewAnimation_Click(object sender, EventArgs e)
        {
            splitContainer.Panel2Collapsed = !menuViewAnimation.Checked;
        }

        // Показва демонстрация на стилизиран текст.
        private void menuViewText_Click(object sender, EventArgs e)
        {
            if (drawingGraphics == null) return;

            // Рисуване на различни стилове текст
            int y = 50;

            using (Font titleFont = new Font("Segoe UI", 20, FontStyle.Bold))
            {
                GraphicsHelper.DrawTextWithShadow(drawingGraphics,
                    "Демо: Стилизиран текст", titleFont,
                    Color.DarkBlue, Color.LightGray, new PointF(20, y));
            }

            y += 60;

            using (Font demoFont = new Font("Arial", 18, FontStyle.Bold))
            {
                GraphicsHelper.DrawOutlinedText(drawingGraphics,
                    "Текст с контур", demoFont,
                    Color.Orange, Color.DarkOrange, new PointF(20, y));
            }

            y += 50;

            using (Font gradientFont = new Font("Georgia", 22, FontStyle.Italic))
            {
                GraphicsHelper.DrawGradientText(drawingGraphics,
                    "Градиентен текст", gradientFont,
                    Color.Purple, Color.Magenta, new PointF(20, y));
            }

            panelDrawing.Invalidate();
        }

        // ========================================
        // Меню Език
        // ========================================

        // Превключва интерфейса на български.
        private void menuLanguageBulgarian_Click(object sender, EventArgs e)
        {
            ChangeLanguage("bg-BG");
            menuLanguageBulgarian.Checked = true;
            menuLanguageEnglish.Checked = false;
        }

        // Превключва интерфейса на английски.
        private void menuLanguageEnglish_Click(object sender, EventArgs e)
        {
            ChangeLanguage("en");
            menuLanguageBulgarian.Checked = false;
            menuLanguageEnglish.Checked = true;
        }

        // Променя езика на интерфейса.
        // cultureName: Име на културата (напр. "bg-BG", "en")
        //
        // Този метод:
        // 1. Задава новата култура на текущата нишка
        // 2. Актуализира всички текстове на контролите
        // 3. Показва съобщение за успешна смяна
        private void ChangeLanguage(string cultureName)
        {
            try
            {
                // Задаване на новата култура
                currentCulture = new CultureInfo(cultureName);
                Thread.CurrentThread.CurrentUICulture = currentCulture;

                // Актуализиране на текстовете
                UpdateLocalizedTexts();

                MessageBox.Show(
                    resourceManager.GetString("MsgLanguageChanged", currentCulture),
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка при смяна на език: {ex.Message}",
                    "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Актуализира текстовете на всички контроли според езика.
        private void UpdateLocalizedTexts()
        {
            // Заглавие на формата
            this.Text = GetLocalizedString("AppTitle") + " - Димитър Клянев F112194";

            // Меню Файл
            menuFile.Text = GetLocalizedString("MenuFile");
            menuFileNew.Text = GetLocalizedString("MenuFileNew");
            menuFileOpen.Text = GetLocalizedString("MenuFileOpen");
            menuFileSave.Text = GetLocalizedString("MenuFileSave");
            menuFileExit.Text = GetLocalizedString("MenuFileExit");

            // Меню Редактиране
            menuEdit.Text = GetLocalizedString("MenuEdit");
            menuEditClear.Text = GetLocalizedString("MenuEditClear");
            menuEditColor.Text = GetLocalizedString("MenuEditColor");
            menuEditFont.Text = GetLocalizedString("MenuEditFont");

            // Меню Изглед
            menuView.Text = GetLocalizedString("MenuView");
            menuViewGraphics.Text = GetLocalizedString("MenuViewGraphics");
            menuViewAnimation.Text = GetLocalizedString("MenuViewAnimation");
            menuViewText.Text = GetLocalizedString("MenuViewText");

            // Меню Език
            menuLanguage.Text = GetLocalizedString("MenuLanguage");
            menuLanguageBulgarian.Text = GetLocalizedString("MenuLanguageBulgarian");
            menuLanguageEnglish.Text = GetLocalizedString("MenuLanguageEnglish");

            // Меню Помощ
            menuHelp.Text = GetLocalizedString("MenuHelp");
            menuHelpAbout.Text = GetLocalizedString("MenuHelpAbout");

            // Бутони за рисуване
            btnDrawRect.Text = GetLocalizedString("BtnDrawRect");
            btnDrawEllipse.Text = GetLocalizedString("BtnDrawEllipse");
            btnDrawLine.Text = GetLocalizedString("BtnDrawLine");
            btnDrawText.Text = GetLocalizedString("BtnDrawText");
            btnClear.Text = GetLocalizedString("BtnClear");

            // Бутони за анимация
            btnStart.Text = GetLocalizedString("BtnStart");
            btnStop.Text = GetLocalizedString("BtnStop");
            btnAddShape.Text = GetLocalizedString("BtnAddShape");

            // Статус бар
            statusLabel.Text = GetLocalizedString("StudentInfo");
        }

        // Получава локализиран стринг по ключ.
        // key: Ключ в ресурсния файл
        // връща: Локализиран текст или ключът ако не е намерен
        private string GetLocalizedString(string key)
        {
            try
            {
                string value = resourceManager.GetString(key, currentCulture);
                return value ?? key;
            }
            catch
            {
                return key;
            }
        }

        // ========================================
        // Меню Помощ
        // ========================================

        // Показва информация за програмата.
        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            string aboutText = GetLocalizedString("MsgAbout");
            MessageBox.Show(aboutText, GetLocalizedString("MenuHelpAbout"),
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Resize Events

        // Обработва промяна на размера на формата.
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Преинициализиране на bitmap-а при промяна на размера
            if (panelDrawing.Width > 0 && panelDrawing.Height > 0)
            {
                // Запазваме старото съдържание
                Bitmap oldBitmap = drawingBitmap;

                // Създаваме нов bitmap
                InitializeDrawingBitmap();

                // Копираме старото съдържание
                if (oldBitmap != null)
                {
                    drawingGraphics.DrawImage(oldBitmap, 0, 0);
                    oldBitmap.Dispose();
                }
            }
        }

        #endregion

        #region Dispose Pattern

        // Освобождава ресурсите при затваряне.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Спиране на таймера
            animationTimer.Stop();

            // Освобождаване на ресурси
            drawingGraphics?.Dispose();
            drawingBitmap?.Dispose();
            loadedImage?.Dispose();
            currentFont?.Dispose();

            base.OnFormClosing(e);
        }

        #endregion
    }
}
