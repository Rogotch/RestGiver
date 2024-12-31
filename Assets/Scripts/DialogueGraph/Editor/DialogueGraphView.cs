using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 DefaultNodeSize = new Vector2(150, 200);


    public DialogueGraphView()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("Styles/Editor/DialogueGraph/DialogueGraphStyle"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                if (port.portType == startPort.portType)
                {
                    compatiblePorts.Add(port);
                }
            }
            var portView = port;
        }) ;
        return compatiblePorts;
    }

    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode
        {
            title = "Start",
            GUID = Guid.NewGuid().ToString(),
            Phrase = new DialoguePhrase(),
            EntryPoint = true,
        };

        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";    
        node.outputContainer.Add(generatedPort);

        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;
        node.capabilities &= ~Capabilities.Copiable;

        node.RefreshExpandedState();
        node.RefreshPorts();


        node.SetPosition(new Rect(100, 200, 100, 150));

        return node;
    }
    public void CreateNode(string nodeName)
    {
        AddElement(CreateDialogueNode(nodeName));
    }
    public DialogueNode CreateDialogueNode(string nodeName)
    {
        var dialogueNode = new DialogueNode
        {
            title = nodeName,
            Phrase = new DialoguePhrase(),
            GUID = Guid.NewGuid().ToString(),
        };

        
        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Styles/Editor/DialogueGraph/DialogueNode"));

        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);

        var button = new Button(() => { AddChoicePort(dialogueNode); });
        button.text = "New choice";
        dialogueNode.titleContainer.Add(button);

        var textField = new TextField(string.Empty);
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.Phrase.phraseLine = evt.newValue;
        });
        //textField.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(textField);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

        return dialogueNode;
    }

    public void AddChoicePort(DialogueNode dialogueNode, string overridenPortName = "")
    {
        var generatedPort = GeneratePort(dialogueNode, Direction.Output, Port.Capacity.Multi);
        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);

        var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
        var choicePortname = string.IsNullOrEmpty(overridenPortName)
            ? $"Choice {outputPortCount + 1}"
            : overridenPortName;

        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortname,
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        generatedPort.contentContainer.Add(new Label("||||"));
        //generatedPort.contentContainer.Add(new Label("   "));
        generatedPort.contentContainer.Add(textField);

        Button deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
        {
            text = "x"
        };
        generatedPort.contentContainer.Add(deleteButton);

        generatedPort.portName = choicePortname;
        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshPorts() ;
        dialogueNode.RefreshExpandedState () ;
    }

    private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
    {
        var targetEdge = edges.ToList().Where(x  =>
            x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }


        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }
}
