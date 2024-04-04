using UnityEngine;

public class Character : MonoBehaviour
{
    public enum Sex {
        Male, Female
    }
    public enum Type {
        Normal, Unfair
    }
    [SerializeField] private Sex _characterGender = Sex.Male;
    [SerializeField] private Type _characterType = Type.Normal;

    public Sex characterGender {
        get { return _characterGender; }
    }
    public Type characterType {
        get { return _characterType; }
    }
}
