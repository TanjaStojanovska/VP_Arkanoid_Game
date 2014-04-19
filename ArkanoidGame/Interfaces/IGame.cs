﻿using ArkanoidGame.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ArkanoidGame.Interfaces
{
    public interface IGame
    {
        /// <summary>
        /// Име на играта
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Колку периоди од 40 ms поминале во играта
        /// </summary>
        long ElapsedTime { get; set; }

        /// <summary>
        /// Состојба во која се наоѓа играта
        /// </summary>
        IGameState GameState { get; set; }

        /// <summary>
        /// Ја повикува функцијата Draw кај секој објект од играта
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="frameWidth"></param>
        /// <param name="frameHeight"></param>
        void OnDraw(Graphics graphics, int frameWidth, int frameHeight);
        
        void OnUpdate();

        int VirtualGameWidth { get; }
        int VirtualGameHeight { get; } //Играта има посебни единици за должина од прозорецот на кој е црта
    }
}
