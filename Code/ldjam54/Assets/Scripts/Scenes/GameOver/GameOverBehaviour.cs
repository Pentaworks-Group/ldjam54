using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Scenes.Menues;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameOver
{
    public class GameOverBehaviour : BaseMenuBehaviour
    {
        public void Retry()
        {
            Base.Core.Game.Start();
        }
    }
}
