using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

//[Serializable]
//public class DialogSelector : MonoBehaviour
//{
//    [SerializeReference] public BaseDialogSelector selector;

//}
[CustomEditor(typeof(DialogueAction))]
class SelectorOptions : Editor
{
    public override void OnInspectorGUI()
    {
        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.wordWrap = true;
        var dialogSelector = (DialogueAction)target;
        if (dialogSelector == null) return;
        
        Undo.RecordObject(dialogSelector, "Change Dialog selector");

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Common"))
        {
            dialogSelector.selector = new CommonDialogSelector();
        }
        if (GUILayout.Button("Variant"))
        {
            dialogSelector.selector = new VariantsDialogSelector();
        }
        if (GUILayout.Button("Random"))
        {
            dialogSelector.selector = new RandomDialogSelector();
        }
        GUILayout.EndHorizontal();

        if (dialogSelector.selector != null)
        {
            GUILayout.Label(GetSelectorTypeLabel(dialogSelector.selector.GetType()), style);
        }
        DrawDefaultInspector();
        //base.OnInspectorGUI();
    }

    private string GetSelectorTypeLabel(Type selectorType)
    {
        if      (selectorType == typeof(CommonDialogSelector))
        {
            return "Вернёт выбранный диалог, если он подходит по условиям";
        }
        else if (selectorType == typeof(VariantsDialogSelector))
        {
            return "Вернёт первый попавшийся удовлетворительный вариант, либо специальный диалог, если ни один другой не подошёл";
        }
        else if (selectorType == typeof(RandomDialogSelector))
        {
            return "Вернёт случайный вариант";
        }
        else
        {
            return "Пустое значение";
        }
    }
}

[Serializable]
public class BaseDialogSelector
{
    public virtual DialogObject GetSelectedDialog()
    {
        return null;
    }
}

[Serializable]
public class CommonDialogSelector : BaseDialogSelector
{
    [SerializeField] private DialogObject dialog;
    public override DialogObject GetSelectedDialog()
    {
        return dialog;
    }
}

[Serializable]
public class VariantsDialogSelector : BaseDialogSelector
{
    [SerializeField] private DialogObject[] dialogsVariants;
    [SerializeField] private DialogObject falseDialog;
    public override DialogObject GetSelectedDialog()
    {
        return null;
    }
}

[Serializable]
class RandomDialogSelector : BaseDialogSelector
{
    [SerializeField] private DialogObject[] dialogsVariants;
    public override DialogObject GetSelectedDialog()
    {
        return dialogsVariants[UnityEngine.Random.Range(0, dialogsVariants.Length)];
    }
}