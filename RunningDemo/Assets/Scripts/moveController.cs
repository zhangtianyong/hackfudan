using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;
public class moveController : MonoBehaviour {
    // 摄像机位置
    public Transform cameraTransform;
    // 摄像机距离人物的距离
    public float cameraDistance;
    // 游戏管理器
    public GameManager gameManager;
    // 前进移动速度
    float moveVSpeed;
    // 摄像机(游戏者)移动速度
    float cameraVSpeed;
    // 水平移动速度
    public float moveHSpeed = 5.0f;
    // 跳跃高度
    public float jumpHeight = 5.0f;
    // 动画播放器
    Animator m_animator;
    // 起跳时间
    double m_jumpBeginTime;
    // 跳跃标志
    int m_jumpState = 0;
    // 最大速度
    public float maxVSpeed = 10.0f;
    // 最小速度
    public float minVSpeed = 3.0f;
    // 标准小人距离
    private float stdDist = 10.0f;
    // 活动区间
    private float area = 8.0f;

    // Use this for initialization
    
    int port = 23333;
    int remotePort = 32222;
    IPAddress ip = IPAddress.Parse("127.0.0.1");
    IPEndPoint ipaddr;
    IPEndPoint ipRemoteAddr;
    UdpClient client;
    string message = "0";
    void receiveMessage()
    {
        while(true)
        {
            byte[] receiveBytes = client.Receive(ref ipRemoteAddr);
            message = Encoding.Unicode.GetString(receiveBytes);
        }
    }
    void Start () {
        ipaddr = new IPEndPoint(ip, port);
        ipRemoteAddr = new IPEndPoint(ip, remotePort);
        client = new UdpClient(ipaddr);
        Thread receiveThread = new Thread(receiveMessage);
        receiveThread.Start();
        GetComponent<Rigidbody>().freezeRotation = true;
        m_animator = GetComponent<Animator>();
        if (m_animator == null)
            print("null");
        moveVSpeed = cameraVSpeed = 5.0f;
    }
	
 
	// Update is called once per frame
    int count = 0;
	void Update (){
        if (message.Equals("1"))
        {
            if (cameraVSpeed < maxVSpeed)
                cameraVSpeed += 1;
        }
        else if (message.Equals("0"))
        {
            if (this.transform.position.x - cameraTransform.position.x - stdDist > 0.5)
            {
                cameraVSpeed = moveVSpeed + 0.5f;
            }
            else if (this.transform.position.x - cameraTransform.position.x - stdDist < -0.5)
            {
                cameraVSpeed = moveVSpeed - 0.5f;
            }
            else
            {
                cameraVSpeed = moveVSpeed;
            }
        }
        else if (message.Equals("-1"))
        {
            if (cameraVSpeed > minVSpeed)
            {
                cameraVSpeed -= 1;
            }
                
        }
        else
        {
            ;
        }
       

        Debug.Log("distance x = " + (this.transform.position.x - cameraTransform.position.x).ToString());
        Debug.Log("msg = " + message);
        Debug.Log("cameraVSpeed = " + cameraVSpeed.ToString());
        Debug.Log("movespeed = " + moveVSpeed.ToString());
        //if (gameManager.isEnd)
        //{
        //    return;
        //}
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.run"))
        {
            m_jumpState = 0;
            if (Input.GetButton("Jump"))
            {
                // 起跳
                m_animator.SetBool("Jump", true);
                m_jumpBeginTime = m_animator.GetTime();
            }
            else
            {
                // 到地面
            }
        }
        else
        {
            double nowTime = m_animator.GetTime();
            double deltaTime = nowTime - m_jumpBeginTime;

            // 掉下
            m_jumpState = 1;
            m_animator.SetBool("Jump", false);
        }
        float h = Input.GetAxis("Horizontal");
        Vector3 vSpeed = new Vector3(this.transform.forward.x, this.transform.forward.y, this.transform.forward.z) * moveVSpeed ;
        Vector3 hSpeed = new Vector3(this.transform.right.x, this.transform.right.y, this.transform.right.z) * moveHSpeed * h;
        Vector3 jumpSpeed = new Vector3(this.transform.up.x, this.transform.up.y, this.transform.up.z) * jumpHeight * m_jumpState;
        Vector3 vCameraSpeed = new Vector3(this.transform.forward.x, this.transform.forward.y, this.transform.forward.z) * cameraVSpeed;
        this.transform.position += (vSpeed + hSpeed + jumpSpeed) * Time.deltaTime;
        cameraTransform.position += (vCameraSpeed) * Time.deltaTime;

        // 小人不要跑的太远
        if (this.transform.position.x - cameraTransform.position.x > stdDist + area)
        {
            cameraTransform.position = new Vector3(this.transform.position.x - stdDist - area, cameraTransform.position.y, cameraTransform.position.z);
        }
        if (this.transform.position.x - cameraTransform.position.x < stdDist - area)
        {
            cameraTransform.position = new Vector3(this.transform.position.x - stdDist + area, cameraTransform.position.y, cameraTransform.position.z);
        }
        // 当人物与摄像机距离小于cameraDistance时 让其加速
        //if(this.transform.position.x - cameraTransform.position.x < cameraDistance)
        //{
        //    moveVSpeed += 0.1f;
        //    if (moveVSpeed > maxVSpeed)
        //    {
        //        moveVSpeed = maxVSpeed;
        //    }
        //}

        
        //// 摄像机超过人物
        //if(cameraTransform.position.x - this.transform.position.x > 0.0001f)
        //{
        //    Debug.Log("你输啦！！！！！！！！！！");
        //    gameManager.isEnd = true;
        //}
        //cameraTransform.position = new Vector3(this.transform.position.x - cameraDistance, cameraTransform.position.y, cameraTransform.position.z);
    }

    void OnGUI()
    {
        if (gameManager.isEnd)
        {
            GUIStyle style = new GUIStyle();

            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 40;
            style.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "你输了~", style);

        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // 如果是抵达点
        if (other.name.Equals("ArrivePos"))
        {
            gameManager.changeRoad(other.transform);
        }
        // 如果是透明墙
        else if (other.tag.Equals("AlphaWall"))
        {
            // 没啥事情
        }
        // 如果是障碍物
        else if(other.tag.Equals("Obstacle"))
        {

        }
    }
}
