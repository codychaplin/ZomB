using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle", menuName = "Obstacle")]
public class Obstacle : ScriptableObject
{
    public Type type;
    public int count { get; private set; }
    public GameObject prefab;

    public void Init()
    {
        count = 20;
    }

    public void Add(int amount)
    {
        count += amount;
    }

    public void Place()
    {
        count--;
    }
}

public enum Type { Barricade, Barrel, Turret };
