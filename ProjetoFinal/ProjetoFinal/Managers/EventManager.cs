﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProjetoFinal.EventArgs;
using ProjetoFinal.Entities;

namespace ProjetoFinal.Managers
{
    class EventManager
    {
        private static EventManager instance;

        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new EventManager();

                return instance;
            }
        }

        public event EventHandler<PlayerStateChangedArgs> PlayerStateChanged;

        public void throwPlayerStateChanged(short id, Player player)
        {
            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedArgs(id, player));
        }
    }
}
