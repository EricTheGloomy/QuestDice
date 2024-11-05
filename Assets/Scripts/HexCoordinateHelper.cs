// Scripts/Utilities/HexCoordinateHelper.cs
using UnityEngine;

public static class HexCoordinateHelper
{
    public static Vector2 OffsetToAxial(Vector2 offsetCoords)
    {
        float q = offsetCoords.x - (offsetCoords.y - ((int)offsetCoords.y & 1)) / 2f;
        float r = offsetCoords.y;
        return new Vector2(q, r);
    }

    public static Vector3 AxialToCube(Vector2 axialCoords)
    {
        float x = axialCoords.x;
        float z = axialCoords.y;
        float y = -x - z;
        return new Vector3(x, y, z);
    }

    public static Vector2 CubeToAxial(Vector3 cubeCoords)
    {
        float q = cubeCoords.x;
        float r = cubeCoords.z;
        return new Vector2(q, r);
    }

    public static Vector2 AxialToOffset(Vector2 axialCoords)
    {
        float col = axialCoords.x + (axialCoords.y - ((int)axialCoords.y & 1)) / 2f;
        float row = axialCoords.y;
        return new Vector2(col, row);
    }

    public static Vector2 CubeToOffset(Vector3 cubeCoords)
    {
        Vector2 axial = CubeToAxial(cubeCoords);
        return AxialToOffset(axial);
    }

    public static Vector3 GetWorldPosition(Vector2 offsetCoords, bool useFlatTop, float tileSizeX, float tileSizeZ)
    {
        if (useFlatTop)
        {
            float xPos = offsetCoords.x * tileSizeX * Mathf.Cos(Mathf.Deg2Rad * 30);
            float zPos = offsetCoords.y * tileSizeZ + ((offsetCoords.x % 2 == 1) ? tileSizeZ * 0.5f : 0);
            return new Vector3(xPos, 0, zPos);
        }
        else
        {
            float xPos = offsetCoords.x * tileSizeX + ((offsetCoords.y % 2 == 1) ? tileSizeX * 0.5f : 0);
            float zPos = offsetCoords.y * tileSizeZ * Mathf.Cos(Mathf.Deg2Rad * 30);
            return new Vector3(xPos, 0, zPos);
        }
    }
}
