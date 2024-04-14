using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<Player>();
            }
            return instance;
        }
    }
    public static Vector3 Position
    {
        get
        {
            return Instance.transform.parent.position;
        }
    }
}
