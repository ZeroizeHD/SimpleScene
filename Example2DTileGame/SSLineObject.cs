﻿
using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK.Input;
using SimpleScene;



namespace Example2DTileGame
{
    public class SSLineObject : SSObject
    {
        #region Variables
        float squareWidth = 4;

        Vector3[,] mapArray = new Vector3[50, 50]; // W x D map (X & Z)

        bool isGenerating = true;
        int x = 0;

        float vHeight1 = 0;
        float vHeight2 = 0;
        float vHeight3 = 0;
        float vHeight4 = 0;
        float currentHeight;

        int R = 25, G = 25, B = 25; // Default values for color

        struct LineData
        {
            public Vector3 Pos;
            public Color4 Color;

            public LineData(Vector3 pos, Color4 color)
            {
                this.Pos = pos;
                this.Color = color; 
            }
        }

        List<LineData> vectorList = new List<LineData>();
        
        // Default values of square - should never actually set anything to anything
        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(0, 0, 0);
        Vector3 p2 = new Vector3(0, 0, 0);
        Vector3 p3 = new Vector3(0, 0, 0);
        Vector3 middle = new Vector3(0, 0, 0);

        #endregion
        
        /// <summary>
        /// draw a 'wire - frame' of the map
        /// </summary>
        public void drawWireFrame()
        {
            GL.Begin(PrimitiveType.Lines);
            foreach (LineData v in vectorList)
            {
                
                GL.Color4(v.Color);
                GL.Vertex3(v.Pos);

            }
            GL.End();
        }

        /// <summary>
        /// Render line object
        /// </summary>
        public override void Render(ref SSRenderConfig renderConfig)
        {                
                base.Render(ref renderConfig);

                //!important!
                // mode setup
                SSShaderProgram.DeactivateAll(); // disable GLSL
                GL.Disable(EnableCap.Texture2D);                
                GL.Disable(EnableCap.Lighting);
                //!important!                
                drawWireFrame(); // Draw it               
        }

        /// <summary>
        /// Adds points into array-list
        /// </summary>
        public void AddToArray(Vector3 p0, Vector3 p1, 
            Vector3 p2, Vector3 p3, Vector3 Middle, Color4 lineColor)
        {
            #region Vector adding
            vectorList.Add(new LineData(p0, lineColor)); vectorList.Add(new LineData(p1, lineColor));
            vectorList.Add(new LineData(p0, lineColor)); vectorList.Add(new LineData(p2, lineColor));
            vectorList.Add(new LineData(p2, lineColor)); vectorList.Add(new LineData(p3, lineColor));
            vectorList.Add(new LineData(p3, lineColor)); vectorList.Add(new LineData(p1, lineColor));
            vectorList.Add(new LineData(p0, lineColor)); vectorList.Add(new LineData(Middle, lineColor));
            vectorList.Add(new LineData(p1, lineColor)); vectorList.Add(new LineData(Middle, lineColor));
            vectorList.Add(new LineData(p2, lineColor)); vectorList.Add(new LineData(Middle, lineColor));
            vectorList.Add(new LineData(p3, lineColor)); vectorList.Add(new LineData(Middle, lineColor));
            #endregion
        }

        /// <summary>
        /// Relax the map
        /// </summary>
        public void Smoothing()
        {
            // Get height, 
            // Average height,
            // Set height equal to average of points around 

         
        }

        public SSLineObject (Vector3 mapPos) : base()
        {
            Random rand = new Random();
            Console.WriteLine("Set points");


            for (int i = 0; i < mapArray.GetLength(0); i++)
            {
                for (int j = 0; j < mapArray.GetLength(1); j++)
                {
                    float Middle = squareWidth / 2; // Middle point of the square
                    float squareCX = i * squareWidth;
                    float squareCY = j * squareWidth;

                    // So I can control individual heights
                    vHeight1 = rand.Next(0, 5);
                    vHeight2 = rand.Next(0, 5);
                    vHeight3 = rand.Next(0, 5);
                    vHeight4 = rand.Next(0, 5);
                    // 'Peak'
                    currentHeight = rand.Next(0, 10);
                    Smoothing();
                    p0 = new Vector3(squareCX, vHeight1, squareCY);
                    p1 = new Vector3(squareCX + squareWidth, vHeight2, squareCY);
                    p2 = new Vector3(squareCX, vHeight3, squareCY + squareWidth);
                    p3 = new Vector3(squareCX + squareWidth, vHeight4, squareCY + squareWidth);

                    // Determines height
                    middle = new Vector3(squareCX + Middle, currentHeight, squareCY + Middle);
                    Color4 color;

                    #region Color switch
                    color = new Color4(0f, 0f, 0f, 1f);
                    float heightFactor = 0.1f * (float)currentHeight;
                    color.G += heightFactor;
                    color.B += heightFactor;
                    color.R += heightFactor;

                    if(heightFactor == 0)
                    {
                        color.G = 0.1f;
                        color.B = 0.1f;
                        color.R = 0.1f;
                    }
                    
                    #endregion

                    AddToArray(p0, p1, p2, p3, middle, color);


                }

            }
            
        }



    }

}

