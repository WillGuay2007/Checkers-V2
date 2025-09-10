using Unity.VisualScripting;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    protected string _Name;

    protected abstract void MakeMove();

}

public class RealPlayer : Player
{
    protected override void MakeMove()
    {

    }
}

public class AI : Player
{
    protected override void MakeMove()
    {
        
    }
}
