using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomTextWriter : MonoBehaviour
{
    private Text uiText;
    private string textToWrite;
    private int characterIndex;
    private float timePerCharacter;
    private float timer;
    private bool invisibleCharacters;
    private Action onComplete;
    private bool needToVoiceIt;
    private IEnumerator writeMessageCoroutine;
    private Queue<CustomTextWriter.Call> messagesQueue = new Queue<CustomTextWriter.Call>();
    [SerializeField] private float timeBeforeNextMessage = 3f;
    [SerializeField] private BoxCollider2D objectCollider;
    private bool isActive = false;

    public void CallMessageWriting(Text uiText, string textToWrite, float timePerCharacter, Action onComplete = null, bool needToVoiceIt = true, bool invisibleCharacters = true) {
        if (!isActive && messagesQueue.Count == 0) {
            StartMessageWriting(uiText, textToWrite, timePerCharacter, onComplete, needToVoiceIt, invisibleCharacters);
        } else {
            messagesQueue.Enqueue(new CustomTextWriter.Call(uiText, textToWrite, timePerCharacter, onComplete, needToVoiceIt, invisibleCharacters));
        }
    }

    private void StartMessageWriting(Text uiText, string textToWrite, float timePerCharacter, Action onComplete, bool needToVoiceIt, bool invisibleCharacters) {
        isActive = true;
        this.uiText = uiText;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        this.onComplete = onComplete;
        this.needToVoiceIt = needToVoiceIt;
        this.invisibleCharacters = invisibleCharacters;
        characterIndex = 0;

        writeMessageCoroutine = WriteText();
        StartCoroutine(writeMessageCoroutine);
    }

    private IEnumerator WriteText() {
        if (needToVoiceIt) {
            VoiceOverText.StartTalkingSound();
        }
        while (characterIndex < textToWrite.Length) {
            timer += timePerCharacter;
            characterIndex++;
            string text = textToWrite.Substring(0, characterIndex);
            if (invisibleCharacters) {
                text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
            }
            uiText.text = text;
            yield return new WaitForSeconds(timePerCharacter);
        }
        InvokeOnStringWrote();
    }

    public void WriteAllAndDestroy() {
        StopCoroutine(writeMessageCoroutine);
        uiText.text = textToWrite;
        characterIndex = textToWrite.Length;
        InvokeOnStringWrote();
    }

    private void InvokeOnStringWrote() {
        VoiceOverText.StopTalkingSound();
        onComplete?.Invoke();
        isActive = false;
        Invoke("MessageWriteFromQueue", timeBeforeNextMessage);
    }

    private void MessageWriteFromQueue() {
        if (!isActive && messagesQueue.Count > 0) {
            CustomTextWriter.Call call = messagesQueue.Dequeue();
            StartMessageWriting(call.uiText, call.textToWrite, call.timePerCharacter, call.onComplete, call.needToVoiceIt, call.invisibleCharacters);
        }
    }

    private void OnMouseDown() {
        if (isActive) {
            WriteAllAndDestroy();
        } else {
            CancelInvoke("MessageWriteFromQueue");
            MessageWriteFromQueue();
        }
    }

    private void OnDisable() {
        objectCollider.enabled = false;
    }

    private void OnEnable() {
        objectCollider.enabled = true;
    }

    public class Call {
        private Text _uiText;
        private string _textToWrite;
        private float _timePerCharacter;
        private Action _onComlete;
        private bool _needToVoiceIt;
        private bool _invisibleCharacters;

        public Text uiText {
            get { return _uiText; }
        }
        public string textToWrite {
            get { return _textToWrite; }
        }
        public float timePerCharacter {
            get { return _timePerCharacter; }
        }
        public Action onComplete {
            get { return _onComlete; }
        }
        public bool needToVoiceIt {
            get { return _needToVoiceIt; }
        }
        public bool invisibleCharacters {
            get { return _invisibleCharacters; }
        }

        public Call(Text uiText, string textToWrite, float timePerCharacter, Action onComplete, bool needToVoiceIt, bool invisibleCharacters) {
            this._uiText = uiText;
            this._textToWrite = textToWrite;
            this._timePerCharacter = timePerCharacter;
            this._onComlete = onComplete;
            this._needToVoiceIt = needToVoiceIt;
            this._invisibleCharacters = invisibleCharacters;
        }
    }
}
