using UnityEngine;

public class InvObj : MonoBehaviour
{
    public new string name;
    public bool unlimitedAmmo = false;
    public int currentAmmo { get; protected set; }

    public virtual void Show(bool show)
    {
        gameObject.SetActive(show); // enable/disable gameObject
    }
}
