using UnityEngine;

public abstract class GameItem : MonoBehaviour
{
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
