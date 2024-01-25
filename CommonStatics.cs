using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mangerie
{
    // Enums
    public enum Directions
    {
        up, down, left, right
    }
    public enum GameStates
    {
        levelStart, gameplay, levelEnd
    }

    

    // Static helper functions
    static class CommonStatics
    {

        // Get random value from enum T
        // (https://stackoverflow.com/questions/3132126/how-do-i-select-a-random-value-from-an-enumeration)
        static public T? RandomEnumValue<T>(Random rnd)
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(rnd.Next(v.Length));
        }

        // Apply minimum and maximum to value
        static public int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
        static public float Clamp(float value, float min, float max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
        static public double Clamp(double value, double min, double max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        // Collision Detection: circle versus square
        static public bool TryCollisionDetection(double circleX, double circleY, double radius, double squareX, double squareY, double squareSize)
        {
            double closestX = Clamp(circleX, squareX, squareX + squareSize);
            double closestY = Clamp(circleY, squareY, squareY + squareSize);
            double distanceX = circleX - closestX;
            double distanceY = circleY - closestY;
            double realDistance = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

            if (realDistance < radius)
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
    }
}
