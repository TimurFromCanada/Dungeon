using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class DungeonTask
	{
		public static List<Point> GetMergeMinList(IEnumerable<Tuple<SinglyLinkedList<Point>, SinglyLinkedList<Point>>> tuple)
		{
			if (tuple.Count() == 0 || tuple.First().Item2 == null)
			{
				return null;
			}

			var min = int.MaxValue;
			var minElement = tuple.First();

			foreach (var element in tuple)
			{
				if (element.Item1.Length + element.Item2.Length < min)
				{
					min = element.Item1.Length + element.Item2.Length;
					minElement = element;
				}
			}

			return minElement
				.Item1.Reverse()
				.Concat(minElement.Item2.Reverse())
				.ToList();
		}

		public static MoveDirection[] GetMoveDirectionArray(List<Point> points)
		{
			var moveDirection = new List<MoveDirection>();

			for (var i = 1; i < points.Count; i++)
			{
				if (points[i].X - points[i - 1].X == 1)
					moveDirection.Add(MoveDirection.Right);
				else if (points[i].X - points[i - 1].X == -1)
					moveDirection.Add(MoveDirection.Left);
				else if (points[i].Y - points[i - 1].Y == 1)
					moveDirection.Add(MoveDirection.Down);
				else if (points[i].Y - points[i - 1].Y == -1)
					moveDirection.Add(MoveDirection.Up);
			}

			return moveDirection.ToArray();
		}

		public static MoveDirection[] FindShortestPath(Map map)
		{
			var wayFromStartToExit = BfsTask.FindPaths(map, map.InitialPosition, new Point[] { map.Exit }).FirstOrDefault();

			if (wayFromStartToExit != null)
			{
				if (map.Chests.Any(pointChest => wayFromStartToExit.Contains(pointChest)))
				{
					return GetMoveDirectionArray(wayFromStartToExit.Reverse().ToList());
				}
				else
				{
					var wayFromStartToChest = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
					var mergeWay = wayFromStartToChest
						.Select(single => Tuple.Create(single, BfsTask.FindPaths(map, single.Value, new[] { map.Exit }).FirstOrDefault()));
					var result = GetMergeMinList(mergeWay);

					if (result == null)
					{
						return GetMoveDirectionArray(wayFromStartToExit.Reverse().ToList());
					}

					return GetMoveDirectionArray(result);
				}
			}
			else
			{
				return new MoveDirection[0];
			}
		}
	}
}

