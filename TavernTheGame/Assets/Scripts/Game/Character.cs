using UnityEngine;

public class Character : MonoBehaviour
{
    public enum Sex {
        Male, Female
    }
    [SerializeField] private Sex _characterGender = Sex.Male;
    [SerializeField] private float _xOffset = 0;
    [SerializeField] private float _yOffset = 0;

    public float xOffset {
        get { return _xOffset; }
    }

    public float yOffset {
        get { return _yOffset; }
    }

    public Sex characterGender {
        get { return _characterGender; }
    }
}
