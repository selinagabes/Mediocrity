using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MeshGenerator : MonoBehaviour
{
    public SquareGrid squareGrid;
    List<Vector3> _vertices;
    List<int> _triangles;
    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = new SquareGrid(map, squareSize);

        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        for (int x = 0; x < squareGrid.Squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.Squares.GetLength(1); y++)
            {
                TriangulateSqaure(squareGrid.Squares[x, y]);
            }
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void TriangulateSqaure(Square square)
    {
        switch (square.Configuration)
        {
            case 0:
                break;
            case 1:
                MeshFromPoints(square.centreBottom, square.BottomLeft, square.centreLeft);
                break;
            case 2:
                MeshFromPoints(square.centreRight, square.BottomRight, square.centreBottom);
                break;
            case 4:
                MeshFromPoints(square.centreTop, square.TopRight, square.centreRight);
                break;
            case 8:
                MeshFromPoints(square.TopLeft, square.centreTop, square.centreLeft);
                break;

            // 2 points:
            case 3:
                MeshFromPoints(square.centreRight, square.BottomRight, square.BottomLeft, square.centreLeft);
                break;
            case 6:
                MeshFromPoints(square.centreTop, square.TopRight, square.BottomRight, square.centreBottom);
                break;
            case 9:
                MeshFromPoints(square.TopLeft, square.centreTop, square.centreBottom, square.BottomLeft);
                break;
            case 12:
                MeshFromPoints(square.TopLeft, square.TopRight, square.centreRight, square.centreLeft);
                break;
            case 5:
                MeshFromPoints(square.centreTop, square.TopRight, square.centreRight, square.centreBottom, square.BottomLeft, square.centreLeft);
                break;
            case 10:
                MeshFromPoints(square.TopLeft, square.centreTop, square.centreRight, square.BottomRight, square.centreBottom, square.centreLeft);
                break;

            // 3 point:
            case 7:
                MeshFromPoints(square.centreTop, square.TopRight, square.BottomRight, square.BottomLeft, square.centreLeft);
                break;
            case 11:
                MeshFromPoints(square.TopLeft, square.centreTop, square.centreRight, square.BottomRight, square.BottomLeft);
                break;
            case 13:
                MeshFromPoints(square.TopLeft, square.TopRight, square.centreRight, square.centreBottom, square.BottomLeft);
                break;
            case 14:
                MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.centreBottom, square.centreLeft);
                break;

            // 4 point:
            case 15:
                MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
                break;

            default:
                break;
        }
    }

    void MeshFromPoints(params Node[] points)
    {
        AssignVertices(points);
        if(points.Length >= 3)        
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);

    }
    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].VertexIndex.Equals(-1))
            {
                points[i].VertexIndex = _vertices.Count;
                _vertices.Add(points[i].Position);
            }
        }
    }
    
    void CreateTriangle(Node a, Node b, Node c)
    {
        _triangles.Add(a.VertexIndex);
        _triangles.Add(b.VertexIndex);
        _triangles.Add(c.VertexIndex);

    }
    void OnDrawGizmos()
    {
        //if (squareGrid != null)
        //{
        //    for (int x = 0; x < squareGrid.Squares.GetLength(0); x++)
        //    {
        //        for (int y = 0; y < squareGrid.Squares.GetLength(1); y++)
        //        {
        //            Gizmos.color = (squareGrid.Squares[x, y].TopLeft.Active) ? Color.black : Color.white;
        //            Gizmos.DrawCube(squareGrid.Squares[x, y].TopLeft.Position, Vector3.one * 0.4f);

        //            Gizmos.color = (squareGrid.Squares[x, y].TopRight.Active) ? Color.black : Color.white;
        //            Gizmos.DrawCube(squareGrid.Squares[x, y].TopRight.Position, Vector3.one * 0.4f);

        //            Gizmos.color = (squareGrid.Squares[x, y].BottomLeft.Active) ? Color.black : Color.white;
        //            Gizmos.DrawCube(squareGrid.Squares[x, y].BottomLeft.Position, Vector3.one * 0.4f);

        //            Gizmos.color = (squareGrid.Squares[x, y].BottomRight.Active) ? Color.black : Color.white;
        //            Gizmos.DrawCube(squareGrid.Squares[x, y].BottomRight.Position, Vector3.one * 0.4f);


        //            Gizmos.color = Color.grey;
        //            Gizmos.DrawCube(squareGrid.Squares[x, y].centreTop.Position, Vector3.one * 0.15f);
        //            Gizmos.DrawCube(squareGrid.Squares[x, y].centreBottom.Position, Vector3.one * 0.15f);
        //            Gizmos.DrawCube(squareGrid.Squares[x, y].centreLeft.Position, Vector3.one * 0.15f);
        //            Gizmos.DrawCube(squareGrid.Squares[x, y].centreRight.Position, Vector3.one * 0.15f);

        //        }
        //    }
        //}
    }



    public class SquareGrid
    {
        public Square[,] Squares;
        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];
            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, -mapHeight / 2 + y * squareSize + squareSize / 2, 0);
                    controlNodes[x, y] = new ControlNode(pos, map[x, y].Equals(1), squareSize);
                }
            }

            Squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    Squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }

        }
    }

    //TODO:: Rotate the squares
    public class Square
    {
        //   TL   cR   TR
        //   O----o----O 
        //   |         |
        //cL o         o  cR
        //   |         |
        //   O----o----O
        //   BL   cB   BR
        public ControlNode TopLeft, TopRight, BottomRight, BottomLeft;
        public Node centreTop, centreRight, centreBottom, centreLeft;
        public int Configuration;
        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomLeft, ControlNode bottomRight)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;

            centreTop = TopLeft.Right;
            centreRight = BottomRight.Above;
            centreBottom = BottomLeft.Right;
            centreLeft = BottomLeft.Above;

            if (topLeft.Active)
                Configuration += 8;
            if (topRight.Active)
                Configuration += 4;
            if (bottomRight.Active)
                Configuration += 2;
            if (bottomLeft.Active)
                Configuration += 1;
        }
    }
    public class Node
    {
        public Vector3 Position;
        public int VertexIndex = -1;

        public Node(Vector3 position)
        {
            Position = position;
        }
    }

    public class ControlNode : Node
    {
        public bool Active;
        public Node Above, Right;

        public ControlNode(Vector3 position, bool active, float squareSize) : base(position)
        {
            Active = active;
            Above = new Node(position + Vector3.up * squareSize / 2f);
            Right = new Node(position + Vector3.right * squareSize / 2f);

        }
    }
}
