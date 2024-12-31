using System;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class DialogueGraph : EditorWindow
{

    private DialogueGraphView _graphView;
    private string _fileName = "New Narrative";
    private string _directoryName = "NewNarrativeFolder";
    private TextField directoryNameTextField;
    DropdownField directorySelector;

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("DialogueGraph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
        GenerateMinimap();
    }

    private void GenerateMinimap()
    {
        var minimap = new MiniMap {anchored = true};
        minimap.SetPosition(new Rect(10, 30, 200, 140));
        _graphView.Add(minimap);
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView
        {
            name = "Dialogue Graph"
        };
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolbar()
    {
        Toolbar toolbar = new Toolbar();
        //RadioButtonGroup radioButtonGroup = new RadioButtonGroup();
        //Debug.Log(AssetDatabase.GetAllAssetPaths());

        directorySelector = new DropdownField();
        SetFolderVariants();

        Label folderNameLabel = new Label("Folder Name:");
        toolbar.Add(folderNameLabel);
        toolbar.Add(directorySelector);
        directoryNameTextField = new TextField();
        directoryNameTextField.SetValueWithoutNotify(_directoryName);
        directoryNameTextField.MarkDirtyRepaint();
        directoryNameTextField.RegisterValueChangedCallback(evt => _directoryName = evt.newValue);
        directoryNameTextField.visible = false;
        toolbar.Add(directoryNameTextField);

        TextField fileNameTextField = new TextField("File Name:");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });

        Button nodeCreationButton = new Button(() => { _graphView.CreateNode("Dialogue Node"); });
        nodeCreationButton.text = "Create Node";
        toolbar.Add(nodeCreationButton);

        rootVisualElement.Add(toolbar);
    }

    private void RequestDataOperation(bool saveFlag)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "Ok");
        }
        GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(_graphView);
        Debug.Log($"{_directoryName}, {_fileName}");
        if (saveFlag)
        {
            saveUtility.SaveGraph(_directoryName, _fileName);
        }
        else
        {
            saveUtility.LoadGraph(_directoryName, _fileName);
        }
        SetFolderVariants();
    }

    public void SetFolderVariants()
    {
        directorySelector.choices.Clear();
        directorySelector.RegisterCallback<ChangeEvent<string>>((evt) => SelectFolderPathInDropdown(evt, directorySelector.index));
        var info = new DirectoryInfo($"Assets/Resources/Dialogs/");
        var derictoriesInfo = info.GetDirectories();
        directorySelector.choices.Add("New folder");
        foreach (DirectoryInfo directory in derictoriesInfo)
        {
            directorySelector.choices.Add(directory.Name);
        }
    }

    private void SelectFolderPathInDropdown(ChangeEvent<string> evt, int index)
    {
        //Debug.Log($"{evt.newValue}, {index}, {System.IO.Directory.Exists($"Assets/Resources/Dialogs/{evt.newValue}")}");
        if (index == 0)
        {
            directoryNameTextField.visible = true;
            //directoryNameTextField.maxLength = 128;
        }
        else
        {
            directoryNameTextField.visible = false;
            //directoryNameTextField.maxLength = 0;
        }
        directoryNameTextField.SetValueWithoutNotify(evt.newValue);
        directoryNameTextField.MarkDirtyRepaint();
        _directoryName = evt.newValue;
    }
}
