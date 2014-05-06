using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DracosDescendentsLevelEditor
{
    public class AI
    {
        private List<Tuple<int, int>> waypoints;

        public AI()
        {
            waypoints = new List<Tuple<int, int>>();
        }

        public void scale(float scale_value)
        {
            for (int i = 0; i < waypoints.Count() ; i++)
            {
                Tuple<int, int> old_waypoint = waypoints[i];
                Tuple<int, int> new_waypoint = Tuple.Create((int)(old_waypoint.Item1 * scale_value), 
                    (int)(old_waypoint.Item2 * scale_value));
                waypoints[i] = new_waypoint;
            }
        }

        public void clearAI()
        {
            waypoints = new List<Tuple<int, int>>();
        }

        public void addWaypoint(int x, int y)
        {
            waypoints.Add(Tuple.Create(x,y));
        }

        public void addWaypoint(int x, int y, Tuple<int, int> insertAfterMe)
        {
            if (insertAfterMe == null)
            {
                addWaypoint(x, y);
            }
            else
            {
                waypoints.Insert(waypoints.IndexOf(insertAfterMe), Tuple.Create(x, y));
            }
        }

        public List<Tuple<int, int>> getWaypoints()
        {
            return waypoints;
        }

        public void deleteWaypoint(Tuple<int, int> waypoint)
        {
            waypoints.Remove(waypoint);
        }

        public void updateWaypoint(Tuple<int, int> waypoint, Tuple<int, int> insertAfterMe, int x, int y)
        {
            waypoints.Remove(waypoint);
            if (insertAfterMe != null)
            {
                waypoints.Insert(waypoints.IndexOf(insertAfterMe), Tuple.Create(x, y));
            }
            else
            {
                waypoints.Insert(0, Tuple.Create(x, y));
            }
        }

        /// <summary>
        /// Used to display the AI's info for ComboBoxes
        /// </summary>
        public string Display
        {
            get
            {
                int index = LevelEditorForm.aiList.IndexOf(this);
                string display = "AI ";
                if (index == 2)
                {
                    display += "(default: blank AI)";
                }
                else if (index == 0)
                {
                    display += "(default: custom AI 1)";
                }
                else if (index == 1)
                {
                    display += "(default: custom AI 2)";
                }
                return display;
            }
        }
    }
}