using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon
{
	public class BfsTask
	{
		public static IEnumerable<Point> GetNewPoints(Map map, Point start)
		{
			var listPoint = new List<Point>();
			if (start.X - 1 >= 0 && map.Dungeon[start.X - 1, start.Y] is MapCell.Empty)
			{
				listPoint.Add(new Point(start.X - 1, start.Y));
			}
			if (start.Y - 1 >= 0 && map.Dungeon[start.X, start.Y - 1] is MapCell.Empty)
			{
				listPoint.Add(new Point(start.X, start.Y - 1));
			}
			if (start.X + 1 <= map.Dungeon.GetUpperBound(0) && map.Dungeon[start.X + 1, start.Y] is MapCell.Empty)
			{
				listPoint.Add(new Point(start.X + 1, start.Y));
			}
			if (start.Y + 1 <= map.Dungeon.GetUpperBound(1) && map.Dungeon[start.X, start.Y + 1] is MapCell.Empty)
			{
				listPoint.Add(new Point(start.X, start.Y + 1));
			}
			return listPoint;
		}

		public static HashSet<Point> GetHashSetPoint(Point[] points)
		{
			var hashChests = new HashSet<Point>();

			foreach (var point in points)
			{
				hashChests.Add(point);
			}

			return hashChests;
		}

		public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
		{
			var queue = new Queue<SinglyLinkedList<Point>>();
			var hashChests = GetHashSetPoint(chests);
			var visited = new HashSet<Point>
			{
				start
			};
			queue.Enqueue(new SinglyLinkedList<Point>(start));

			while (queue.Count != 0)
			{
				var previous = queue.Dequeue();

				foreach (var next in GetNewPoints(map, previous.Value))
				{
					if (visited.Contains(next))
					{
						continue;
					}

					var result = new SinglyLinkedList<Point>(next, previous);
					visited.Add(next);
					queue.Enqueue(result);

					if (hashChests.Contains(next))
					{
						yield return result;
					}
				}
			}
			yield break;
		}
	}
}

