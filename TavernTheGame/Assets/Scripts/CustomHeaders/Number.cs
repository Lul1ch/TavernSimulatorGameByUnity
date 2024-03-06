using System.Collections.Generic;
using UnityEngine;

public class Number
{
    private static Word zero = new Word("ноль", "нуля", "нулю", "ноль", "нулём", "нуле");
    private Dictionary<int, Word> intToWord = new Dictionary<int, Word>() {
        [0] = zero
    };

    /*public static Word GetNumberWord(int number) {
        return intToWord[number];
    }*/
}
