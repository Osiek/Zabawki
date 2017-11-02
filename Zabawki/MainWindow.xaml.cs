using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace Zabawki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeometryModel3D mGeometry;
        private bool mDown;
        private Point mLastPos;


        public MainWindow()
        {
            InitializeComponent();
            build3DThing();
        }

        private void loadMovementFile_Click(object sender, RoutedEventArgs e)
        {
            string filename;
            string initialDirectory = "H:\\";
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (Directory.Exists(initialDirectory))
            {
                fileDialog.InitialDirectory = initialDirectory;
            }
            else
            {
                fileDialog.InitialDirectory = "C:\\";
            } 
            
            Nullable<bool> result = fileDialog.ShowDialog();
            if (result == true)
            {
                filename = fileDialog.FileName;
            }

        }

        public void build3DThing()
        {

            MeshGeometry3D mesh = new MeshGeometry3D();

            //front face
            mesh.Positions.Add(new Point3D(0, 0, 0));
            mesh.Positions.Add(new Point3D(1, 0, 0));
            mesh.Positions.Add(new Point3D(0, 1, 0));
            mesh.Positions.Add(new Point3D(1, 1, 0));
            //back face
            mesh.Positions.Add(new Point3D(0, 0, -1));
            mesh.Positions.Add(new Point3D(1, 0, -1));
            mesh.Positions.Add(new Point3D(0, 1, -1));
            mesh.Positions.Add(new Point3D(1, 1, -1));

            //Front face
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(3);

            //back face
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(4);

            //Left face
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(4);

            //right face
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(7);

            //bottom
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(0);

            //top
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(7);

            mGeometry = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.YellowGreen));
            //mGeometry.Transform = new Transform3DGroup();
            var MyTranslateTransform = new TranslateTransform3D();
            this.RegisterName("myTranslateTransform", MyTranslateTransform);
            mGeometry.Transform = MyTranslateTransform;
            group.Children.Add(mGeometry);

            //Próba animacaj
            DoubleAnimation x = new DoubleAnimation();
            x.From = -10;
            x.To = 10;
            x.Duration = TimeSpan.FromSeconds(3);
            x.RepeatBehavior = RepeatBehavior.Forever;

            DoubleAnimation y = new DoubleAnimation();
            y.From = 10;
            y.To = -15;
            y.Duration = TimeSpan.FromSeconds(3);
            y.RepeatBehavior = RepeatBehavior.Forever;

            Storyboard.SetTargetName(x, "myTranslateTransform");
            Storyboard.SetTargetProperty(x, new PropertyPath(TranslateTransform3D.OffsetXProperty));

            Storyboard.SetTargetName(y, "myTranslateTransform");
            Storyboard.SetTargetProperty(y, new PropertyPath(TranslateTransform3D.OffsetYProperty));

            Storyboard sb = new Storyboard();
            sb.Children.Add(x);
            sb.Children.Add(y);
            sb.Completed += sb_Completed;
            sb.Begin(this);
        }

        void sb_Completed(object sender, EventArgs e)
        {
            Console.WriteLine("Completed.");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            camMain.Position = new Point3D(
                camMain.Position.X,
                camMain.Position.Y,
                camMain.Position.Z - e.Delta / 250D);
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            int klawisz = (int)e.Key;
            if (e.Key != Key.Up || e.Key != Key.Down || e.Key != Key.Left || e.Key != Key.Right) System.Console.WriteLine(e.Key.ToString());

            switch(e.Key)
            {
                case Key.Left:
                    camMain.Position = new Point3D(camMain.Position.X - 0.5, camMain.Position.Y, camMain.Position.Z);
                    break;
                case Key.Right:
                    camMain.Position = new Point3D(camMain.Position.X + 0.5, camMain.Position.Y, camMain.Position.Z);
                    break;
                case Key.Up:
                    camMain.Position = new Point3D(camMain.Position.X, camMain.Position.Y + 0.5, camMain.Position.Z);
                    break;
                case Key.Down:
                    camMain.Position = new Point3D(camMain.Position.X, camMain.Position.Y - 0.5, camMain.Position.Z);
                    break;
            }
        }
    }
}
