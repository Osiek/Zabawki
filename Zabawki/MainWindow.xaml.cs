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
using LiveCharts;
using LiveCharts.Wpf;

namespace Zabawki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeometryModel3D mGeometry;
        TranslateTransform3D MyTranslateTransform;
        private List<_3DCoordinates> myAnimationCoords;
        private RotateTransform3D myRotateTransformX;
        private RotateTransform3D myRotateTransformY;
        private AxisAngleRotation3D myRotX;
        private AxisAngleRotation3D myRotY;
        Transform3DGroup cameraRotationsGroup;
        AnimationClock clock;
        private Storyboard sb;
        public SeriesCollection SeriesCollection { get; set; }
        ChartValues<double> cvX;
        ChartValues<double> cvY;
        ChartValues<double> cvZ;
        double[] valuesX;
        double[] valuesY;
        double[] valuesZ;
        int licznik_punktow_animacji;
        int ARRAY_SIZE = 50;

        public MainWindow()
        {
            
            NameScope.SetNameScope(this, new NameScope());
            InitializeComponent();
            sb = new Storyboard();
            cameraRotationsGroup = new Transform3DGroup();

            cvX = new ChartValues<double>();
            cvY = new ChartValues<double>();
            cvZ = new ChartValues<double>();

            valuesX = new double[ARRAY_SIZE];
            valuesY = new double[ARRAY_SIZE];
            valuesZ = new double[ARRAY_SIZE];

            myRotX = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 10);
            myRotateTransformX = new RotateTransform3D(myRotX);
            myRotY = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 10);
            myRotateTransformY = new RotateTransform3D(myRotY);

            cameraRotationsGroup.Children.Add(myRotateTransformX);
            cameraRotationsGroup.Children.Add(myRotateTransformY);
            camMain.Transform = cameraRotationsGroup;

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "X",
                    Values = cvX
                },
                new LineSeries
                {
                    Title = "Y",
                    Values = cvY
                },
                new LineSeries
                {
                    Title = "Z",
                    Values = cvZ
                }
            };
            DataContext = this;
            licznik_punktow_animacji = 0;


            build3DThing();


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
            MyTranslateTransform = new TranslateTransform3D();
            MyTranslateTransform.Changed += showOffsetValues;
            this.RegisterName("myTranslateTransform", MyTranslateTransform);
            mGeometry.Transform = MyTranslateTransform;
            group.Children.Add(mGeometry);

            //Próba animacaj
            //DoubleAnimation x = new DoubleAnimation
            //{
            //    From = 0,
            //    To = 10,
            //    Duration = TimeSpan.FromSeconds(5)
            //};
            ////x.RepeatBehavior = RepeatBehavior.Forever;

            //DoubleAnimation y = new DoubleAnimation();
            //y.From = 10;
            //y.To = -15;
            //y.Duration = TimeSpan.FromSeconds(10);
            //y.RepeatBehavior = RepeatBehavior.Forever;

            //clock = x.CreateClock();
            //mGeometry.ApplyAnimationClock(TranslateTransform3D.OffsetXProperty, clock);

            //Storyboard.SetTargetName(x, "myTranslateTransform");
            //Storyboard.SetTargetProperty(x, new PropertyPath(TranslateTransform3D.OffsetXProperty));

            //Storyboard.SetTargetName(y, "myTranslateTransform");
            //Storyboard.SetTargetProperty(y, new PropertyPath(TranslateTransform3D.OffsetYProperty));

            sb.Name = "SamolotSB";
            this.RegisterName(sb.Name, sb);

            //var xDoubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
            //var yDoubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
            //var zDoubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();

            //myAnimationCoords = new List<_3DCoordinates>();
            //myAnimationCoords.Add(new _3DCoordinates(0, 0, 0));
            //myAnimationCoords.Add(new _3DCoordinates(3, 0, 0));
            //myAnimationCoords.Add(new _3DCoordinates(5, 5, 0));
            //myAnimationCoords.Add(new _3DCoordinates(10, 0, 0));

            //int czas = 0;
            //foreach (var coord in myAnimationCoords)
            //{
            //    var xkeyFrame = new LinearDoubleKeyFrame();
            //    var ykeyFrame = new LinearDoubleKeyFrame();
            //    var zkeyFrame = new LinearDoubleKeyFrame();

            //    xkeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(czas));
            //    xkeyFrame.Value = coord.x;

            //    ykeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(czas));
            //    ykeyFrame.Value = coord.y;

            //    zkeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(czas));
            //    zkeyFrame.Value = coord.z;

            //    xDoubleAnimationUsingKeyFrames.KeyFrames.Add(xkeyFrame);
            //    yDoubleAnimationUsingKeyFrames.KeyFrames.Add(ykeyFrame);
            //    zDoubleAnimationUsingKeyFrames.KeyFrames.Add(zkeyFrame);
            //    czas += 1;
            //}

            //Storyboard.SetTargetName(xDoubleAnimationUsingKeyFrames, "myTranslateTransform");
            //Storyboard.SetTargetProperty(xDoubleAnimationUsingKeyFrames, new PropertyPath(TranslateTransform3D.OffsetXProperty));
            //Storyboard.SetTargetName(yDoubleAnimationUsingKeyFrames, "myTranslateTransform");
            //Storyboard.SetTargetProperty(yDoubleAnimationUsingKeyFrames, new PropertyPath(TranslateTransform3D.OffsetYProperty));
            //Storyboard.SetTargetName(zDoubleAnimationUsingKeyFrames, "myTranslateTransform");
            //Storyboard.SetTargetProperty(zDoubleAnimationUsingKeyFrames, new PropertyPath(TranslateTransform3D.OffsetZProperty));
            //Storyboard.SetTargetProperty(x, new PropertyPath(TranslateTransform3D.OffsetXProperty));

            //sb.Children.Add(x);
            //sb.Children.Add(y);
            //sb.Children.Add(xDoubleAnimationUsingKeyFrames);
            //sb.Children.Add(yDoubleAnimationUsingKeyFrames);
            //sb.Children.Add(zDoubleAnimationUsingKeyFrames);
            //sb.Completed += sb_Completed;
            //sb.Begin(this, true);
        }

        void sb_Completed(object sender, EventArgs e)
        {
            Console.WriteLine("Storyboard completed.\nLiczba punktow: {0}", licznik_punktow_animacji);

            for(int i = licznik_punktow_animacji - 1; i < ARRAY_SIZE; i++)
            {
                valuesX[i] = 0;
                valuesY[i] = 0;
                valuesZ[i] = 0;
            }

            cvX.AddRange(valuesX);
            cvY.AddRange(valuesY);
            cvZ.AddRange(valuesZ);
        }

        void showOffsetValues(object sender, EventArgs e)
        {
            //Console.WriteLine("X: {0}, Y: {1}, Z:{2}", MyTranslateTransform.OffsetX, MyTranslateTransform.OffsetY, MyTranslateTransform.OffsetZ);
            //SeriesCollection[0].Values.Add(MyTranslateTransform.OffsetX);
            //SeriesCollection[1].Values.Add(MyTranslateTransform.OffsetY);
            //SeriesCollection[2].Values.Add(MyTranslateTransform.OffsetZ);

            if (licznik_punktow_animacji > ARRAY_SIZE - 1)
            {
                cvX.AddRange(valuesX);
                cvY.AddRange(valuesY);
                cvZ.AddRange(valuesZ);

                //SeriesCollection[0].Values.Add(cvX);
                //SeriesCollection[1].Values.Add(cvY);
                //SeriesCollection[2].Values.Add(cvZ);

                licznik_punktow_animacji = 0;
                //cvX.Clear();
                //cvY.Clear();
                //cvZ.Clear();
                //SeriesCollection[0].Values.AddRange(((IEnumerable<T>)valuesY).GetEnumerator());
                //SeriesCollection[0].Values.AddRange(valuesX[]);

            }

            valuesX[licznik_punktow_animacji] = MyTranslateTransform.OffsetX;
            valuesY[licznik_punktow_animacji] = MyTranslateTransform.OffsetY;
            valuesZ[licznik_punktow_animacji] = MyTranslateTransform.OffsetZ;

            licznik_punktow_animacji += 1;
        }

        private void storyboardAnimationFromFile()
        {
            licznik_punktow_animacji = 0;
            SeriesCollection[0].Values.Clear();
            SeriesCollection[1].Values.Clear();
            SeriesCollection[2].Values.Clear();

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
                czas += 1;
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
            Button btn = (Button)sender;
            Image lol  = (Image)btn.Template.FindName("playPouseButton", btn);
            //Image img = (Image)playPauseImage;
            switch (btn.Name)
            {
                case "pauseButton":
                    sb.Pause(this);
                    break;
                case "playButton":
                    sb.Resume(this);
                    break;
                case "stopButton":
                    sb.Pause(this);
                    sb.Seek(this, TimeSpan.FromMilliseconds(0), TimeSeekOrigin.BeginTime);
                    SeriesCollection[0].Values.Clear();
                    SeriesCollection[1].Values.Clear();
                    SeriesCollection[2].Values.Clear();
                    licznik_punktow_animacji = 0;
                    break;
            }
                
        }
    }
}
