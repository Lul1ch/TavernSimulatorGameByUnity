using UnityEngine;

public class Character : MonoBehaviour
{
    public enum Sex {
        Male, Female
    }
    [SerializeField] private Sex _characterGender;

    public Sex characterGender {
        get { return _characterGender; }
    }
}
