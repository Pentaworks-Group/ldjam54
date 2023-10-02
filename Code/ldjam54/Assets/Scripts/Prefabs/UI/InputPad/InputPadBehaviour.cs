using UnityEngine;

namespace Assets.Scripts.Scenes.Space.InputHandling
{
    public class InputPadBehaviour : MonoBehaviour
    {
        private SpaceShipBehaviour shipBehaviour;

        public void Init(SpaceShipBehaviour shipBehaviour)
        {
            this.shipBehaviour = shipBehaviour;
            gameObject.SetActive(true);
        }

        public void OnButtonBottomMiddle()
        {
            shipBehaviour.DeAccelerate();
        }

        public void OnButtonMiddleMiddle()
        {
            shipBehaviour.FireProjectile();
        }

        public void OnButtonMiddleLeft()
        {
            shipBehaviour.TurnLeft();
        }

        public void OnButtonMiddleRight()
        {
            shipBehaviour.TurnRight();
        }

        public void OnButtonTopMiddle()
        {
            shipBehaviour.Accelerate();
        }
    }
}
