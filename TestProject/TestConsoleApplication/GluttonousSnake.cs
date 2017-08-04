using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestConsoleApplication
{
    public partial class GluttonousSnake : Form
    {
        public GluttonousSnake()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);//双缓冲
            this.KeyUp += (s, e) => timer1.Start();
            this.timer1.Tick += (s, e) => this.Move(m_cd);
        }

        private int m_w = 11;                   //格子宽度
        private Size m_size = new Size(25, 23); //地图大小
        private char m_cd = 'R';                //移动方向
        private List<Point> m_lst_snake;        //蛇的点数
        private Point m_pt_new;                 //食物位置

        private void GluttonousSnake_Load(object sender, EventArgs e)
        {
            timer1.Interval = 500;
            m_lst_snake = new List<Point>();
            m_lst_snake.Add(new Point(0, 0));   //默认蛇一格大小 左上角
            this.NewPoint();                    //创建食物
        }

        private void GluttonousSnake_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left: this.Move('L'); break;
                case Keys.Up: this.Move('U'); break;
                case Keys.Right: this.Move('R'); break;
                case Keys.Down: this.Move('D'); break;
                default: return;
            }
            timer1.Stop();//按下按键停止timer 因为别人可能长按 和timer就冲突了
        }

        private void GluttonousSnake_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int y = 0; y < m_size.Height; y++)
            {
                for (int x = 0; x < m_size.Width; x++)
                {
                    //绘制空白的格子
                    g.FillRectangle(Brushes.Black, x * m_w, y * m_w, m_w - 1, m_w - 1);
                }
            }
            //绘制蛇所在的格子
            m_lst_snake.ForEach(p => g.FillRectangle(Brushes.White, p.X * m_w, p.Y * m_w, m_w - 1, m_w - 1));
            //绘制食物
            g.FillRectangle(Brushes.Red, m_pt_new.X * m_w, m_pt_new.Y * m_w, m_w - 1, m_w - 1);
        }

        private void Move(char d)
        {
            Point pHead = m_lst_snake[0];//当前蛇头 并且将作为新的蛇头
            switch (d)
            {
                case 'L'://如果当前蛇长度不为1 往反方向移动禁止
                    if (m_cd == 'R' && m_lst_snake.Count > 1) return;
                    pHead.Offset(-1, 0);
                    break;
                case 'U':
                    if (m_cd == 'D' && m_lst_snake.Count > 1) return;
                    pHead.Offset(0, -1);
                    break;
                case 'R':
                    if (m_cd == 'L' && m_lst_snake.Count > 1) return;
                    pHead.Offset(1, 0);
                    break;
                case 'D':
                    if (m_cd == 'U' && m_lst_snake.Count > 1) return;
                    pHead.Offset(0, 1);
                    break;
            }
            //判断移动位置后的蛇头 是否撞墙
            if (pHead.X < 0 || pHead.Y < 0 || pHead.X >= m_size.Width || pHead.Y >= m_size.Height)
                throw new Exception("Game Over");
            //判断移动位置后的舌头 是否撞自己
            if (m_lst_snake.Contains(pHead)) throw new Exception("Game Over");
            if (pHead == m_pt_new)
            {//如果新蛇头在食物上 吃掉食物 并创建新食物
                m_lst_snake.Insert(0, pHead);
                this.NewPoint();
            }
            else
            {//否者添加蛇头 并移除最后一个位置
                m_lst_snake.Insert(0, pHead);
                m_lst_snake.RemoveAt(m_lst_snake.Count - 1);
            }
            m_cd = d;
            this.Invalidate();
        }

        private void NewPoint()
        {//创建食物
            List<Point> lst_temp = new List<Point>();
            for (int y = 0; y < m_size.Height; y++)
            {
                for (int x = 0; x < m_size.Width; x++)
                {//遍历所以格子去除蛇所在的坐标
                    if (m_lst_snake.Contains(new Point(x, y))) continue;
                    lst_temp.Add(new Point(x, y));
                }
            }
            //在除去蛇以外的格子中随机出现一个食物
            if (lst_temp.Count == 0) throw new Exception("No Box");
            m_pt_new = lst_temp[new Random().Next(0, lst_temp.Count)];
        }
    }
}
