using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zabawki.Model
{
    class _3DCoordinates
    {
        private int x { get; set; }
        private int y { get; set; }
        private int z { get; set; }
        private int rotX { get; set; }
        private int rotY { get; set; }
        private int rotZ { get; set; }
        private int rotAngle { get; set; }

        public _3DCoordinates(int x, int y, int z, int rotX, int rotY, int rotZ, int rotAngle) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.rotX = rotX;
            this.rotY = rotY;
            this.rotZ = rotZ;
            this.rotAngle = rotAngle;
        }

        public void lol()
        {
            x = 3;
        }

    }
}
