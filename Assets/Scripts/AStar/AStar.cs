// Copyright 2020 Talespin, LLC. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
	// algorithm based on WikiPedia: http://en.wikipedia.org/wiki/A*_search_algorithm
	//
	// implements the static GetPath(...) function that will return a IList of IAStarNodes that is the shortest currentPath
	// between the two IAStarNodes that are passed as parameters to the function
	public static class AStar
	{
		private class OpenSorter : IComparer<IAStarNode>
		{
			private Dictionary<IAStarNode, float> fScore;

			public OpenSorter(Dictionary<IAStarNode, float> f)
			{
				fScore = f;
			}

			public int Compare(IAStarNode x, IAStarNode y)
			{
				if (x != null && y != null)
					return fScore[x].CompareTo(fScore[y]);
				else
					return 0;
			}
		}

		private static List<IAStarNode> closed;
		private static List<IAStarNode> open;
		private static Dictionary<IAStarNode, IAStarNode> cameFrom;
		private static Dictionary<IAStarNode, float> gScore;
		private static Dictionary<IAStarNode, float> hScore;
		private static Dictionary<IAStarNode, float> fScore;

		static AStar()
		{
			closed = new List<IAStarNode>();
			open = new List<IAStarNode>();
			cameFrom = new Dictionary<IAStarNode, IAStarNode>();
			gScore = new Dictionary<IAStarNode, float>();
			hScore = new Dictionary<IAStarNode, float>();
			fScore = new Dictionary<IAStarNode, float>();
		}

		// this function is the C# implementation of the algorithm presented on the wikipedia page
		// start and goal are the nodes in the graph we should find a currentPath for
		//
		// returns null if no currentPath is found
		//
		// this function is NOT thread-safe (due to using static data for GC optimization)
		public static IList<IAStarNode> GetPath(IAStarNode start, IAStarNode goal)
		{
			if (start == null || goal == null)
			{
				return null;
			}

			closed.Clear();
			open.Clear();
			//adds start node
			open.Add(start);

			cameFrom.Clear();
			gScore.Clear();
			hScore.Clear();
			fScore.Clear();

			gScore.Add(start, 0f);
			hScore.Add(start, start.EstimatedCostTo(goal));
			fScore.Add(start, hScore[start]);

			OpenSorter sorter = new OpenSorter(fScore);
			IAStarNode current,
						from = null;
			float tentativeGScore;
			bool tentativeIsBetter;

			while (open.Count > 0)
			{
                UnityEngine.Debug.Log("the count of open is: " + open.Count);
				//starting node
				current = open[0];
                //the current hexagon is equal to the goal hexagon construct final currentPath
                if (current == goal)
                {
                    return ReconstructPath(new List<IAStarNode>(), cameFrom, goal);
                }
                open.Remove(current);
				closed.Add(current);

				if (current != start)
				{
					//sets the current hex to from (past hex)
                    from = cameFrom[current];
                }

                //for each neighbour in Neighbours
                foreach (IAStarNode next in current.Neighbours)
				{
                    if (from != next && !closed.Contains(next))
                    {
                        tentativeGScore = gScore[current] + current.CostTo(next);
                        tentativeIsBetter = true;

						//if open doesn't contain current selected neighbour yet
                        if (!open.Contains(next))
                        {
                            open.Add(next);
                        }
                        else
						//checks if the tentative gscore is overestimating
                        if (tentativeGScore >= gScore[next])
                        {
							//gets set to false
                            tentativeIsBetter = false;
                        }
						//if tentative var is better 
                        if (tentativeIsBetter)
                        {
							//sets neighbour to next hex? i think...
							//GIVES NULL???
                            cameFrom[next] = current;
							//
                            gScore[next] = tentativeGScore;
                            hScore[next] = next.EstimatedCostTo(goal);
                            fScore[next] = gScore[next] + hScore[next];
                        }
                    }
                }
                open.Sort(sorter);
			}
			return null;
		}

		private static IList<IAStarNode> ReconstructPath(IList<IAStarNode> path, Dictionary<IAStarNode, IAStarNode> cameFrom, IAStarNode currentNode)
		{
			if (cameFrom.ContainsKey(currentNode))
			{
				ReconstructPath(path, cameFrom, cameFrom[currentNode]);
			}
			path.Add(currentNode);
			return path;
		}
	}
}