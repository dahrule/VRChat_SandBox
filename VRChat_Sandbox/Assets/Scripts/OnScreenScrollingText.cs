
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Creates the effect of horizontally scrolling text on the screen by continuously creating a new version of the text message and displaying only part of it.
/// !Set Synchronization Method to Manual in Editor.
/// </summary>
public class OnScreenScrollingText : UdonSharpBehaviour
{
    [SerializeField] Text messageToDisplay;

    [Tooltip("If a portal is using the screen, the variable will be set by the portal")]
    public string originalMessage;

    [Tooltip("Time in seconds the screen takes to refresh, creating the text moving effect")]
    [SerializeField] float refreshTime = 0.2f;

    [Tooltip("Characters of the original message that the screen displays each refresh time.")]
    [SerializeField] int screenSize=25;

    private float lastTimestamp;

    private void Start()
    {
        //Avoid errors with an empty message or shorter than the screensize.
        if (originalMessage.Length==0) originalMessage = " ";
        screenSize = Mathf.Clamp(screenSize,0, originalMessage.Length);
    }
    private void Update()
    {
        //Repeat the body of the function each timerSpeed seconds to animate the sliding text.
        if (Time.time - lastTimestamp >= refreshTime)
        {
            lastTimestamp = Time.time;

            string newMessage = CreateNewMessage(originalMessage);
            originalMessage = newMessage;

            messageToDisplay.text = FitMessageToScreen(newMessage, screenSize);
        }
    }

    private string CreateNewMessage(string message)
    {
        /*Moves each character in message to the right by one step; last  char becomes first.
         * Example: danielruiz -> zdanielrui
        */
        string firstPiece = message.Substring(0, message.Length - 1); //Get all chars but last from message.
        string lastPiece = message.Substring(message.Length - 1, 1); //Get last char from message.
        string newMessage = lastPiece + firstPiece; //Create new message from pieces.

        return newMessage;
    }

    private string FitMessageToScreen(string message, int screenSize)
    {
        /*Creates a substring of the message to make it fit to the desired screen size. 
         * Together with CreateNewMesage method and the Update function they create the effect of sliding text.
        */
        return message.Substring(0, screenSize);
    }

    //TODELETE: NotUsed
    private char[] StringToArray(string message)
    {
        char[] newMessage = new char[message.Length];

        int i = 0;
        foreach (char c in message)
        {
            newMessage[i] = c;
            i++;
        }
        return newMessage;
    }
}
