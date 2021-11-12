using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle", menuName = "Obstacle")]
public class Obstacle : ScriptableObject
{
    public Type type;
    public int count = 0;
    public GameObject prefab;
}

public enum Type { Barricade, Barrel, Turret };
