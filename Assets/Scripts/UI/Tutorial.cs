using System.Collections;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string[] _lines;
    [SerializeField] private float _textSpeed;
    [SerializeField] private float _delayBetweenLines;

    private int _index;

    private void Start()
    {
        _text.text = string.Empty;
        StartSpeech();
    }

    private void StartSpeech()
    {
        _index = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (char letter in _lines[_index].ToCharArray())
        {
            _text.text += letter;
            yield return new WaitForSeconds(_textSpeed);
        }

        yield return new WaitForSeconds(_delayBetweenLines);

        StartNextLine();
    }

    private void StartNextLine()
    {
        if(_index < _lines.Length - 1)
        {
            _index++;
            _text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
