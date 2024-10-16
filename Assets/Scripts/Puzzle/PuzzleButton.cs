using System;
using MEvent;
using UnityEngine; 

namespace IV
{
  namespace Puzzle
  {
    /// <summary>
    /// パズル専用ボタンインターフェース
    /// TODO パズルと同じ扱いしていいの？？？？
    /// </summary>
    internal interface IPuzzleButton : IPuzzleBase
    {
      /// <summary>
      /// ボタンが押された(まだ離していない)とき呼び出されるコールバック
      /// </summary>
      public event Action OnPress;
      /// <summary>
      /// ボタンを離した(クリックやマウスをボタンから離した)とき呼び出されるコールバック
      /// </summary>
      public event Action OnRelease;
      /// <summary>
      /// ボタンがクリックされたとき呼び出されるコールバック
      /// </summary>
      public event Action OnClick;

      /// <summary>
      /// ボタンを破棄する
      /// </summary>
      public void DisposeButton();
    }
    /// <summary>
    /// パズル専用ボタンクラス
    /// </summary>
    internal class PuzzleButton : MonoBehaviour, IPuzzleButton
    {
      private DisposableEvent _onPressEvent = new DisposableEvent();
      private DisposableEvent _onReleaseEvent = new DisposableEvent();
      private DisposableEvent _onClickEvent = new DisposableEvent();
      event Action IPuzzleButton.OnPress
      {
        add
        {
          _onPressEvent.Subscribe(value);
        }
        remove
        {
          _onPressEvent.Unsubscribe(value);
        }
      }
      event Action IPuzzleButton.OnRelease
      {
        add
        {
          _onReleaseEvent.Subscribe(value);
        }
        remove
        {
          _onReleaseEvent.Unsubscribe(value);
        }
      }
      event Action IPuzzleButton.OnClick
      {
        add
        {
          _onClickEvent.Subscribe(value);
        }
        remove
        {
          _onClickEvent.Unsubscribe(value);
        }
      }
      private bool _isButtonPressed;
      private void Awake()
      {
        _isButtonPressed = false;
        ((IPuzzleButton)this).OnPress += () => {Debug.Log("On Press");};
        ((IPuzzleButton)this).OnRelease += () => {Debug.Log("On Release");};
        ((IPuzzleButton)this).OnClick += () => {Debug.Log("On Click");};
      }
      private void OnMouseDown() 
      {
        _isButtonPressed = true;
        _onPressEvent?.Invoke();
      }
      private void OnMouseUp() 
      {    
        if (_isButtonPressed)
        {
          _onReleaseEvent?.Invoke();
          _onClickEvent?.Invoke();
          enabled = false;
        }
      }
      private void OnMouseExit() 
      {
        if (_isButtonPressed)
        {
          _onReleaseEvent?.Invoke();
          _isButtonPressed = false;
        }
      }
      void IPuzzleBase.ShowPuzzle()
      {
        gameObject.SetActive(true);
        _isButtonPressed = false;
      }

      void IPuzzleBase.HidePuzzle()
      {
        gameObject.SetActive(false);
        _isButtonPressed = false;
      }

      void IPuzzleBase.ResetPuzzle()
      {
        _onClickEvent.Dispose();
        _onPressEvent.Dispose();
        _onReleaseEvent.Dispose();

        _isButtonPressed = false;
      }

      public void DisposeButton()
      {
        ((IPuzzleButton)this).ResetPuzzle();
        Destroy(gameObject);
      }
  }
  }
}