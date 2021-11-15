using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle", menuName = "Obstacle")]
public class Obstacle : ScriptableObject
{
    public Type type;
    public int count = 0;
    public GameObject prefab;

    public void Add(int amount)
    {
        count += amount;
        Debug.Log("Added " + amount + " ammo to " + type);
    }
}

public enum Type { Barricade, Barrel, Turret };
