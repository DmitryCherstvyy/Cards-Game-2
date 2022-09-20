using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public abstract class ChangeableValue<T>  where T : struct
{
    public T Value
    {
        get => m_SavedValue.Value;
        set
        {
            if(m_SavedValue.HasValue)
                if (value.Equals(m_SavedValue)) return;
            
            if (!m_SavedValue.HasValue)
                displayText.text = value.ToString();
            else
                InterpolateChanges(m_SavedValue.Value, value);

            m_SavedValue = value;
            OnValueChanged?.Invoke();
        }
    }

    T? m_SavedValue;

    [SerializeField] protected TextMeshProUGUI displayText;

    public event Action OnValueChanged;

    protected abstract void InterpolateChanges(T previous,T current);
}

[Serializable]
public class ChangeableIntValue : ChangeableValue<int>
{
    protected override async void InterpolateChanges(int previous,int current)
    {
        while (previous > current)
        {
            previous--;
            displayText.text = previous.ToString();
            await UniTask.Delay(100);
        }
        while(previous<current)
        {
            previous++;
            displayText.text = previous.ToString();
            await UniTask.Delay(100);
        }
    }
}