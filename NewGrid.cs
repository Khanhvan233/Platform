using UnityEditor;
using UnityEngine;

public class GridMap : EditorWindow
{
    private int[,] maze;
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
            DrawMazeInScene();
        }
    }

    // ===== MAZE GENERATION (DFS chuẩn - đi cách 2 ô) =====
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
                case 0: ny = y + 2; break; // lên
                case 1: nx = x + 2; break; // phải
                case 2: ny = y - 2; break; // xuống
                case 3: nx = x - 2; break; // trái
            }

            if (nx > 0 && ny > 0 && nx < w - 1 && ny < h - 1 && maze[nx, ny] == 1)
            {
                // phá tường giữa
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

    // ===== DRAW TILE GRID =====
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

                // 🎨 màu
                var renderer = tile.GetComponent<Renderer>();
                if (renderer != null)
                {
                    if (maze[x, y] == 1)
                        renderer.material.color = Color.black; // tường
                    else
                        renderer.material.color = new Color(0.3f, 0.2f, 0.8f); // đường
                }
            }
        }
    }
}
