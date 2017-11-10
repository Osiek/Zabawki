using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Zabawki.Model
{
    class Airplane
    {
        MeshGeometry3D mesh;
        GeometryModel3D airplaneModel;

        public Airplane()
        {
            mesh = new MeshGeometry3D();
            makeAirplane();
        }

        public GeometryModel3D getPlane()
        {
            return airplaneModel;
        }

        private void makeAirplane()
        {
            definePoints();
            makeMesh();
            airplaneModel = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.YellowGreen));
        }

        private void definePoints()
        {
            //Lewa sciana samolotu
            mesh.Positions.Add(new Point3D(0,0,0)); //pkt. 1
            mesh.Positions.Add(new Point3D(5,0,0)); //pkt. 2
            mesh.Positions.Add(new Point3D(1,1,0)); //pkt. 3
            mesh.Positions.Add(new Point3D(5,1,0)); //pkt. 4
            mesh.Positions.Add(new Point3D(4.5,1,0)); //pkt. 5
            mesh.Positions.Add(new Point3D(5,1.5,0)); //pkt. 6

            //Prawa sciana samolotu
            mesh.Positions.Add(new Point3D(0, 0, 1)); //pkt. 1 tył - 7
            mesh.Positions.Add(new Point3D(5, 0, 1)); //pkt. 2 tył - 8
            mesh.Positions.Add(new Point3D(1, 1, 1)); //pkt. 3 tył - 9
            mesh.Positions.Add(new Point3D(5, 1, 1)); //pkt. 4 tył - 10
            mesh.Positions.Add(new Point3D(4.5, 1, 1)); //pkt. 5 tył - 11
            mesh.Positions.Add(new Point3D(5, 1.5, 1)); //pkt. 6 tył - 12

            //Lewe skrzydlo
            mesh.Positions.Add(new Point3D(3, 0, 0)); //pkt. 13
            mesh.Positions.Add(new Point3D(4, 0, 0)); //pkt. 14
            mesh.Positions.Add(new Point3D(3, 0, -4)); //pkt. 15
            mesh.Positions.Add(new Point3D(4, 0, -4)); //pkt. 16

            //Prawe skrzydlo
            mesh.Positions.Add(new Point3D(3, 0, 1)); //pkt. 17
            mesh.Positions.Add(new Point3D(4, 0, 1)); //pkt. 18
            mesh.Positions.Add(new Point3D(3, 0, 5)); //pkt. 19
            mesh.Positions.Add(new Point3D(4, 0, 5)); //pkt. 20
        }

        private void makeMesh()
        {
            //Lewa strona samolotu
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(3);

            //Prawa strona samolotu
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(8);
            mesh.TriangleIndices.Add(9);
            mesh.TriangleIndices.Add(9);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(6);

            //Dol
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(0);

            //Gora
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(9);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(9);
            mesh.TriangleIndices.Add(8);

            //Przod
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(8);
            mesh.TriangleIndices.Add(8);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(0);

            //Tyl
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(9);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(1);

            //Ogon
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(10);
            mesh.TriangleIndices.Add(11);
            mesh.TriangleIndices.Add(9);

            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(9);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(9);
            mesh.TriangleIndices.Add(11);

            //mesh.TriangleIndices.Add(4);
            //mesh.TriangleIndices.Add(11);
            //mesh.TriangleIndices.Add(10);
            //mesh.TriangleIndices.Add(4);
            //mesh.TriangleIndices.Add(5);
            //mesh.TriangleIndices.Add(11);
        }
    }
}
