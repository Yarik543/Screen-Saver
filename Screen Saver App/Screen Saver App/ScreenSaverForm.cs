using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Screen_Saver_App
{
    public partial class ScreenSaverForm : Form
    {

        class Snowflake
        {
            public float X;
            public float Y;
            public float Speed;
            public float Size;
            public Image Image;
        }

        private List<Snowflake> snowflakes = new List<Snowflake>();
        private Timer timer = new Timer();
        private Random rand = new Random();
        private Image background;
        private Image[] snowImages;

        public ScreenSaverForm()
        {
            InitializeComponent();
            // Настройки окна
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.KeyDown += (s, e) => this.Close();
            this.MouseDown += (s, e) => this.Close();


            // Загружаем снежинки (несколько png с прозрачностью)
            snowImages = new Image[]
            {
                Image.FromFile(@"C:\C#\Коноплев 4 курс\Screen saver\Snow1.png"),
                Image.FromFile(@"C:\C#\Коноплев 4 курс\Screen saver\Snow2.png")
            };

            // Создаём снежинки
            for (int i = 0; i < 120; i++)
            {
                snowflakes.Add(CreateSnowflake());
            }

            // Таймер
            timer.Interval = 30;
            timer.Tick += timer1_Tick;
            timer.Start();
        }

        private Snowflake CreateSnowflake()
        {
            float size = rand.Next(10, 35);
            return new Snowflake
            {
                X = rand.Next(0, Screen.PrimaryScreen.Bounds.Width),
                Y = rand.Next(-Screen.PrimaryScreen.Bounds.Height, 0),
                Speed = (float)(rand.NextDouble() * 2 + size / 15f), // крупные падают быстрее
                Size = size,
                Image = snowImages[rand.Next(snowImages.Length)]
            };
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var flake in snowflakes)
            {
                flake.Y += flake.Speed;
                if (flake.Y > Screen.PrimaryScreen.Bounds.Height)
                {
                    // Перезапускаем сверху
                    flake.Y = -flake.Size;
                    flake.X = rand.Next(0, Screen.PrimaryScreen.Bounds.Width);
                }
            }
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;


            // Рисуем снежинки
            foreach (var flake in snowflakes)
            {
                g.DrawImage(flake.Image, flake.X, flake.Y, flake.Size, flake.Size);
            }
        }
    }
}
