

using System.Drawing;
using System.Windows.Forms;

namespace GraphicsDemo
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();


            this.menuStrip = new MenuStrip();
            this.menuFile = new ToolStripMenuItem();
            this.menuFileNew = new ToolStripMenuItem();
            this.menuFileOpen = new ToolStripMenuItem();
            this.menuFileSave = new ToolStripMenuItem();
            this.menuFileSeparator = new ToolStripSeparator();
            this.menuFileExit = new ToolStripMenuItem();
            this.menuEdit = new ToolStripMenuItem();
            this.menuEditClear = new ToolStripMenuItem();
            this.menuEditSeparator = new ToolStripSeparator();
            this.menuEditColor = new ToolStripMenuItem();
            this.menuEditFont = new ToolStripMenuItem();
            this.menuView = new ToolStripMenuItem();
            this.menuViewGraphics = new ToolStripMenuItem();
            this.menuViewAnimation = new ToolStripMenuItem();
            this.menuViewText = new ToolStripMenuItem();
            this.menuLanguage = new ToolStripMenuItem();
            this.menuLanguageBulgarian = new ToolStripMenuItem();
            this.menuLanguageEnglish = new ToolStripMenuItem();
            this.menuHelp = new ToolStripMenuItem();
            this.menuHelpAbout = new ToolStripMenuItem();

            this.splitContainer = new SplitContainer();
            this.panelDrawing = new Panel();
            this.panelAnimation = new PictureBox();
            this.panelControls = new Panel();
            this.statusStrip = new StatusStrip();
            this.statusLabel = new ToolStripStatusLabel();

            this.btnDrawRect = new Button();
            this.btnDrawEllipse = new Button();
            this.btnDrawLine = new Button();
            this.btnDrawText = new Button();
            this.btnClear = new Button();
            this.btnStart = new Button();
            this.btnStop = new Button();
            this.btnAddShape = new Button();

            this.groupBoxDrawing = new GroupBox();
            this.groupBoxAnimation = new GroupBox();

            this.animationTimer = new Timer(this.components);
            this.colorDialog = new ColorDialog();
            this.fontDialog = new FontDialog();
            this.openFileDialog = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();


            this.menuStrip.Items.AddRange(new ToolStripItem[] {
                this.menuFile, this.menuEdit, this.menuView, this.menuLanguage, this.menuHelp
            });
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(1000, 24);


            this.menuFile.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuFileNew, this.menuFileOpen, this.menuFileSave, this.menuFileSeparator, this.menuFileExit
            });
            this.menuFile.Text = "Файл";
            this.menuFileNew.Text = "Нов";
            this.menuFileNew.ShortcutKeys = Keys.Control | Keys.N;
            this.menuFileNew.Click += menuFileNew_Click;
            this.menuFileOpen.Text = "Отвори изображение...";
            this.menuFileOpen.ShortcutKeys = Keys.Control | Keys.O;
            this.menuFileOpen.Click += menuFileOpen_Click;
            this.menuFileSave.Text = "Запази като...";
            this.menuFileSave.ShortcutKeys = Keys.Control | Keys.S;
            this.menuFileSave.Click += menuFileSave_Click;
            this.menuFileExit.Text = "Изход";
            this.menuFileExit.Click += menuFileExit_Click;


            this.menuEdit.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuEditClear, this.menuEditSeparator, this.menuEditColor, this.menuEditFont
            });
            this.menuEdit.Text = "Редактиране";
            this.menuEditClear.Text = "Изчисти всичко";
            this.menuEditClear.Click += menuEditClear_Click;
            this.menuEditColor.Text = "Избери цвят...";
            this.menuEditColor.Click += menuEditColor_Click;
            this.menuEditFont.Text = "Избери шрифт...";
            this.menuEditFont.Click += menuEditFont_Click;


            this.menuView.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuViewGraphics, this.menuViewAnimation, this.menuViewText
            });
            this.menuView.Text = "Изглед";
            this.menuViewGraphics.Text = "Графика";
            this.menuViewGraphics.Checked = true;
            this.menuViewGraphics.CheckOnClick = true;
            this.menuViewGraphics.Click += menuViewGraphics_Click;
            this.menuViewAnimation.Text = "Анимация";
            this.menuViewAnimation.Checked = true;
            this.menuViewAnimation.CheckOnClick = true;
            this.menuViewAnimation.Click += menuViewAnimation_Click;
            this.menuViewText.Text = "Стилизиран текст";
            this.menuViewText.Click += menuViewText_Click;


            this.menuLanguage.DropDownItems.AddRange(new ToolStripItem[] {
                this.menuLanguageBulgarian, this.menuLanguageEnglish
            });
            this.menuLanguage.Text = "Език";
            this.menuLanguageBulgarian.Text = "Български";
            this.menuLanguageBulgarian.Checked = true;
            this.menuLanguageBulgarian.Click += menuLanguageBulgarian_Click;
            this.menuLanguageEnglish.Text = "English";
            this.menuLanguageEnglish.Click += menuLanguageEnglish_Click;


            this.menuHelp.DropDownItems.AddRange(new ToolStripItem[] { this.menuHelpAbout });
            this.menuHelp.Text = "Помощ";
            this.menuHelpAbout.Text = "За програмата";
            this.menuHelpAbout.ShortcutKeys = Keys.F1;
            this.menuHelpAbout.Click += menuHelpAbout_Click;


            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelAnimation)).BeginInit();

            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Location = new Point(0, 24);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.SplitterDistance = 500;
            this.splitContainer.Panel1.Controls.Add(this.panelDrawing);
            this.splitContainer.Panel2.Controls.Add(this.panelAnimation);


            this.panelDrawing.BackColor = Color.White;
            this.panelDrawing.BorderStyle = BorderStyle.FixedSingle;
            this.panelDrawing.Dock = DockStyle.Fill;
            this.panelDrawing.Name = "panelDrawing";
            this.panelDrawing.Paint += panelDrawing_Paint;
            this.panelDrawing.MouseDown += panelDrawing_MouseDown;
            this.panelDrawing.MouseMove += panelDrawing_MouseMove;
            this.panelDrawing.MouseUp += panelDrawing_MouseUp;


            this.panelAnimation.BackColor = Color.FromArgb(30, 30, 40);
            this.panelAnimation.BorderStyle = BorderStyle.FixedSingle;
            this.panelAnimation.Dock = DockStyle.Fill;
            this.panelAnimation.Name = "panelAnimation";
            this.panelAnimation.Paint += panelAnimation_Paint;

            ((System.ComponentModel.ISupportInitialize)(this.panelAnimation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();


            this.panelControls.BackColor = Color.FromArgb(45, 45, 48);
            this.panelControls.Dock = DockStyle.Bottom;
            this.panelControls.Height = 80;
            this.panelControls.Name = "panelControls";


            this.groupBoxDrawing.Text = "Рисуване";
            this.groupBoxDrawing.ForeColor = Color.White;
            this.groupBoxDrawing.Location = new Point(10, 5);
            this.groupBoxDrawing.Size = new Size(520, 70);


            int btnY = 25;
            int btnW = 75;
            int btnH = 30;

            this.btnDrawRect.Location = new Point(10, btnY);
            this.btnDrawRect.Size = new Size(110, btnH);
            this.btnDrawRect.Text = "Правоъгълник";
            this.btnDrawRect.FlatStyle = FlatStyle.Flat;
            this.btnDrawRect.BackColor = Color.FromArgb(0, 122, 204);
            this.btnDrawRect.ForeColor = Color.White;
            this.btnDrawRect.Click += btnDrawRect_Click;

            this.btnDrawEllipse.Location = new Point(130, btnY);
            this.btnDrawEllipse.Size = new Size(80, btnH);
            this.btnDrawEllipse.Text = "Елипса";
            this.btnDrawEllipse.FlatStyle = FlatStyle.Flat;
            this.btnDrawEllipse.BackColor = Color.FromArgb(0, 122, 204);
            this.btnDrawEllipse.ForeColor = Color.White;
            this.btnDrawEllipse.Click += btnDrawEllipse_Click;

            this.btnDrawLine.Location = new Point(220, btnY);
            this.btnDrawLine.Size = new Size(80, btnH);
            this.btnDrawLine.Text = "Линия";
            this.btnDrawLine.FlatStyle = FlatStyle.Flat;
            this.btnDrawLine.BackColor = Color.FromArgb(0, 122, 204);
            this.btnDrawLine.ForeColor = Color.White;
            this.btnDrawLine.Click += btnDrawLine_Click;

            this.btnDrawText.Location = new Point(310, btnY);
            this.btnDrawText.Size = new Size(80, btnH);
            this.btnDrawText.Text = "Текст";
            this.btnDrawText.FlatStyle = FlatStyle.Flat;
            this.btnDrawText.BackColor = Color.FromArgb(0, 122, 204);
            this.btnDrawText.ForeColor = Color.White;
            this.btnDrawText.Click += btnDrawText_Click;

            this.btnClear.Location = new Point(400, btnY);
            this.btnClear.Size = new Size(80, btnH);
            this.btnClear.Text = "Изчисти";
            this.btnClear.FlatStyle = FlatStyle.Flat;
            this.btnClear.BackColor = Color.FromArgb(200, 50, 50);
            this.btnClear.ForeColor = Color.White;
            this.btnClear.Click += btnClear_Click;

            this.groupBoxDrawing.Controls.AddRange(new Control[] {
                this.btnDrawRect, this.btnDrawEllipse, this.btnDrawLine, this.btnDrawText, this.btnClear
            });


            this.groupBoxAnimation.Text = "Анимация";
            this.groupBoxAnimation.ForeColor = Color.White;
            this.groupBoxAnimation.Location = new Point(540, 5);
            this.groupBoxAnimation.Size = new Size(300, 70);

            this.btnStart.Location = new Point(10, btnY);
            this.btnStart.Size = new Size(btnW, btnH);
            this.btnStart.Text = "Старт";
            this.btnStart.FlatStyle = FlatStyle.Flat;
            this.btnStart.BackColor = Color.FromArgb(50, 150, 50);
            this.btnStart.ForeColor = Color.White;
            this.btnStart.Click += btnStart_Click;

            this.btnStop.Location = new Point(90, btnY);
            this.btnStop.Size = new Size(btnW, btnH);
            this.btnStop.Text = "Стоп";
            this.btnStop.FlatStyle = FlatStyle.Flat;
            this.btnStop.BackColor = Color.FromArgb(150, 50, 50);
            this.btnStop.ForeColor = Color.White;
            this.btnStop.Click += btnStop_Click;

            this.btnAddShape.Location = new Point(170, btnY);
            this.btnAddShape.Size = new Size(110, btnH);
            this.btnAddShape.Text = "Добави фигура";
            this.btnAddShape.FlatStyle = FlatStyle.Flat;
            this.btnAddShape.BackColor = Color.FromArgb(100, 100, 180);
            this.btnAddShape.ForeColor = Color.White;
            this.btnAddShape.Click += btnAddShape_Click;

            this.groupBoxAnimation.Controls.AddRange(new Control[] {
                this.btnStart, this.btnStop, this.btnAddShape
            });

            this.panelControls.Controls.Add(this.groupBoxDrawing);
            this.panelControls.Controls.Add(this.groupBoxAnimation);


            this.statusStrip.Items.Add(this.statusLabel);
            this.statusStrip.Dock = DockStyle.Bottom;
            this.statusStrip.BackColor = Color.FromArgb(0, 122, 204);
            this.statusLabel.Text = "Студент: Dimitar Klianev | Ф.Н.: F112194";
            this.statusLabel.ForeColor = Color.White;

            // ========================================
            // Timer
            // ========================================
            this.animationTimer.Interval = 16;
            this.animationTimer.Tick += animationTimer_Tick;

            // ========================================
            // Диалози
            // ========================================
            this.openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Всички файлове|*.*";
            this.saveFileDialog.Filter = "PNG изображение|*.png|JPEG изображение|*.jpg|BMP изображение|*.bmp";

            // ========================================
            // MainForm
            // ========================================
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ClientSize = new Size(1000, 650);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panelControls);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new Size(800, 600);
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Графична Демонстрация - Dimitar Klianev F112194";

            this.ResumeLayout(false);
            this.PerformLayout();
        }



        // Декларации
        private MenuStrip menuStrip;
        private ToolStripMenuItem menuFile, menuFileNew, menuFileOpen, menuFileSave, menuFileExit;
        private ToolStripSeparator menuFileSeparator;
        private ToolStripMenuItem menuEdit, menuEditClear, menuEditColor, menuEditFont;
        private ToolStripSeparator menuEditSeparator;
        private ToolStripMenuItem menuView, menuViewGraphics, menuViewAnimation, menuViewText;
        private ToolStripMenuItem menuLanguage, menuLanguageBulgarian, menuLanguageEnglish;
        private ToolStripMenuItem menuHelp, menuHelpAbout;

        private SplitContainer splitContainer;
        private Panel panelDrawing;
        private PictureBox panelAnimation;
        private Panel panelControls;
        private GroupBox groupBoxDrawing, groupBoxAnimation;

        private Button btnDrawRect, btnDrawEllipse, btnDrawLine, btnDrawText, btnClear;
        private Button btnStart, btnStop, btnAddShape;

        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;

        private Timer animationTimer;
        private ColorDialog colorDialog;
        private FontDialog fontDialog;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
    }
}
