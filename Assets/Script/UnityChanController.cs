using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour
{
    //アニメーションをするためのコンポーネントを入れる
    private Animator myAnimator;
    //Unityちゃんを移動させるためのコンポーネントを入れる
    private Rigidbody myRigidbody;
    //前方向の速度(追加1)
    private float velocityZ = 16f;
    //横方向の速度(追加２)
    private float velocityX = 10f;
    //上方向の速度(追加３)
    private float velocityY = 10f;
    //左右の移動できる範囲(追加２)
    private float movableRange = 3.4f;
    //動きを減速させる係数(追加４)
    private float coefficient = 0.99f;
    //ゲーム終了の判定(追加４)
    private bool isEnd = false;
    //ゲーム終了時に表示するテキスト(追加６)
    private GameObject stateText;
    //スコアを表示するテキスト(追加７)
    private GameObject scoreText;
    //得点(追加７)
    private int score = 0;
    //左ボタン押下の判定(追加８)
    private bool isLButtonDown = false;
    //右ボタン押下の判定(追加８)
    private bool isRButtonDown = false;
    //ジャンプボタン押下の判定(追加８)
    private bool isJButtonDown = false;

    // Start is called before the first frame update
    void Start()
    {
        //Animationコンポーネントを取得
        this.myAnimator = GetComponent<Animator>();

        //走るアニメーションを開始
        this.myAnimator.SetFloat("Speed", 1);

        //Rigidbodyコンポーネントを取得(追加1)
        this.myRigidbody = GetComponent<Rigidbody>();

        //シーン中のstateTextオブジェクトを取得(追加６）
       　this.stateText = GameObject.Find("GameResultText");

        //シーン中のscoreTextオブジェクトを取得(追加７)
        this.scoreText = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム終了ならUnityちゃんの動きを減衰する(追加４)
        if (this.isEnd)
        {
            this.velocityZ *= this.coefficient;
            this.velocityX *= this.coefficient;
            this.velocityY *= this.coefficient;
            this.myAnimator.speed *= this.coefficient;
        }

        //横方向の入力による速度(追加２）
        float inputVelocityX = 0;

        //上方向の入力による速度(追加３）
        float inputvelocityY = 0;


        //Unityちゃんを左右キーまたはボタンに応じて左右に移動させる(追加２ , 8)
        if ((Input.GetKey (KeyCode.LeftArrow) || this.isLButtonDown)  &&  -this.movableRange < this.transform.position.x)
        {
            //左方向への速度を代入(追加２)
            inputVelocityX = -this.velocityX;
        }
        else if ((Input.GetKey(KeyCode.RightArrow) || this.isRButtonDown) && this.transform.position.x < movableRange)
        {
            //右方向への速度を代入(追加２)
            inputVelocityX = this.velocityX;
        }

        //ジャンプしていないときにスペースが押されたらジャンプする(追加３)
        if ((Input.GetKeyDown(KeyCode.Space) || this.isJButtonDown) && this.transform.position.y < 0.5f)
        {
            //ジャンプアニメを再生(追加３)
            this.myAnimator.SetBool("Jump", true);

            //上方向への速度を代入(追加３)
            inputvelocityY = this.velocityY;
        }
        else
        {
            //現在のY軸の速度を代入(追加３)
            inputvelocityY = this.myRigidbody.velocity.y;
        }

        //Jumpステートの場合はJumpにfalseをセットする(追加3)
        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            this.myAnimator.SetBool("Jump", false);
        }

        //Unityちゃんに速度を与える(追加1 + 変更 +変更) 
        this.myRigidbody.velocity = new Vector3(inputVelocityX, inputvelocityY, this.velocityZ);
    }

    //トリガーモードで他のオブジェクトと接触した場合の処理(追加４)
    void OnTriggerEnter(Collider other)
    {
        //障害物に衝突した場合(追加４)
        if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag")
        {
            this.isEnd = true;
            //stateTextにGAME OVERを表示(追加６)
            this.stateText.GetComponent<Text>().text = "GAME OVER";
        }
        //ゴール地点に到達した場合
        if (other.gameObject.tag == "GoalTag")
        {
            this.isEnd = true;
            //stateTextにGAME CLEARを表示
            this.stateText.GetComponent<Text>().text = "GAME CLEAR";
        }
        //コインに衝突した場合(追加４)
        if (other.gameObject.tag == "CoinTag")
        {
            //スコアを加算(追加７)
            this.score += 10;

            //ScoreText獲得した点数を表示(追加７)
            this.scoreText.GetComponent<Text>().text = "Score" + this.score + "pt"; 

            //パーティクルを再生(追加５)
            GetComponent<ParticleSystem>().Play();

            //接触したコインのオブジェクトを破棄(追加４)
            Destroy(other.gameObject);
        }
    }
    //ジャンプボタンを押した場合の処理(追加８)
    public void GetMyJumpButtonDown()
    {
        this.isJButtonDown = true;
    }
    //ジャンプボタンを話した場合の処理(追加８)
    public void GetMyJumpButtonUp()
    {
        this.isJButtonDown = false;
    }
    //左ボタンを押し続けた場合の処理(追加８)
    public void GetMyLButtonDown()
    {
        this.isLButtonDown = true;
    }
    //左ボタンを離した場合の処理(追加８)
    public void GetMyLButtonUp()
    {
        this.isLButtonDown = false;
    }
    //右ボタンを押し続けた場合の処理(追加８)
    public void GetMyRButtonDown()
    {
        this.isRButtonDown = true;
    }
    //右ボタンを離した場合の処理(追加８)
    public void GetMyRButtonUp()
    {
        this.isRButtonDown = false;
    }
}
