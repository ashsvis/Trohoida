using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trohoida
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            var R = 100f;
            var d = 80f;
            var offset = new SizeF(200f, 300f);
            var points0 = new List<PointF>();
            var points = new List<PointF>();
            var tof = 2f * Math.PI / 3f;
            for (var t = 0f; t < 3 * Math.PI + tof / 2f; t += 0.1f)
            {
                var ta = t - tof;
                // точки циклоиды
                var x0 = R * (ta - Math.Sin(ta));
                var y0 = R * (1f - Math.Cos(ta));
                var pt0 = new PointF((float)x0, -(float)y0);
                points0.Add(PointF.Add(pt0, offset));
                // точки трохоиды
                var x = R * ta - d * Math.Sin(ta);
                var y = R - d * Math.Cos(ta);
                var pt = new PointF((float)x, -(float)y);
                points.Add(PointF.Add(pt, offset));
            }
            // считаем границы графика (по циклоиде)
            var ymin = points0.Min(p => p.Y);
            var ymax = points0.Max(p => p.Y);
            var xmin = points0.Min(p => p.X);
            var xmax = points0.Max(p => p.X);
            var xzero = points0.First(p => Math.Abs(p.Y - ymax) < float.Epsilon).X;
            // рисуем ось X
            e.Graphics.DrawLine(Pens.Black, new PointF(xmin, ymax), new PointF(xmax, ymax));
            // рисуем ось Y
            e.Graphics.DrawLine(Pens.Black, new PointF(xzero, ymin), new PointF(xzero, ymax * 1.1f));
            // рисуем трохоиду
            if (points.Count > 1)
                e.Graphics.DrawLines(Pens.Red, points.ToArray());
        }
    }
}
