using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace MPathFinder
{
  public class AStarPoint
  {
    public int x;
    public int y;
    public int FCost;
    public int GCost;
    public int HCost;
    public AStarPoint parent;

  }
  public class AStar
  {
    public int[,] maze;
    public List<AStarPoint> openList = new List<AStarPoint>();
    public List<AStarPoint> closeList = new List<AStarPoint>();

    public void InitMaze(int[,] map)
    {
      maze = map;
    }
    
    public int calcG(AStarPoint starPoint,AStarPoint point)
    {
      int extraG = Math.Abs(point.x - starPoint.x) + Math.Abs(point.y - starPoint.y) == 1 ? 10 : 14;
      int parentG = point.parent == null ? 0 : point.parent.GCost;
      return extraG + parentG;
    }
    public int calcH(AStarPoint point,AStarPoint endPoint)
    {
      return (int)Math.Sqrt(Math.Pow(endPoint.x - point.x,2)+Math.Pow(endPoint.y - point.y,2)) * 10;
    }

    public int calcF(AStarPoint point)
    {
      return point.GCost+point.HCost;
    }

    public AStarPoint GetPathImpl(AStarPoint starPoint, AStarPoint endPoint, bool isIgnoreCorner)
    {
      var copyPoint = new AStarPoint();
      copyPoint.x = starPoint.x;
      copyPoint.y = starPoint.y;
      openList.Add(copyPoint);

      while (!(openList.Count < 1))
      {
        var curtPoint = GetLeastFPoint();
        openList.Remove(curtPoint);
        closeList.Add(curtPoint);

        var surroundPoints = GetSurroundPoints(curtPoint,isIgnoreCorner);
        foreach(var surround in surroundPoints)
        {
          if(!IsInList(openList,surround))
          {
            surround.parent = curtPoint;
            
            surround.GCost=calcG(curtPoint,surround);
            surround.HCost=calcH(surround,endPoint);
            surround.FCost=calcF(surround);

            openList.Add(surround);
          }
          else
          {
            int tempG=calcG(curtPoint,surround);
            if(tempG<surround.GCost)
            {
              surround.parent = curtPoint;

              surround.GCost=tempG;
              surround.FCost=calcF(surround);
            }
          }
          if(IsInList(openList,endPoint))
          {
            return openList.Find(entity => {return entity.x==endPoint.x&&entity.y==endPoint.y;});
          }
        }
      }
      return null;
    }
    public AStarPoint[] GetPath(AStarPoint starPoint, AStarPoint endPoint, bool isIgnoreCorner = false)
    {
      var result = GetPathImpl(starPoint,endPoint,isIgnoreCorner);
      Stack<AStarPoint> path = new Stack<AStarPoint>();
      while(result != null)
      {
        path.Push(result);
        result = result.parent;
      }

      openList.Clear();
      closeList.Clear();

      return path.ToArray();
    }

    public AStarPoint GetLeastFPoint()
    {
      if (openList.Count > 0)
      {
        var resultPoint = openList.First();
        foreach(var point in openList)
        {
          if (point.FCost < resultPoint.FCost)
          {
            resultPoint = point;
          }
        }
        return resultPoint;
      }
      return null;
    }

    public AStarPoint[] GetSurroundPoints(AStarPoint point, bool isIgnoreCorner)
    {
      List<AStarPoint> surrondPoints = new List<AStarPoint>();

      for (int xcoor = point.x-1;xcoor<=point.x+1;++xcoor)
      {
        for(int ycoor = point.y-1;ycoor<=point.y+1;++ycoor)
        {
          if(IsCanReach(point,new AStarPoint{x = xcoor,y = ycoor},isIgnoreCorner))
          {
            surrondPoints.Add(new AStarPoint{x = xcoor,y = ycoor});
          }
        }
      }

      return surrondPoints.ToArray();
      
    }

    public bool IsCanReach(AStarPoint point,AStarPoint target,bool isIgnoreCorner)
    {
      if (   (target.x <0 ||target.x > maze.GetLength(0) - 1)
          || (target.y <0 ||target.y > maze.GetLength(1) - 1)
          || (maze[target.x,target.y] == 1)
          || (target.x==point.x&&target.y==point.y)
          || (IsInList(closeList,target))
         )
      {
        return false;
      }
      else
      {
        if(Math.Abs(point.x-target.x)+Math.Abs(point.y-target.y)==1)
        {
          return true;
        }
        else
        {
          if(maze[point.x,target.y]==0&&maze[target.x,point.y]==0)
          {
            return true;
          }
          else
          {
            return isIgnoreCorner;
          }
        }
      }
    }

    public bool IsInList(List<AStarPoint> list,AStarPoint target)
    {
      foreach(var point in list)
      {
        if(point.x == target.x && point.y == target.y)
        {
          return true;
        }
      }

      return false;
    }
  }
}