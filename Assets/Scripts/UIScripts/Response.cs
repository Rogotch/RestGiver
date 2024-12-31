using UnityEngine;

[System.Serializable]
public class Response
{
    [SerializeField] private string responseText;
    [SerializeField] private ReplicsLine dialogObject;

    public string ResponseText => responseText;
    public ReplicsLine DialogueObject => dialogObject;
}
