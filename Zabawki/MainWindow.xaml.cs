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
            mGeometry.Transform = new Transform3DGroup();
            group.Children.Add(mGeometry);

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
            mDown = false;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            mDown = true;
            Point pos = Mouse.GetPosition(viewport3D1);
            mLastPos = new Point(pos.X - viewport3D1.ActualWidth / 2, viewport3D1.ActualHeight / 2 - pos.Y);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mDown) return;
            Point pos = Mouse.GetPosition(viewport3D1);
            Point actualPos = new Point(pos.X - viewport3D1.ActualWidth / 2,
            viewport3D1.ActualHeight / 2 - pos.Y);
            double dx = actualPos.X - mLastPos.X;
            double dy = actualPos.Y - mLastPos.Y;
            double mouseAngle = 0;

            if(dx != 0 && dy != 0)
            {
                mouseAngle = Math.Asin(Math.Abs(dy) / Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
                if (dx < 0 && dy > 0) mouseAngle += Math.PI / 2;
                else if (dx < 0 && dy < 0) mouseAngle += Math.PI;
                else if (dx > 0 && dy < 0) mouseAngle += Math.PI * 1.5;

            }
            else if(dx == 0 && dy != 0)
            {
                mouseAngle = Math.Sign(dy) > 0 ? Math.PI / 2 : Math.PI * 1.5;
            }
            else if(dx != 0 && dy == 0)
            {
                mouseAngle = Math.Sign(dx) > 0 ? 0 : Math.PI;
            }

            double axisAngle = mouseAngle + Math.PI / 2;

            Vector3D axis = new Vector3D(
                Math.Cos(axisAngle) * 4,
                Math.Sin(axisAngle) * 4, 0);

            double rotation = 0.02 *
                Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

            Transform3DGroup group = mGeometry.Transform as Transform3DGroup;
            QuaternionRotation3D r =
                new QuaternionRotation3D(
                    new Quaternion(axis, rotation * 180 / Math.PI));
            group.Children.Add(new RotateTransform3D(r));

            mLastPos = actualPos;
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            int klawisz = (int)e.Key;
            if (e.Key != Key.Up || e.Key != Key.Down || e.Key != Key.Left || e.Key != Key.Right) System.Console.WriteLine(e.Key.ToString());

            switch(e.Key)
            {
                case Key.Left:
                    camMain.Position = new Point3D(camMain.Position.X + 0.5, camMain.Position.Y, camMain.Position.Z);
                    break;
                case Key.Right:
                    camMain.Position = new Point3D(camMain.Position.X - 0.5, camMain.Position.Y, camMain.Position.Z);
                    break;
                case Key.Up:
                    camMain.Position = new Point3D(camMain.Position.X, camMain.Position.Y - 0.5, camMain.Position.Z);
                    break;
                case Key.Down:
                    camMain.Position = new Point3D(camMain.Position.X, camMain.Position.Y + 0.5, camMain.Position.Z);
                    break;
            }
        }
    }
}
