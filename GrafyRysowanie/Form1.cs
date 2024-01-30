using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GrafyRysowanie
{
    public partial class Form1 : Form
    {
        private bool[] visited;
        private int startVertex;
        private int endVertex;
        private Pen start;
        private Pen end;

        private const int r = 10;
        private Graphics g;
        private Pen pWierzcholek;
        private Pen pWierzcholekAktywny;
        private Pen pKrawedz;
        private Wierzchlek MouseDownWierzcholek;

        private List<Wierzchlek> wierzcholki = new List<Wierzchlek>();

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Image = new Bitmap(500, 500);

            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            pWierzcholek = new Pen(Color.Orange);
            pWierzcholek.Width = 3;
            pWierzcholekAktywny = new Pen(Color.Red);
            pWierzcholekAktywny.Width = 3;

            start = new Pen(Color.Green);
            start.Width = 3;

            end = new Pen(Color.Red);
            end.Width = 3;

            pKrawedz = new Pen(Color.Blue);
            pKrawedz.Width = 10;
            //pKrawedz.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            pKrawedz.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            checkedListBoxAlgorithm.SetItemChecked(0, true);
            checkedListBoxDirection.SetItemChecked(0, true);
        }




        private void odrysujGraf()
        {
            g.Clear(Color.White);
            foreach (Wierzchlek w in wierzcholki)
            {
                foreach (Wierzchlek wn in w.Nastpniki)
                {
                    g.DrawLine(pKrawedz, w.Polozenie, wn.Polozenie);
                }
            }

            foreach (Wierzchlek w in wierzcholki)
            {

                if (w.Id == startVertex)
                {
                    g.DrawEllipse(start, w.Polozenie.X - r, w.Polozenie.Y - r, 2 * r, 2 * r);
                }
                else
                {
                    g.DrawEllipse(pWierzcholek, w.Polozenie.X - r, w.Polozenie.Y - r, 2 * r, 2 * r);

                }
                g.DrawString(w.Id.ToString(),
                             new System.Drawing.Font("Microsoft Sans Serif", r),
                             new SolidBrush(Color.Red),
                             w.Polozenie.X + r,
                             w.Polozenie.Y + r);
                if (w == MouseDownWierzcholek)
                {
                    g.DrawEllipse(pWierzcholekAktywny, w.Polozenie.X - r, w.Polozenie.Y - r, 2 * r, 2 * r);
                }
            }
            pictureBox1.Refresh();
        }

        private void SetStartVertex(int vertexId)
        {
            startVertex = vertexId;
            Array.Clear(visited, 0, visited.Length);
            odrysujGraf();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && MouseDownWierzcholek != null)
            {
                odrysujGraf();
                g.DrawLine(pKrawedz, MouseDownWierzcholek.Polozenie, e.Location);
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && MouseDownWierzcholek != null)
            {
                foreach (Wierzchlek w in wierzcholki)
                {
                    if (w.Odleglosc(e.Location) < r)
                    {
                        if(checkedListBoxDirection.GetItemCheckState(1) == CheckState.Checked)
                        {
                            pKrawedz.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                            MouseDownWierzcholek.Nastpniki.Add(w);
                            

                        }
                        else
                        {
                            pKrawedz.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                            MouseDownWierzcholek.Nastpniki.Add(w);
                            w.Nastpniki.Add(MouseDownWierzcholek);
                        }
                    }
                }
                MouseDownWierzcholek = null;
                odrysujGraf();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                wierzcholki.Add(new Wierzchlek(e.Location));
                odrysujGraf();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownWierzcholek = null;
                foreach (Wierzchlek w in wierzcholki)
                {
                    if (w.Odleglosc(e.Location) < r)
                    {
                        MouseDownWierzcholek = w;
                    }
                }
                odrysujGraf();
            }
            else if (e.Button == MouseButtons.Right)
            {
                visited = new bool[wierzcholki.Count + 1];
                startVertex = -1;
                endVertex = -1;

                foreach (Wierzchlek w in wierzcholki)
                {
                    if (w.Odleglosc(e.Location) < r)
                    {
                        g.DrawEllipse(start, w.Polozenie.X - r, w.Polozenie.Y - r, 2 * r, 2 * r);

                        SetStartVertex(w.Id);
                    }
                }
            }
        }
        private void AppendToTextBox(string message)
        {
            textBox1.AppendText(message + Environment.NewLine);
        }
        private void checkedListBoxAlgorithm_ItemCheck(object sender, ItemCheckEventArgs e)
        {

            for (int i = 0; i < checkedListBoxAlgorithm.Items.Count; ++i)
            {

                if (i != e.Index)
                {
                    checkedListBoxAlgorithm.SetItemChecked(i, false);
                }
            }

        }
        private void DFS(int vertexId)
        {
            visited[vertexId - 1] = true;
            //AppendToTextBox($"start: {vertexId}");
            

            foreach (Wierzchlek neighbor in wierzcholki[vertexId - 1].Nastpniki)
            {
                if (!visited[neighbor.Id - 1])
                {
                    AppendToTextBox($"z {vertexId} do {neighbor.Id}");
                    DFS(neighbor.Id);
                }
            }

        }
        

        private void BFS(int startVertex)
        {
            Queue<Wierzchlek> queue = new Queue<Wierzchlek>();
            bool[] visited = new bool[wierzcholki.Count];

            queue.Enqueue(wierzcholki[startVertex - 1]);
            visited[startVertex - 1] = true;
            int x = 1;
            //AppendToTextBox($"level: {x}");
            AppendToTextBox($"{x}){startVertex}");


            while (queue.Count > 0)
            {
                //AppendToTextBox($"level {++x}");
                Wierzchlek currentVertex = queue.Dequeue();

                foreach (Wierzchlek neighbor in currentVertex.Nastpniki)
                {

                    if (!visited[neighbor.Id - 1])
                    {

                        queue.Enqueue(neighbor);
                        visited[neighbor.Id - 1] = true;
                        AppendToTextBox($"{++x}) visited: {neighbor.Id}");
                    }
                }

            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            startVertex = -1;
            wierzcholki.Clear();
            Wierzchlek.ResetGraf();
            textBox1.Clear();
            odrysujGraf();
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {

            textBox1.Clear();
            if (wierzcholki.Count < 2)
            {
                MessageBox.Show("Za mało wierzchołków");
            }
            else if (startVertex <= 0)
            {
                MessageBox.Show("Nie wybrano wierzchołka początkowego");
            }
            else if(checkedListBoxAlgorithm.GetItemCheckState(1) == CheckState.Checked)
            {
                AppendToTextBox($"Algorithm BFS.");
                BFS(startVertex);
                MessageBox.Show("Gotowe");
            }
            else
            {
                visited = new bool[wierzcholki.Count];
                AppendToTextBox($"Algorithm DFS.");
                DFS(startVertex);
                MessageBox.Show("Gotowe");
            }
            
        }

        private void checkedListBoxDirection_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < checkedListBoxDirection.Items.Count; ++i)
            {

                if (i != e.Index)
                {
                    checkedListBoxDirection.SetItemChecked(i, false);
                }
            }
        }
    }
}
