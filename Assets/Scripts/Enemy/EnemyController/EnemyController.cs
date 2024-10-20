using UnityEngine;
using MPathFinder;

/// <summary>
/// ナビゲーター設定できるオブジェクト
/// </summary>
public interface ICanSetNavigator
{
    /// <summary>
    /// ナビゲーターを設定
    /// </summary>
    /// <param name="navigator">ナビゲーター</param>
    public void SetNavigator(INavigator navigator);
}

public interface INavigatable
{
  public Transform GetNavigateTarget();
}

namespace IV
{
  namespace Enemy
  {
    public class EnemyController : MonoBehaviour, ICanSetNavigator,INavigatable,ICatchable
    {
      private int [,] maze = 
      {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,1,1,1,1,1},
        {1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,0,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1},
        {1,1,0,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
      };

      AStarPoint start = new AStarPoint
      {
        x = 17,
        y = 1,
      };
      AStarPoint end = new AStarPoint
      {
        x = 4,
        y = 8,
      };

      AStar astar = new AStar();
      // TODO
      private INavigator _navigator;
      private float _speed = 5f;
      AStarPoint[] testPath;
      private int pathFindIndex = 0;


      public Transform Target;

      private Vector2 ChaseEndPoint;
      private void Awake()
      {
        _navigator = new Navigator();
        _navigator.InitNavigator(transform);
        astar.InitMaze(maze);
        testPath = astar.GetPath(start,end,false);
      }

      private void Start()
      {
        if (testPath != null && testPath.Length > 0)
        {
          var start = testPath[pathFindIndex++];
          Vector2 StartPos = new Vector2((float)start.y - 16.5f,-(float)start.x + 9.5f);
          _navigator.StartNavigate(StartPos);
          Debug.Log(StartPos);
        }
      }

      private void Update()
      {
        if (Target != null)
        {
          var pos = (Vector2)Target.transform.position;
          if (pos != ChaseEndPoint)
          {
            ChaseEndPoint = pos;
            start = new AStarPoint();
            start.x = Mathf.FloorToInt(-(transform.position.y - 0.5f - 9.5f));
            start.y = Mathf.FloorToInt((transform.position.x + 0.5f + 16.5f));

            end = new AStarPoint();
            end.x = Mathf.FloorToInt(-(ChaseEndPoint.y - 0.5f - 9.5f));
            end.y = Mathf.FloorToInt((ChaseEndPoint.x + 0.5f + 16.5f));

            testPath = astar.GetPath(start,end,false);
            pathFindIndex = 1;
          }
        }
        _navigator?.UpdateNavigating(_speed);

        if((_navigator != null) && _navigator.IsReachTargetDestination)
        {
          if (pathFindIndex < testPath.Length)
          {
            var next = testPath[pathFindIndex++];
            Vector2 nextPos = new Vector2((float)next.y - 16.5f,-(float)next.x + 9.5f);
            _navigator.StartNavigate(nextPos);
          }
        }
      }
      public void SetNavigator(INavigator navigator)
      {
        _navigator = navigator;
        _navigator.InitNavigator(transform);
      }

      public Transform GetNavigateTarget()
      {
        return transform;
      }

      public void OnCatch(ICanBeCaught beCaught)
      {
        beCaught.GetCaught(this);
      }
    }
  }
}