using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;
    private DialogueContainer _containerCache;
    private List<Edge>          Edges => _targetGraphView.edges.ToList();
    private List<BaseGraphNode>  Nodes => _targetGraphView.nodes.ToList().Cast<BaseGraphNode>().ToList();

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }


    public void SaveGraph(string folderName, string fileName)
    {
        if (!Edges.Any()) return;
        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        foreach ( var connectedPort in connectedPorts)
        {
            BaseGraphNode outputNode = connectedPort.output.node as BaseGraphNode;
            BaseGraphNode inputNode  = connectedPort.input.node  as BaseGraphNode;
            dialogueContainer.NodeLinks.Add(new BaseNodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName     = connectedPort.output.portName,
                TargetNodeGuid = inputNode.GUID,
            });
        }

        foreach ( var graphNode in Nodes.Where(node => !node.EntryPoint))
        {
            if (graphNode.GetType() == typeof(DialogueNode))
            {
                DialogueNode dialogueNode = (DialogueNode)graphNode;
                dialogueContainer.NodeData.Add(new DialogueNodeData
                {
                    Guid = dialogueNode.GUID,
                    Phrase = dialogueNode.Phrase,
                    Position = dialogueNode.GetPosition().position
                });
            }
            else if (graphNode.GetType() == typeof(CharacterNodeData))
            {
                dialogueContainer.NodeData.Add(new CharacterNodeData
                {
                    Guid = graphNode.GUID,
                    Position = graphNode.GetPosition().position
                });
            }
        }


        ValidatePath($"Assets/Resources/Dialogs/{folderName}");
        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/Dialogs/{folderName}/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    private void ValidatePath(string path)
    {
        string[] splittedPath = path.Split('/');
        string checkedPath = "";
        foreach ( string folder_name in splittedPath)
        {
            if (!AssetDatabase.IsValidFolder(checkedPath + "/" + folder_name))
            {
                var testt = AssetDatabase.CreateFolder(checkedPath, folder_name);
                //Debug.Log($"{checkedPath}   {folder_name}");
                //Debug.Log(testt);
            }
            else
            {
                //Debug.Log($"path {checkedPath + folder_name} is valid!");
            }

            if (folder_name != "Assets")
            {
                checkedPath += "/" + folder_name;
            }
            else
            {
                checkedPath = "Assets";
            }
        }
    }
    public void LoadGraph(string folderName, string fileName) 
    {
        string filePath = $"Assets/Resources/Dialogs/{folderName}/{fileName}.asset";
        _containerCache = AssetDatabase.LoadAssetAtPath<DialogueContainer>(filePath);
        //AssetDatabase.LoadAssetAtPath<DialogueContainer>(filePath);
        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File not found", $"Target dialogue graph by path {filePath} file does not exist!", "Ok");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ConnectNodes()
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();
            for (int j = 0; j < connections.Count; j++)
            {
                var targetNodeGUID = connections[j].TargetNodeGuid;
                var targetNode = Nodes.First(x => x.GUID == targetNodeGUID);
                LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                targetNode.SetPosition(
                    new Rect(_containerCache.NodeData.First(x => x.Guid == targetNodeGUID).Position,
                    _targetGraphView.DefaultNodeSize
                    ));
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            output = output,
            input = input,
        };
        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);
    }

    private void CreateNodes()
    {
        int counter = 0;
        foreach (BaseNodeData nodeData in _containerCache.NodeData)
        {
            var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            BaseGraphNode temporaryNode;
            if (nodeData.GetType() == typeof(DialogueNodeData))
            {
                temporaryNode = _targetGraphView.CreateDialogueNode($"Node{counter}");
                temporaryNode.GUID = nodeData.Guid;

                _targetGraphView.AddElement(temporaryNode);

                nodePorts.ForEach(x => _targetGraphView.AddChoicePort((DialogueNode)temporaryNode, x.PortName));

            }
            counter++;
        }
    }

    private void ClearGraph()
    {
        Nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;

        foreach (DialogueNode node in Nodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList()
                .ForEach(edge => _targetGraphView.RemoveElement(edge));

            _targetGraphView.RemoveElement(node);
        }
    }
}
