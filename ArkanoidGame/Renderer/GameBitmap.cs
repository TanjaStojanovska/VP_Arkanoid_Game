﻿using ArkanoidGame.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ArkanoidGame.Renderer
{
    public class GameBitmapKey
    {
        private Vector2D positionUR;
        private Vector2D positionUL;
        private Vector2D positionDL;

        private long uniqueKey;

        public GameBitmapKey(long uniqueName, Vector2D positionUL, Vector2D positionUR, Vector2D positionDL)
        {
            this.positionUL = positionUL;
            this.positionUR = positionUR;
            this.positionDL = positionDL;

            this.uniqueKey = uniqueName;
        }

        public override int GetHashCode()
        {
            Vector2D HU = positionUR - positionUL; //горна хоризонтала
            Vector2D VL = positionDL - positionUL; //лева вертикала
            Vector2D HD = (positionDL + (positionUR - positionUL)) - positionDL; //долна хоризонатала
            int temp = (uniqueKey.GetHashCode() + HU.GetHashCode()) % int.MaxValue;
            temp = (temp + VL.GetHashCode()) % int.MaxValue;
            temp = (temp + HD.GetHashCode()) % int.MaxValue;
            return temp;
        }
    }

    public class GameBitmap
    {
        private static readonly object lockObject;
        private static long IDCounter;

        static GameBitmap()
        {
            lockObject = new object();
            IDCounter = long.MinValue;
        }

        /// <summary>
        /// Дали сликата да се прикаже како квадрат на било која резолуција
        /// </summary>
        public bool IsSquare { get; set; }

        /// <summary>
        /// Ширина на сликата во единици од играта
        /// </summary>
        public double WidthInGameUnits { get; set; }

        /// <summary>
        /// Висина на сликата во единици од играта
        /// </summary>
        public double HeightInGameUnits { get; set; }

        /// <summary>
        /// Позиција на горниот лев агол од сликата во единици од играта
        /// </summary>
        public Vector2D PositionUL { get; set; }

        /// <summary>
        /// Позиција на горниот десен агол од сликата
        /// </summary>
        public Vector2D PositionUR { get; set; }

        /// <summary>
        /// Позиција на долниот лев агол од сликата
        /// </summary>
        public Vector2D PositionDL { get; set; }

        /// <summary>
        /// Позиција на долниот десен агол од сликата
        /// </summary>
        public Vector2D PositionDR
        {
            get
            {
                return GetPositionVectorDR();
            }
        }

        /// <summary>
        /// Позиција на долниот десен агол од сликата
        /// </summary>
        /// <returns></returns>
        private Vector2D GetPositionVectorDR()
        {
            return (PositionDL + (PositionUR - PositionUL));
        }

        /// <summary>
        /// Клуч за hash и tree мапа.
        /// </summary>
        /// <returns></returns>
        public GameBitmapKey GetKey()
        {
            return new GameBitmapKey(UniqueKey, PositionUR, PositionUL, PositionDL);
        }

        /// <summary>
        /// Дефинира со кваква боја да се исцрта објектот на слаб хардвер.
        /// </summary>
        public Color ColorLowSpec { get; set; }

        /// <summary>
        /// UniqueKey е клуч со кој што сликата се
        /// зачувува во меморија (со класата RendererCache).
        /// Истата може да се добие со повик на методот
        /// RendererCache.GetBitmapFromMainMemory(long uniqueKey);
        /// </summary>
        public long UniqueKey { get; private set; }

        /// <summary>
        /// Дефинира дали оваа слика да се исцртува на послаб хардвер. Предефинираната 
        /// вредност е false.
        /// </summary>
        public bool DrawLowSpec { get; set; }

        /// <summary>
        /// Дефинира дали на слаб хардвер наместо оваа слика треба да се прикаже геометриска
        /// фигура што ќе личи на топка.
        /// </summary>
        public bool IsBall { get; set; }

        public GameBitmap(string relativePath, double x, double y, double widthInGameUnits,
            double heightInGameUnits)
        {
            long uniqueKey = 0;
            lock (lockObject)
            {
                uniqueKey = IDCounter++;
            }

            RendererCache.LoadBitmapIntoMainMemory(relativePath, uniqueKey);
            this.WidthInGameUnits = widthInGameUnits;
            this.HeightInGameUnits = heightInGameUnits;
            this.PositionUL = new Vector2D(x, y);
            this.PositionUR = this.PositionUL + new Vector2D(widthInGameUnits, 0);
            this.PositionDL = this.PositionUL + new Vector2D(0, heightInGameUnits);
            this.UniqueKey = uniqueKey;
            this.DrawLowSpec = false;
        }

        private void SetDimensions()
        {
            this.WidthInGameUnits = (this.PositionUR - this.PositionUL).Magnitude();
            this.HeightInGameUnits = (this.PositionDL - this.PositionUL).Magnitude();
        }

        public GameBitmap(string relativePath, Vector2D positionUL, Vector2D positionUR, Vector2D positionDL)
        {
            long uniqueKey = 0;
            lock (lockObject)
            {
                uniqueKey = IDCounter++;
            }
            RendererCache.LoadBitmapIntoMainMemory(relativePath, uniqueKey);
            this.PositionUL = positionUL;
            this.PositionUR = positionUR;
            this.PositionDL = positionDL;
            this.UniqueKey = uniqueKey;
            this.SetDimensions();
            this.DrawLowSpec = false;
        }

        public GameBitmap(Bitmap bitmap, double x,
            double y, double widthInGameUnits, double heightInGameUnits)
        {

            long uniqueKey = 0;
            lock (lockObject)
            {
                uniqueKey = IDCounter++;
            }
            RendererCache.SaveBitmap(uniqueKey, bitmap);
            this.WidthInGameUnits = widthInGameUnits;
            this.HeightInGameUnits = heightInGameUnits;
            this.PositionUL = new Vector2D(x, y);
            this.PositionUR = this.PositionUL + new Vector2D(widthInGameUnits, 0);
            this.PositionDL = this.PositionUL + new Vector2D(0, heightInGameUnits);
            this.UniqueKey = uniqueKey;
            this.DrawLowSpec = false;
        }

        public GameBitmap(Bitmap bitmap, Vector2D positionUL, Vector2D positionUR, Vector2D positionDL)
        {
            long uniqueKey = 0;
            lock (lockObject)
            {
                uniqueKey = IDCounter++;
            }
            RendererCache.SaveBitmap(uniqueKey, bitmap);
            this.UniqueKey = uniqueKey;
            this.PositionUL = positionUL;
            this.PositionUR = positionUR;
            this.PositionDL = positionDL;
            this.SetDimensions();
            this.UniqueKey = uniqueKey;
            this.DrawLowSpec = false;
        }

        public GameBitmap(long UniqueKey, double x,
            double y, double widthInGameUnits, double heightInGameUnits)
        {
            this.WidthInGameUnits = widthInGameUnits;
            this.HeightInGameUnits = heightInGameUnits;
            this.PositionUL = new Vector2D(x, y);
            this.PositionUR = this.PositionUL + new Vector2D(widthInGameUnits, 0);
            this.PositionDL = this.PositionUL + new Vector2D(0, heightInGameUnits);
            this.UniqueKey = UniqueKey;
            this.DrawLowSpec = false;
        }

        public GameBitmap(long uniqueKey, Vector2D positionUL, Vector2D positionUR, Vector2D positionDL)
        {
            this.UniqueKey = uniqueKey;
            this.PositionUL = positionUL;
            this.PositionUR = positionUR;
            this.PositionDL = positionDL;
            this.SetDimensions();
            this.UniqueKey = uniqueKey;            
            this.DrawLowSpec = false;
        }
    }
}
