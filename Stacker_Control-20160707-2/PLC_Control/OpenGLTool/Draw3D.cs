using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL;
//using PLC_Control;

namespace OpenGLTool
{
    class Draw3D : GLDrawObject
    {
        public static void Draw3DInformation(SharpGL.OpenGL gl_object, float[] X, float[] Y, float[] Z)
        {
            double MaxDistance = 0;
            double r = 0, g = 0, b = 0;
            gl_object.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl_object.LoadIdentity();
            gl_object.Translate(_LX, _LY, _LZ);
            gl_object.Rotate(_RoX, 0.0, 1.0, 0.0);
            gl_object.Rotate(_RoY, 1.0, 0.0, 0.0);
            gl_object.Rotate(_RoZ, 0.0, 0.0, 1.0);

            //畫光達自己的位置
            gl_object.Begin(OpenGL.GL_TRIANGLES);
            gl_object.Color(1.0, 1.0, 1.0);
            gl_object.Vertex(-0.2, -0.15, 0);
            gl_object.Vertex(0, 0.2, 0);
            gl_object.Vertex(0.2, -0.15, 0);
            gl_object.End();

            drawAxis_3D(gl_object);

            //畫D場景
            gl_object.Begin(OpenGL.GL_POINTS);
            for (int i = 0; i < 23040; i++)
            {
                if (X[i] != 0 && Y[i] != 0 && Z[i] != 0)
                {
                    //用XY距離計算顯示顏色
                    double XYDistance = Math.Sqrt(X[i] * X[i] + Y[i] * Y[i]);
                    if (XYDistance > MaxDistance) MaxDistance = XYDistance;
                    if (XYDistance > 10) XYDistance = 10;
                    XYDistance = XYDistance / 10 * (360 - 30) + 30;
                    HsvToRgb(XYDistance, 1, 1, out r, out g, out b);
                    gl_object.Color(r, g, b);
                    //畫上點座標
                    gl_object.Vertex(X[i], Y[i], Z[i]);
                }
            }
            gl_object.End();
            gl_object.Flush();
        }

        public static void drawAxis_3D(SharpGL.OpenGL gl_object)
        {
            gl_object.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl_object.LoadIdentity();
            gl_object.Translate(_LX, _LY, _LZ);
            gl_object.Rotate(_RoX, 0.0, 1.0, 0.0);
            gl_object.Rotate(_RoY, 1.0, 0.0, 0.0);
            gl_object.Rotate(_RoZ, 0.0, 0.0, 1.0);

            //畫Z軸
            gl_object.Color(1.0, 1.0, 1.0);
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, -0.02, -0.02);
            gl_object.Vertex(-0.02, -0.02, 10);
            gl_object.Vertex(0.02, -0.02, -0.02);
            gl_object.Vertex(0.02, -0.02, 10);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, 10);
            gl_object.Vertex(-0.02, 0.02, -0.02);
            gl_object.Vertex(-0.02, 0.02, 10);
            gl_object.Vertex(-0.02, -0.02, -0.02);
            gl_object.Vertex(-0.02, -0.02, 10);
            gl_object.End();
            //up
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, -0.02, 10);
            gl_object.Vertex(0.02, -0.02, 10);
            gl_object.Vertex(-0.02, 0.02, 10);
            gl_object.Vertex(0.02, 0.02, 10);
            gl_object.End();

            //down
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, -0.02, -0.02);
            gl_object.Vertex(0.02, -0.02, -0.02);
            gl_object.Vertex(-0.02, 0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.End();

            //畫X軸
            gl_object.Color(0, 1.0, 0);
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(0.02, -0.02, -0.02);
            gl_object.Vertex(10, -0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.Vertex(10, 0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, 0.02);
            gl_object.Vertex(10, 0.02, 0.02);
            gl_object.Vertex(0.02, -0.02, 0.02);
            gl_object.Vertex(10, -0.02, 0.02);
            gl_object.Vertex(0.02, -0.02, -0.02);
            gl_object.Vertex(10, -0.02, -0.02);
            gl_object.End();
            //up
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(10, -0.02, -0.02);
            gl_object.Vertex(10, 0.02, -0.02);
            gl_object.Vertex(10, -0.02, 0.02);
            gl_object.Vertex(10, 0.02, 0.02);
            gl_object.End();

            //down
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(0.02, -0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.Vertex(0.02, -0.02, 0.02);
            gl_object.Vertex(0.02, 0.02, 0.02);
            gl_object.End();

            //畫Y軸
            gl_object.Color(0, 0, 1.0);
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, 0.02, -0.02);
            gl_object.Vertex(-0.02, 10, -0.02);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.Vertex(0.02, 10, -0.02);
            gl_object.Vertex(0.02, 0.02, 0.02);
            gl_object.Vertex(0.02, 10, 0.02);
            gl_object.Vertex(-0.02, 0.02, 0.02);
            gl_object.Vertex(-0.02, 10, 0.02);
            gl_object.Vertex(-0.02, 0.02, -0.02);
            gl_object.Vertex(-0.02, 10, -0.02);
            gl_object.End();
            //up
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, 10, -0.02);
            gl_object.Vertex(0.02, 10, -0.02);
            gl_object.Vertex(-0.02, 10, 0.02);
            gl_object.Vertex(0.02, 10, 0.02);
            gl_object.End();

            //down
            gl_object.Begin(OpenGL.GL_TRIANGLE_STRIP);
            gl_object.Vertex(-0.02, 0.02, -0.02);
            gl_object.Vertex(0.02, 0.02, -0.02);
            gl_object.Vertex(-0.02, 0.02, 0.02);
            gl_object.Vertex(0.02, 0.02, 0.02);
            gl_object.End();

            //gl_object.Flush();
        }
    }
}
