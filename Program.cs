// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

// After reading the problem brief I assume that the cones view is the specified degrees either side 
// of 0 degrees for north, 90 degrees for east, 180 degrees for south, 270 degrees for west if we 
// were looking at a compass.

// Replaced North, South, East and West with the angle in degrees that
// will be used as reference points for calculating the cone angle
double[,] PointData ={  { 28, 42, 1, 0 },
                        { 27, 46, 2, 90 },
                        { 16, 22, 3, 180 },
                        { 40, 50, 4, 270 },
                        { 8, 6, 5, 0 },
                        { 6, 19, 6, 90 },
                        { 28, 5, 7, 180 },
                        { 39, 36, 8, 270 },
                        { 12, 34, 9, 0 },
                        { 36, 20, 10, 90 },
                        { 22, 47, 11, 180 },
                        { 33, 19, 12, 270 },
                        { 41, 18, 13, 0 },
                        { 41, 34, 14, 90 },
                        { 14, 29, 15, 180 },
                        { 6, 49, 16, 270 },
                        { 46, 50, 17, 0 },
                        { 17, 40, 18, 90 },
                        { 28, 26, 19, 180 },
                        { 2, 12, 20, 270 } };   

List<double[]> PointResult = new List<double[]>();

// 1
int originPoint = 1;
int maxDistance = 20;
int coneDegrees = 45;

double leftAngle = normaliseAngle2(PointData[originPoint - 1, 3] - coneDegrees);
double rightAngle = normaliseAngle2(PointData[originPoint - 1, 3] + coneDegrees);

// loops through the pointdata to check each point
for (int a = 0; a < PointData.GetLength(0); a++)
{
    if (originPoint != (a + 1))
    {
        // if point isn't the origin point then check angle and distance to origin point to
        // determine if its in the cones view
        double angle = normaliseAngle(radiansToDegrees(calculateAngle(PointData[originPoint - 1, 0], PointData[originPoint - 1, 1], PointData[a, 0], PointData[a, 1])));
        double distToPoint = Distance(PointData[originPoint - 1, 0], PointData[originPoint - 1, 1], PointData[a, 0], PointData[a, 1]);

        if ((distToPoint < maxDistance) &&
            isWithinCone(angle))
        {
            // should be a valid point so this pointdata to the list.
            double[] pointDat = { PointData[a, 0], PointData[a, 1], PointData[a, 2], PointData[a, 3] };
            PointResult.Add(pointDat);
        }
    }
}

PointResult.ForEach(i =>
{
    Console.WriteLine(i[0] + ", " + i[1] + ", " + i[2] +", " + i[3]);
});

bool isWithinCone(double angle)
{

    // returns true if angle is within the constraints of cone angle
    if (leftAngle < rightAngle)
    { 
        // left angle is lower than right so just check angle is between the range
        return (angle >= leftAngle) && (angle <= rightAngle);
    }
    else
    {
        // handle range spanning 0 degrees/360 degrees
        return (angle >= leftAngle) || (angle <= rightAngle);
    }
}
double Distance(double oX, double oY, double dX, double dY)
{
    // returns distance of line between two points in 2d space
    double distance = 0;

    distance = Math.Sqrt(((dX - oX) * (dX - oX)) + ((dY - oY) * (dY - oY)));

    return distance;
}

double calculateAngle(double oX, double oY, double dX, double dY)
{
    // use atan2 function to calculate the angle using reverse tangent
    double angleInRadians = Math.Atan2((double)(dY - oY), (double)(dX - oX));
    return angleInRadians;
}

double radiansToDegrees(double angle)
{
    // convert radians to degrees by multiplying radians by result of 180/PI
    return angle * (180/Math.PI);
}

double normaliseAngle(double angle)
{
    // as well as normalise i want to orient since it appears atan2 sees east or the right of the point on
    // the x axis in our 0 degree, and subtract from 360 to give clockwise degree value
    return ((360 - (angle -90)) + 360) % 360;
}

double normaliseAngle2(double angle)
{
    // same as the normaliseAngle function but omitting the fixing of atan2 degrees,
    // this is purely for hard coded degrees if needed such as the directions.
    return ((angle + 360) % 360);
}