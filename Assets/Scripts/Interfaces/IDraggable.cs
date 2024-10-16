using System;
using UnityEngine;

// /// <summary>
// /// ドラッグ操作ができるオブジェクト
// /// </summary>
// internal interface IDraggable
// {
//     /// <summary>
//     /// ドラッグする
//     /// </summary>
//     /// <param name="startPos">始点座標</param>
//     /// <param name="endPos">終点座標</param>
//     public void Drag(Vector2 startPos,Vector2 endPos);

//     /// <summary>
//     /// ドラッグできるか
//     /// </summary>
//     public bool CanDrag {get;}

//     /// <summary>
//     /// ドラッグが終了したら呼び出されるコールバック
//     /// </summary>
//     public event Action OnDragFinish;
// }