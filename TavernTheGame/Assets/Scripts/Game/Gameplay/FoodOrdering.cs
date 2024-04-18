using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FoodOrdering : MonoBehaviour
{
    [SerializeField] private CharactersVariants _variants;
    [SerializeField] private QueueCreating _queueCreator;
    [SerializeField] private Tavern _tavern;
    [SerializeField] private Kitchen _kitchen;
    [SerializeField] private Shop _shop;
    [SerializeField] private TrainingManager _trainingManager;
    [SerializeField] private CustomTextWriter _textWriter;
    [SerializeField] private Hint _tavernHint;

    public CharactersVariants variants {
        get { return _variants; }
    }
    public QueueCreating queueCreating {
        get { return _queueCreator; }
    }
    public Tavern tavern {
        get { return _tavern; }
    }
    public Shop shop {
        get { return _shop; }
    }
    public TrainingManager trainingManager {
        get { return _trainingManager; }
    }
    public Kitchen kitchen {
        get { return _kitchen; }
    }
    public CustomTextWriter textWriter {
        get { return _textWriter; }
    }
    public Hint tavernHint {
        get { return _tavernHint; }
    }

    private GameObject _curOrder = null;
    private GameObject _curIssue = null;
    private Character _curGuestScript = null;
    private bool _isOrderTold, _isDoublePayChanceBought, _isAutomaticCookingBought;

    public GameObject curOrder {
        get { return _curOrder; }
        set { _curOrder = value; }
    }
    public GameObject curIssue {
        get { return _curIssue; }
        set { _curIssue = value; }
    }
    public bool isOrderTold {
        get { return _isOrderTold; }
        set { _isOrderTold = value; }
    }
    public bool isDoublePayChanceBought {
        get { return _isDoublePayChanceBought; }
        set { _isDoublePayChanceBought = value; }
    }
    public bool isAutomaticCookingBought {
        get { return _isAutomaticCookingBought; }
        set { _isAutomaticCookingBought = value; }
    }

    [Header("GuestMessage")]
    [SerializeField] private Text _messageText;

    public Text message {
        get { return _messageText; }
    }
    public string messageText {
        set { _messageText.text = queueCreating.UpdateAllGenderRelatedWords(value); }
        get { return _messageText.text; }
    }


    public enum Mood {
        Sad = -1, Happy = 1
    }

    public void InitiateServicingProcess() {
        _curGuestScript = queueCreating.curGuest.GetComponent<Character>();
        if (queueCreating.charStatus == QueueCreating.Status.Waiting && _curOrder == null) {
            _curGuestScript.SayHello();
            _curGuestScript.MakeOrder();
        }
    }

    public void EndServicingProcess() {
        if (_curIssue != null && _isOrderTold && queueCreating.charStatus == QueueCreating.Status.Waiting) {
            if (_curGuestScript.React()) {
                _curGuestScript.Pay();
            }
            queueCreating.charStatus = QueueCreating.Status.Serviced;
            EventBus.onGuestReacted?.Invoke();
        }
    }

    public void ClearVariablesValues() {
        _curOrder = null;
        _curIssue = null;
        _isOrderTold = false;
    }
}
