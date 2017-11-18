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
using Zabawki.Model;

namespace Zabawki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeometryModel3D mGeometry;
        private List<_3DCoordinates> myAnimationCoords;
        private RotateTransform3D myRotateTransformX;
        private RotateTransform3D myRotateTransformY;
        private AxisAngleRotation3D myRotX;
        private AxisAngleRotation3D myRotY;
        Transform3DGroup cameraRotationsGroup;
        AnimationClock clock;
        Storyboard sb;

        public MainWindow()
        {
            InitializeComponent();
            build3DThing();

            sb = new Storyboard();
            cameraRotationsGroup = new Transform3DGroup();



            myRotX = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 10);
            myRotateTransformX = new RotateTransform3D(myRotX);
            myRotY = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 10);
            myRotateTransformY = new RotateTransform3D(myRotY);
            cameraRotationsGroup.Children.Add(myRotateTransformX);
            cameraRotationsGroup.Children.Add(myRotateTransformY);

            camMain.Transform = cameraRotationsGroup;
        }

        private void loadMovementFile_Click(object sender, RoutedEventArgs e)
        {
            File file = new File();
            string path = file.getPathFromDialogWindow();
            file.open();
            file.read3DCoordinates();
            myAnimationCoords = file.get3DCoordinates();
            storyboardAnimationFromFile();
        }

        public void build3DThing()
        {
            var myPlane = new Airplane();
            mGeometry = myPlane.getPlane();
            //mGeometry = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.YellowGreen));
            //mGeometry.Transform = new Transform3DGroup();
            var MyTranslateTransform = new TranslateTransform3D();
            this.RegisterName("myTranslateTransform", MyTranslateTransform);
            mGeometry.Transform = MyTranslateTransform;
            group.Children.Add(mGeometry);

            //Próba animacaj
            DoubleAnimation x = new DoubleAnimation();
            x.From = 0;
            x.To = 10;
            x.Duration = TimeSpan.FromSeconds(5);
            //x.RepeatBehavior = RepeatBehavior.Forever;

            DoubleAnimation y = new DoubleAnimation();
            y.From = 10;
            y.To = -15;
            y.Duration = TimeSpan.FromSeconds(10);
            y.RepeatBehavior = RepeatBehavior.Forever;

            clock = x.CreateClock();
            //mGeometry.ApplyAnimationClock(TranslateTransform3D.OffsetXProperty, clock);

            //Storyboard.SetTargetName(x, "myTranslateTransform");
            //Storyboard.SetTargetProperty(x, new PropertyPath(TranslateTransform3D.OffsetXProperty));

            //Storyboard.SetTargetName(y, "myTranslateTransform");
            //Storyboard.SetTargetProperty(y, new PropertyPath(TranslateTransform3D.OffsetYProperty));

            sb = new Storyboard();

            var xDoubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
            var yDoubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
            var zDoubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();

            myAnimationCoords = new List<_3DCoordinates>();
            myAnimationCoords.Add(new _3DCoordinates(0, 0, 0));
            myAnimationCoords.Add(new _3DCoordinates(3, 0, 0));
            myAnimationCoords.Add(new _3DCoordinates(5, 5, 0));
            myAnimationCoords.Add(new _3DCoordinates(10, 0, 0));

            int czas = 0;
            foreach (var coord in myAnimationCoords)
            {
                var xkeyFrame = new LinearDoubleKeyFrame();
                var ykeyFrame = new LinearDoubleKeyFrame();
                var zkeyFrame = new LinearDoubleKeyFrame();

                xkeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(czas));
                xkeyFrame.Value = coord.x;

                ykeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(czas));
                ykeyFrame.Value = coord.y;

                zkeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(czas));
                zkeyFrame.Value = coord.z;

                xDoubleAnimationUsingKeyFrames.KeyFrames.Add(xkeyFrame);
                yDoubleAnimationUsingKeyFrames.KeyFrames.Add(ykeyFrame);
                zDoubleAnimationUsingKeyFrames.KeyFrames.Add(zkeyFrame);
                czas += 1;
            }

            Storyboard.SetTargetName(xDoubleAnimationUsingKeyFrames, "myTranslateTransform");
            Storyboard.SetTargetProperty(xDoubleAnimationUsingKeyFrames, new PropertyPath(TranslateTransform3D.OffsetXProperty));
            Storyboard.SetTargetName(yDoubleAnimationUsingKeyFrames, "myTranslateTransform");
            Storyboard.SetTargetProperty(yDoubleAnimationUsingKeyFrames, new PropertyPath(TranslateTransform3D.OffsetYProperty));
            Storyboard.SetTargetName(zDoubleAnimationUsingKeyFrames, "myTranslateTransform");
            Storyboard.SetTargetProperty(zDoubleAnimationUsingKeyFrames, new PropertyPath(TranslateTransform3D.OffsetZProperty));
            //Storyboard.SetTargetProperty(x, new PropertyPath(TranslateTransform3D.OffsetXProperty));

            //sb.Children.Add(x);
            //sb.Children.Add(y);

            sb.Children.Add(xDoubleAnimationUsingKeyFrames);
            sb.Children.Add(yDoubleAnimationUsingKeyFrames);
            sb.Children.Add(zDoubleAnimationUsingKeyFrames);
            sb.Completed += sb_Completed;
            sb.Begin(this, true);
        }

        void sb_Completed(object sender, EventArgs e)
        {
            Console.WriteLine("Storyboard completed.");
        }

        private void storyboardAnimationFromFile()
        {
            sb = new Storyboard();

            var xDoubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
            var yDoubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
            var zDoubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();

            int czas = 0;
            foreach (var coord in myAnimationCoords)
            {
                var xkeyFrame = new LinearDoubleKeyFrame();
                var ykeyFrame = new LinearDoubleKeyFrame();
                var zkeyFrame = new LinearDoubleKeyFrame();

                xkeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(czas));
                xkeyFrame.Value = coord.x;

                ykeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(czas));
                ykeyFrame.Value = coord.y;

                zkeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(czas));
                zkeyFrame.Value = coord.z;

                xDoubleAnimationUsingKeyFrames.KeyFrames.Add(xkeyFrame);
                yDoubleAnimationUsingKeyFrames.KeyFrames.Add(ykeyFrame);
                zDoubleAnimationUsingKeyFrames.KeyFrames.Add(zkeyFrame);
                czas += 2;
            }

            Storyboard.SetTargetName(xDoubleAnimationUsingKeyFrames, "myTranslateTransform");
            Storyboard.SetTargetProperty(xDoubleAnimationUsingKeyFrames, new PropertyPath(TranslateTransform3D.OffsetXProperty));
            Storyboard.SetTargetName(yDoubleAnimationUsingKeyFrames, "myTranslateTransform");
            Storyboard.SetTargetProperty(yDoubleAnimationUsingKeyFrames, new PropertyPath(TranslateTransform3D.OffsetYProperty));
            Storyboard.SetTargetName(zDoubleAnimationUsingKeyFrames, "myTranslateTransform");
            Storyboard.SetTargetProperty(zDoubleAnimationUsingKeyFrames, new PropertyPath(TranslateTransform3D.OffsetZProperty));

            sb.Children.Add(xDoubleAnimationUsingKeyFrames);
            sb.Children.Add(yDoubleAnimationUsingKeyFrames);
            sb.Children.Add(zDoubleAnimationUsingKeyFrames);
            sb.Completed += sb_Completed;
            sb.Begin(this, true);
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

            if(Keyboard.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        myRotX.Angle -= 10;
                        break;
                    case Key.Right:
                        myRotX.Angle += 10;
                        break;
                    case Key.Up:
                        myRotY.Angle += 10;
                        break;
                    case Key.Down:
                        myRotY.Angle -= 10;
                        break;
                }

                return;
            }

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

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            sb.Seek(TimeSpan.FromSeconds(0));
        }
    }
}
