using System;

namespace PSMWebAPI.Services;

public class MarkerService
{
    public static bool IsInside(int circle_x, int circle_y, 
                              int rad, int x, int y)
    {
        // Compare radius of circle with
        // distance of its center from
        // given point
        if ((x - circle_x) * (x - circle_x) +
            (y - circle_y) * (y - circle_y)    <= rad * rad)
            return true;
        else
            return false;
    }
}
