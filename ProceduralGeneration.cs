using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GridMapEditor : EditorWindow
{
    Vector2 offSet;
    Vector2 drag;
    Rect MenuBar;
    private bool drawingGrid;
    private Color wallColor = new Color(0.3f, 0.8f, 0.3f, 0.6f);

    public static int mazeHeight;
    public static int mazeWidth;

    [MenuItem("Tools/Grid Map Editor")]
    public static void showWindow()
    {
       GridMapEditor window = GetWindow<GridMapEditor>();
        window.titleContent = new GUIContent("Grid Map Creator");
    }
    public void OnGUI()
    {
        DrawSettings();
        if (drawingGrid)
        {
            DrawGrid(mazeHeight, mazeWidth);
        }
        //ProcessGrid(Event.current);
        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void DrawSettings()
    {
        mazeWidth = EditorGUILayout.IntField("Width", mazeWidth);
        mazeHeight = EditorGUILayout.IntField("Height", mazeHeight);

        if (GUILayout.Button("Create Grid"))
        {
            Debug.Log("Grid: " + mazeWidth + " x " + mazeHeight);
            drawingGrid = true;
        }
    }
    // xử lý sự kiện chuột để di chuyển lưới
    private void ProcessGrid(Event e)
    {
        drag  = Vector2.zero;
        switch (e.type)
        {
            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnMouseDrag(e.delta);
                }
                break;
        }
        }
    private void OnMouseDrag(Vector2 delta)
    {
        drag = delta;
        GUI.changed = true;
    }
    private void DrawGrid(int height, int width)
    {
        float cellSize = 20;

        Handles.BeginGUI();

        // 🔹 tính center
        float gridWidth = width * cellSize;
        float gridHeight = height * cellSize;

        float startX = (position.width - gridWidth) / 2f;
        float startY = (position.height - gridHeight) / 2f;

        Vector3 origin = new Vector3(startX, startY, 0);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x * cellSize, y * cellSize, 0) + origin;

                Vector3[] square = new Vector3[]
                {
                pos,
                pos + new Vector3(cellSize, 0, 0),
                pos + new Vector3(cellSize, cellSize, 0),
                pos + new Vector3(0, cellSize, 0)
                };

                Handles.DrawSolidRectangleWithOutline(square, wallColor, Color.clear);
            }
        }

        Handles.color = new Color(0, 0, 0, 0.5f);

        for (int x = 0; x <= width; x++)
        {
            Handles.DrawLine(
                new Vector3(x * cellSize, 0, 0) + origin,
                new Vector3(x * cellSize, gridHeight, 0) + origin
            );
        }

        for (int y = 0; y <= height; y++)
        {
            Handles.DrawLine(
                new Vector3(0, y * cellSize, 0) + origin,
                new Vector3(gridWidth, y * cellSize, 0) + origin
            );
        }

        Handles.EndGUI();
    }
}
