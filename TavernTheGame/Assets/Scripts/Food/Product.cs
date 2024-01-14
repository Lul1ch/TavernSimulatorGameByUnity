using UnityEngine;

public class Product : Food
{
    [Header("Product Variables")]
    [SerializeField] private Sprite _productSprite;
    private int _productIndex;

    public Sprite productSprite {
        get { return _productSprite; }
        set { _productSprite = value; }
    }

    public int productIndex {
        get { return _productIndex; }
        set { _productIndex = value; }
    }
}
