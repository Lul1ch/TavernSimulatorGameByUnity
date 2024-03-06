using UnityEngine;

public class Word 
{
    private string _primalForm;

    public Word(string _primalForm) {
        this._primalForm = _primalForm;
    }

    public Word(string _primalForm, string _roditelniyPadej, string _datelniyPadej, string _vinitelniyPadej, string _tvoritelniyPadej, string _predlojniyPadej) {
        this._primalForm = _primalForm;
        this._roditelniyPadej = _roditelniyPadej;
        this._datelniyPadej = _datelniyPadej;
        this._vinitelniyPadej = _vinitelniyPadej;
        this._tvoritelniyPadej = _tvoritelniyPadej;
        this._predlojniyPadej = _predlojniyPadej;
    }

    private string _roditelniyPadej, _datelniyPadej, _vinitelniyPadej, _tvoritelniyPadej, _predlojniyPadej;

    public string primalForm {
        get { return _primalForm; }
    }
    public string roditelniyPadej {
        get { return _roditelniyPadej; }
    }
    public string datelniyPadej {
        get { return _datelniyPadej; }
    }
    public string vinitelniyPadej {
        get { return _vinitelniyPadej; }
    }
    public string tvoritelniyPadej {
        get { return _tvoritelniyPadej; }
    }
    public string predlojniyPadej {
        get { return _predlojniyPadej; }
    }
}
