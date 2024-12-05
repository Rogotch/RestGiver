using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    //[SerializeField] private RectTransform responseContainer;
    public void ShowResponses(Response[] responses)
    {
        float responseBoxHeight = 0f;

        foreach (Response response in responses)
        {
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseBox);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response));

            responseBoxHeight += responseButtonTemplate.sizeDelta.y;
        }
        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
    }

    private void OnPickedResponse(Response response)
    {
        Debug.Log(response.ResponseText);
        DialogManager.Instance.SetActiveDialogObject(response.DialogObject);
    }
}