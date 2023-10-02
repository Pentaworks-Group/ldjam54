using UnityEngine;

namespace Assets.Scripts.Scenes.Space.InputHandling
{
    public class InputPadBehaviour : MonoBehaviour
    {
        private SpacecraftBehaviour shipBehaviour;

        public void Init(SpacecraftBehaviour shipBehaviour)
        {
            this.shipBehaviour = shipBehaviour;
            gameObject.SetActive(true);
        }

        public void OnButtonBottomMiddle()
        {
            shipBehaviour.Decelerate();
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
