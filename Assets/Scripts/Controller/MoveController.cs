using UnityEngine;

namespace MGameLogic
{
    public abstract class MComponent
    {
        public abstract void StartComponent();
        public abstract void EndComponent();
        public abstract void OnEnableComponent();
        public abstract void OnDisableComponent();

    }
    namespace Movement
    {
        public abstract class MoveController : MComponent
        {
            public abstract void UpdateMove();
        }
        public abstract class ClimbController: MComponent
        {
            public abstract void UpdateClimb();
        }
    }
}