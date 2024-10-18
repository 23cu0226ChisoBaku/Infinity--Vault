using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace MEvent
{
  public interface IUnregister
  {
    public void Unregister();
  }

  public interface ICancelable : IDisposable
  {
    bool IsDisposed{get;}
  }
    public class AnonymousDisposable : ICancelable
    {
      private volatile Action _dispose;
      private readonly MethodInfo _targetMethodInfo;
      public bool IsDisposed => _dispose == null;

      public AnonymousDisposable(Action dispose, MethodInfo equal)
      {
        System.Diagnostics.Debug.Assert(dispose != null);
        System.Diagnostics.Debug.Assert(equal != null);

        _dispose = dispose;
        _targetMethodInfo = equal;

      }
      public void Dispose()
      {
          Interlocked.Exchange(ref _dispose, null)?.Invoke();
      }

      public static bool operator ==(AnonymousDisposable lhs, AnonymousDisposable rhs)
      {
        return lhs._targetMethodInfo == rhs._targetMethodInfo;
      }

      public static bool operator !=(AnonymousDisposable lhs, AnonymousDisposable rhs)
      {
        return !(lhs == rhs);
      }

      public static bool operator ==(AnonymousDisposable disposable, Action handler)
      {
        return disposable._targetMethodInfo == handler.Method;
      }

      public static bool operator !=(AnonymousDisposable disposable, Action handler)
      {
        return !(disposable == handler);
      }

      public override bool Equals(object obj)
      {
          return obj is AnonymousDisposable disposable && Equals(disposable);
      }
      public bool Equals(AnonymousDisposable handle)
      {
          return this == handle;
      }
      public override int GetHashCode()
      {
          return HashCode.Combine(_dispose,_targetMethodInfo);
      }
    }

    public static class DisposeGenerator
    {
      public static IDisposable Create(Action dispose, MethodInfo equalTest)
      {
        if(dispose == null)
        {
          throw new ArgumentNullException(nameof(dispose));
        }

        return new AnonymousDisposable(dispose,equalTest);
      }
    }

  public class DisposableEvent : ICancelable
  {
    private readonly List<IDisposable> _disposables;

    public bool IsDisposed => _eventHandler == null;

    private event Action _eventHandler;
    public DisposableEvent()
    {
      _eventHandler = null;
      _disposables = new List<IDisposable>();
    }
    public DisposableEvent(params Action[] handlers)
      :this()
    {
      foreach(var handler in handlers)
      {
        Subscribe(handler);
      }
    }

    ~DisposableEvent()
    {
      Dispose(false);
    }

    public void Subscribe(Action handler)
    {
      _eventHandler += handler;
      var disposable = DisposeGenerator.Create(()=>
      {
        _eventHandler -= handler;
      },handler.Method);

      _disposables.Add(disposable);

    }

    public void Unsubscribe(Action handler)
    {
      foreach(var disposable in _disposables)
      {
        if ((AnonymousDisposable)disposable == handler)
        {
          disposable.Dispose();
          _disposables.Remove(disposable);
          break;
        }
      }
    }
    public void Dispose()
    {
      Dispose(true);
    }

    private void Dispose(bool disposing)
    {
      if (disposing)
      {
        GC.SuppressFinalize(this);
      }

      foreach(var dispose in _disposables)
      {
        dispose?.Dispose();
      }
      _disposables.Clear();
      _eventHandler = null;

    }

    public void Invoke()
    {
      _eventHandler?.Invoke();
    }

    public static DisposableEvent operator +(DisposableEvent origin,Action handler)
    {
      origin.Subscribe(handler);
      return origin;
    }

    public static DisposableEvent operator +(Action handler, DisposableEvent origin)
    {
      origin.Subscribe(handler);
      return origin;
    }

  }
}