using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Chat : NetworkBehaviour
{
    const short chatMsg = MsgType.Highest + 1;

    private SyncListString chatLog = new SyncListString();

    public static NetworkClient myClient;
    private GameObject textArea;
    private bool active;
    [SerializeField]
    InputField
        chatInput;
    [SerializeField]
    Text
        chatWindow;
    [SerializeField]
    string username;
    [SerializeField]
    GameObject chatFieldObject;
    bool chatFieldActive;
    public void SetName(string _name) { username = _name; }
    [SerializeField]
    float Timer;


    //	 Use this for initialization
    void Start()
    {
        textArea = GameObject.Find("Canvas");
        textArea.SetActive(false);
        active = true;
        chatLog.Callback = OnChatUpdated;
        //setup text boxes
        chatWindow.text = "";
        Timer = 0;
        NetworkServer.RegisterHandler(chatMsg, OnServerPostChatMessage);
        chatFieldActive = false;
        chatInput.onEndEdit.AddListener(delegate
        {
            PostChatMessage(chatInput.text);
        });
        //SetName(GameObject.Find("NetManager").GetComponent<MyNetworkManager>().GetUsername());
    }
    void Update()
    {

    }
    public override void OnStartClient()
    {
        //Callback is the delegate type used for SyncListChanged.
        chatLog.Callback = OnChatUpdated;
    }
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (active)
            {
                textArea.SetActive(true);
                active = false;
            }
            chatFieldActive = !chatFieldActive;
            chatFieldObject.SetActive(chatFieldActive);
            if (chatFieldActive) chatInput.Select();
            Timer = 0;
        }
        if (active == false)
        {
            Timer += Time.fixedDeltaTime;
        }
        if (Timer > 5)
        {
            active = true;
            textArea.SetActive(false);
            Timer = 0;
        }
    }
    /*
	 * [Server] 
	 * A Custom Attribute that can be added to member functions of NetworkBehaviour scripts, 
	 * to make them only run on servers.
	 * 
	 * A [Server] method returns immediately if NetworkServer.active is not true, 
	 * and generates a warning on the console. This attribute can be put on member 
	 * functions that are meant to be only called on server. This would redundant for 
	 * [Command] functions, as being server-only is already enforced for them.
	 */
    [Server]
    void OnServerPostChatMessage(NetworkMessage netMsg)
    {
        string message = netMsg.ReadMessage<StringMessage>().value;
        chatLog.Add(message);
    }
    /*
     * [Client]
     * makes a NetworkBehaviour script only run on clients.
     * 
     * A [Client] method returns immediately if NetworkClient.active is not true, 
     * and generates a warning on the console. This attribute can be put on member 
     * functions that are meant to be only called on clients. This would redundant 
     * for [ClientRPC] functions, as being client-only is already enforced for them.
     */
    [Client]
    public void PostChatMessage(string message)
    {
        if (message.Length == 0)
            return;
        var msg = new StringMessage(username + " : " + message);
        NetworkManager.singleton.client.Send(chatMsg, msg);

        chatInput.text = "";


    }

    //callback we registered for when the syncList changes
    private void OnChatUpdated(SyncListString.Operation op, int index)
    {
        chatWindow.text += chatLog[chatLog.Count - 1] + "\n";
    }
}
