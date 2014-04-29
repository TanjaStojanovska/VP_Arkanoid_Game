﻿using ArkanoidGame.Geometry;
using ArkanoidGame.Interfaces;
using ArkanoidGame.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkanoidGame.Objects
{
    public class SmallPurpleBrick : AbstractBrick
    {
       
        public override void OnUpdate(long gameElapsedTime)
        {
            

            Position += (Velocity) / 2;

            if (Position.X > GameWidth - 10 - ObjectWidth)
                Position.X = GameWidth - 10 - ObjectWidth;
            else if (Position.X < 5)
                Position.X = 5;

            ObjectTextures[0].PositionUL = Position;
        }


        public override void InitTextures()
        {
            ObjectTextures = new List<GameBitmap>();
            ObjectTextures.Add(new GameBitmap("\\Resources\\Images\\element_purple_square.png", Position.X,
                Position.Y, ObjectWidth, ObjectHeight, "element_purple_square"));
        }

        public SmallPurpleBrick(Vector2D positionVector, int virtualGameWidth, int virtualGameHeight)
            : base()
        {
            this.GameWidth = virtualGameWidth;
            this.GameHeight = virtualGameHeight;
            this.Position = new Vector2D(positionVector);
            ObjectWidth = 100;
            ObjectHeight = 80;
            Velocity = new Vector2D(0, 0);

            this.Health = 100;
            this.DamageEffect = 100;
            this.InitTextures();
        }


    }
}

