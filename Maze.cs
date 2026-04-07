using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class GridMap : EditorWindow
{
    private int[,] maze;
    private List<Vector2Int> path;
    private System.Random rand = new System.Random();

    // ===== SETTINGS =====
    public static int mazeWidth = 10;
    public static int mazeHeight = 10;

    public GameObject tilePrefab;
    public Transform parent;
    public float cellSize = 1f;

    [MenuItem("Tools/Grid Map Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridMap>("Maze Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Maze Settings", EditorStyles.boldLabel);

        mazeWidth = EditorGUILayout.IntField("Width", mazeWidth);
        mazeHeight = EditorGUILayout.IntField("Height", mazeHeight);
        cellSize = EditorGUILayout.FloatField("Cell Size", cellSize);

        tilePrefab = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", tilePrefab, typeof(GameObject), false);
        parent = (Transform)EditorGUILayout.ObjectField("Parent", parent, typeof(Transform), true);

        if (GUILayout.Button("Generate Maze In Scene"))
        {
            GenerateMaze(mazeWidth, mazeHeight);
            path = null;
            DrawMazeInScene();
        }

        if (GUILayout.Button("Find Path"))
        {
            if (maze != null)
            {
                FindPath();
            }
            else
            {
                Debug.LogWarning("Generate maze first!");
            }
        }
    }

    // ===== MAZE GENERATION (DFS) =====
    void GenerateMaze(int width, int height)
    {
        int w = width * 2 + 1;
        int h = height * 2 + 1;

        maze = new int[w, h];

        // 1 = tường
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                maze[x, y] = 1;

        DFS(1, 1, w, h);

        // Entrance
        maze[1, 0] = 0;

        // Exit
        maze[w - 2, h - 1] = 0;
    }

    void DFS(int x, int y, int w, int h)
    {
        maze[x, y] = 0;

        int[] dirs = { 0, 1, 2, 3 };
        Shuffle(dirs);

        foreach (int dir in dirs)
        {
            int nx = x, ny = y;

            switch (dir)
            {
                case 0: ny = y + 2; break;
                case 1: nx = x + 2; break;
                case 2: ny = y - 2; break;
                case 3: nx = x - 2; break;
            }

            if (nx > 0 && ny > 0 && nx < w - 1 && ny < h - 1 && maze[nx, ny] == 1)
            {
                maze[(x + nx) / 2, (y + ny) / 2] = 0;
                DFS(nx, ny, w, h);
            }
        }
    }

    void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    // ===== PATHFINDING (A*) =====
    void FindPath()
    {
        int w = maze.GetLength(0);
        int h = maze.GetLength(1);

        Vector2Int start = new Vector2Int(1, 0);
        Vector2Int end = new Vector2Int(w - 2, h - 1);

        path = AStar(start, end);

        DrawMazeInScene();
    }

    List<Vector2Int> AStar(Vector2Int start, Vector2Int goal)
    {
        var openSet = new List<Vector2Int> { start };

        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        var gScore = new Dictionary<Vector2Int, int>();
        gScore[start] = 0;

        var fScore = new Dictionary<Vector2Int, int>();
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet[0];

            foreach (var node in openSet)
            {
                if (fScore.ContainsKey(node) && fScore[node] < fScore[current])
                    current = node;
            }

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                int tentativeG = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }

    int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] dirs = {
            new Vector2Int(0,1),
            new Vector2Int(1,0),
            new Vector2Int(0,-1),
            new Vector2Int(-1,0)
        };

        foreach (var dir in dirs)
        {
            Vector2Int next = node + dir;

            if (next.x >= 0 && next.y >= 0 &&
                next.x < maze.GetLength(0) &&
                next.y < maze.GetLength(1) &&
                maze[next.x, next.y] == 0)
            {
                neighbors.Add(next);
            }
        }

        return neighbors;
    }

    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> totalPath = new List<Vector2Int>();
        totalPath.Add(current);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }

        totalPath.Reverse();
        return totalPath;
    }

    // ===== DRAW =====
    void DrawMazeInScene()
    {
        if (tilePrefab == null)
        {
            Debug.LogWarning("Missing Tile Prefab!");
            return;
        }

        int w = maze.GetLength(0);
        int h = maze.GetLength(1);

        // clear cũ
        if (parent != null)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector3 pos = new Vector3(x * cellSize, 0, y * cellSize);

                GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab);

                tile.transform.position = pos;
                tile.transform.rotation = Quaternion.identity;
                tile.transform.localScale = Vector3.one * cellSize;

                if (parent != null)
                    tile.transform.SetParent(parent);

                var renderer = tile.GetComponent<Renderer>();
                if (renderer != null)
                {
                    if (maze[x, y] == 1)
                    {
                        renderer.material.color = Color.black; // wall
                    }
                    else
                    {
                        Vector2Int pos2D = new Vector2Int(x, y);

                        if (path != null && path.Contains(pos2D))
                        {
                            renderer.material.color = Color.yellow; // path
                        }
                        else
                        {
                            renderer.material.color = new Color(0.3f, 0.2f, 0.8f); // road
                        }
                    }
                }
            }
        }
    }
}
