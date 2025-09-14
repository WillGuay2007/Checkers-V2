using Unity.VisualScripting;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    protected string _Name;

    public abstract void MakeMove();

}