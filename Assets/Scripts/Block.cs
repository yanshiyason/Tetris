using UnityEngine;

public class Block : MonoBehaviour {
    public static Color[] Colors = { Color.blue, Color.red, Color.green, Color.yellow, Color.magenta };

    [SerializeField] private Color _color;

    public Color Color {
        get { return _color; }
        set {
            _color = value;
            gameObject.GetComponent<Renderer> ().material.color = value;
        }
    }
}