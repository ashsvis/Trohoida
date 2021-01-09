﻿using System;
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
        float R, d, ymin, ymax, xmin, xmax, xzero;
        SizeF offset;
        PointF[] points0, points;

        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// Первая загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            R = 100f;
            d = 80f;
            offset = new SizeF(200f, 300f);
            points0 = BuildPoints(R, R).Select(p => PointF.Add(p, offset)).ToArray();
            points = BuildPoints(R, d).Select(p => PointF.Add(p, offset)).ToArray();
            // считаем границы графика (по циклоиде)
            ymin = points0.Min(p => p.Y);
            ymax = points0.Max(p => p.Y);
            xmin = points0.Min(p => p.X);
            xmax = points0.Max(p => p.X);
            xzero = points0.First(p => Math.Abs(p.Y - ymax) < float.Epsilon).X;
        }

        /// <summary>
        /// Построение точек циклоиды в отдельном методе
        /// </summary>
        /// <param name="R">Радиус окружности</param>
        /// <param name="d">Смещение наблюдаемой точки от центра окружности</param>
        /// <returns>Возвращает перечисление получившихся точек</returns>
        private static IEnumerable<PointF> BuildPoints(float R, float d)
        {
            var points = new List<PointF>();
            var tof = 2f * Math.PI / 3f;
            for (var t = 0f; t < 3 * Math.PI + tof / 2f; t += 0.1f)
            {
                var ta = t - tof;
                // точки трохоиды
                var x = R * ta - d * Math.Sin(ta);
                var y = R - d * Math.Cos(ta);
                var pt = new PointF((float)x, -(float)y);
                points.Add(pt);
            }
            return points;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            // рисуем ось X
            e.Graphics.DrawLine(Pens.Black, new PointF(xmin, ymax), new PointF(xmax, ymax));
            // рисуем ось Y
            e.Graphics.DrawLine(Pens.Black, new PointF(xzero, ymin), new PointF(xzero, ymax * 1.1f));
            // рисуем трохоиду
            if (points.Count() > 1)
                e.Graphics.DrawLines(Pens.Red, points.ToArray());
        }
    }
}
