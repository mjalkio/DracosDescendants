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
            waypoints.Insert(waypoints.IndexOf(insertAfterMe), Tuple.Create(x,y));
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