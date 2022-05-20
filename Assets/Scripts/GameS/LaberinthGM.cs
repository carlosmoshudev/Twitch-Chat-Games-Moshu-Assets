using UnityEngine;
using System.Collections.Generic;

public class LaberinthGM : MonoBehaviour {
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _parent;
    private const int _wallSize = 10;
    private Dictionary<Vector2Int, Cell> _walls = new Dictionary<Vector2Int, Cell>();

    public void Generate() { }
    public void StartGame() { }
    public void Move(Vector2Int direction) { }

    [ContextMenu("Create")]
    private void CreateWalls() {
        _walls.Clear();
        var delta = _parent.GetComponent<RectTransform>().sizeDelta;
        for (int i = 0; i < delta.x + _wallSize; i += _wallSize) {
            for (int j = 0; j < delta.y + _wallSize; j += _wallSize) {
                Vector2Int pos = new Vector2Int(i, j);
                Cell cell = new Cell();
                cell.status = CellStatus.Wall;
                if (!_walls.ContainsKey(pos)) { _walls.Add(pos, cell); }
            }
        }
        var startPos = new Vector2Int(0, 0);
        var finalPos = new Vector2Int(0, 0);
        while (Vector2.Distance(startPos, finalPos) < Mathf.Sqrt(2) * delta.x * 0.8) {
            startPos = new Vector2Int(
                (int)Random.Range(0, delta.x / _wallSize),
                (int)Random.Range(0, delta.y / _wallSize)) * _wallSize;

            finalPos = new Vector2Int(
                (int)Random.Range(0, delta.x / _wallSize),
                (int)Random.Range(0, delta.y / _wallSize)) * _wallSize;
        }
        _walls[startPos].status = CellStatus.Player;
        _walls[finalPos].status = CellStatus.Goal;

        foreach (KeyValuePair<Vector2Int, Cell> wall in _walls) {
            Debug.Log(startPos + " " + finalPos);
            if (wall.Value.status == CellStatus.Wall) {
                GameObject go = Instantiate(_wallPrefab, _parent.transform);
                go.transform.localPosition = (wall.Key) - _parent.GetComponent<RectTransform>().sizeDelta / 2;
                //TODO: Instanciar el player y el goal
            }
        }
    }
    private Vector2 getRandomPosition(Vector2 delta) {
        return new Vector2(Random.Range(1, (int)delta.x), Random.Range(1, (int)delta.y));
    }

    private Vector2Int RecursiveWallGenerator(Vector2Int chamber, int iterations) 
    {
        var minimumSize = _wallSize * _wallSize;
        if(chamber.x = minimumSize)
        {
            // Generar laberinto en esa chamber
            return new Vector2Int(0, 0);
            Vector2Int cell = new Vector2Int(chamber.x, chamber.y);
            //TODO: Generar mapa de celdas con status 
        }
        else 
        {
            var newChamber = new Vector2Int(chamber.x, chamber.y) / 2;
            RecursiveWallGenerator(newChamber, 4);
        }
        return new Vector2Int(0, 0);

    }

    public enum CellStatus { Path, Wall, Player, Goal }
    public class Cell { public CellStatus status; public Cell() { } }
}