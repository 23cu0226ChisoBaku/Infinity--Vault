using UnityEngine;

internal interface ICanClimb
{
    Transform GetTransform();
}
/// <summary>
/// はしごなど登られる/降りられるもの
/// </summary>
internal interface IClimbable
{
    public float ClimbTopRate {get;}
    public float ClimbBottomRate {get;}

    public Vector2 ClimbTopPos {get;}
    public Vector2 ClimbBottomPos {get;}
    public float ClimbLength {get;}
}