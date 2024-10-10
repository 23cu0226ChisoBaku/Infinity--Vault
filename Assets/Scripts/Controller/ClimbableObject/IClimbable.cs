using UnityEngine;
internal interface IClimbable
{
    public float GetClimbRate(ICanClimb canClimb);
    public void Climb(ICanClimb canClimb, Vector2 moveDir);
}