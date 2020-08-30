//using UnityEngine;
//using System.Collections.Generic;
//using Windows.Kinect;

//public struct Direction
//{
//    public const float Left = -1;
//    public const float Right = 1;
//}

//public class Owl : MonoBehaviour
//{
//    public Transform backgroundLayer_transform = null;
//    public Transform charaterLayer_transform = null;

//    public int index = 0;
//    public GameObject body_gameObject = null;
//    public GameObject leftWing_gameObject = null;
//    public GameObject rightWing_gameObject = null;
//    private float bodyAngle = 0.0F;
//    private float leftWingAngle = 0.0F;
//    private float rightWingAngle = 0.0F;
//    private float lastLeftWingAngle = 0.0F;
//    private float lastRightWingAngle = 0.0F;

//    private GameObject heathBar_gameObject = null;
//    private HeathBar heathBar_script = null;
//    private Floater heathBar_floater = null;

//    private Transform topDirection_transform = null;
//    private Vector3 velocity = Vector3.zero;
//    private bool isDead = false;

//    public event EventHandler<bool> onDie = null;

//    public float BodyAngle
//    {
//        set
//        {
//            this.body_gameObject.transform.localRotation = Quaternion.AngleAxis(-value, Vector3.forward);
//        }
//        get
//        {
//            float angle;
//            Vector3 axis;
//            this.transform.localRotation.ToAngleAxis(out angle, out axis);
//            axis *= angle;
//            return -axis.z;
//        }
        
//    }
//    public float LeftWingAngle
//    {
//        set
//        {
//            this.leftWing_gameObject.transform.localRotation = Quaternion.AngleAxis(value * Direction.Left, Vector3.forward);
//            Rigidbody2D rigidbody = CacheManager.Instance.Get<Rigidbody2D>(this.leftWing_gameObject);
//            rigidbody.velocity = rigidbody.velocity * 0.5F;
//        }
//        get
//        {
//            float angle;
//            Vector3 axis;
//            this.leftWing_gameObject.transform.localRotation.ToAngleAxis(out angle, out axis);
//            axis *= angle;
//            return axis.z * Direction.Left;
//        }
//    }
//    public float RightWingAngle
//    {
//        set
//        {
//            this.rightWing_gameObject.transform.localRotation = Quaternion.AngleAxis(value * Direction.Right, Vector3.forward);
//            Rigidbody2D rigidbody = CacheManager.Instance.Get<Rigidbody2D>(this.rightWing_gameObject);
//            rigidbody.velocity = rigidbody.velocity * 0.5F;
//        }
//        get
//        {
//            float angle;
//            Vector3 axis;
//            this.rightWing_gameObject.transform.localRotation.ToAngleAxis(out angle, out axis);
//            axis *= angle;
//            return axis.z * Direction.Right;
//        }
//    }

//    public Vector2 TopDirection
//    {
//        get
//        {
//            return this.topDirection_transform.position - this.gameObject.transform.position;
//        }
//    }

//    public Vector3 Velocity
//    {
//        set
//        {
//            this.velocity = CacheManager.Instance.Get<Rigidbody2D>(this.gameObject).velocity = value;
//        }
//        get
//        {
//            return this.velocity;
//        }
//    }

//    private void Awake()
//    {
//        this.heathBar_gameObject = Instantiate(CacheManager.Instance.Get<GameObject>("Prefabs/Guis/HeathBar"));
//        this.heathBar_script = CacheManager.Instance.Get<HeathBar>(heathBar_gameObject);
//        this.heathBar_script.maxHeath = 100;
//        this.heathBar_script.OnChangedHeath += this.OnChangedHeath;
//        this.heathBar_floater = CacheManager.Instance.Get<Floater>(heathBar_gameObject);
//        this.heathBar_floater.parent_transform = this.gameObject.transform;
//        this.heathBar_floater.localPosition = new Vector2(-2, 2);

//        this.topDirection_transform = new GameObject("Top").transform;
//        this.topDirection_transform.SetParent(this.gameObject.transform);
//        this.topDirection_transform.localPosition = Vector3.up;

//        if (GameManager.Instance.KinectManager_script)
//            GameManager.Instance.KinectManager_script.OnBodyRecived += this.OnBodyRecived;
//        if (GameManager.Instance.TouchManager_script)
//        {
//            GameManager.Instance.TouchManager_script.OnTouchBegin += this.OnTouchBegin;
//            GameManager.Instance.TouchManager_script.OnTouchEnded += this.OnTouchEnded;
//        }
//        if(GameManager.Instance.GyroscopeManager_script)
//            GameManager.Instance.GyroscopeManager_script.OnGyroArrived += this.OnGyroArrived;

//        this.onDie += this.OnDie;
//    }

//    private void Update()
//    {
//        if (index != 0)
//        {
//            Vector3 value = CacheManager.Instance.Get<AIPlayer>(this.gameObject).Active();
//            this.leftWingAngle = value.x;
//            this.bodyAngle = value.y;
//            this.rightWingAngle = value.z;
//        }

//        if (Input.GetKeyDown(KeyCode.Space))
//            this.OnTouchBegin(new TouchManager.TouchData(0, Vector2.zero));
//        if (Input.GetKeyUp(KeyCode.Space))
//            this.OnTouchEnded(new TouchManager.TouchData(0, Vector2.zero));

//        if (index == 0)
//        {
//            if (Input.GetKey(KeyCode.LeftArrow))
//                bodyAngle += Time.deltaTime * 100 * Direction.Left;
//            if (Input.GetKey(KeyCode.RightArrow))
//                bodyAngle += Time.deltaTime * 100 * Direction.Right;
//        }

//        this.LeftWingAngle += (this.leftWingAngle - this.LeftWingAngle) * 0.5F;
//        this.RightWingAngle += (this.rightWingAngle - this.RightWingAngle) * 0.5F;
//        this.BodyAngle += (this.bodyAngle - this.BodyAngle) * 0.5F;

//        float middleForce = 0;
//        {
//            float distance = this.lastLeftWingAngle - this.leftWingAngle;
//            middleForce += Mathf.Max(0, distance);
//            this.lastLeftWingAngle = this.leftWingAngle;
//        }
//        {
//            float distance = this.lastRightWingAngle - this.rightWingAngle;
//            middleForce += Mathf.Max(0, distance);
//            this.lastRightWingAngle = this.rightWingAngle;
//        }
//        CacheManager.Instance.Get<Rigidbody2D>(this.gameObject).AddForce(this.TopDirection * middleForce * 150.0F);
//    }

//    private void FixedUpdate()
//    {
//        this.velocity = CacheManager.Instance.Get<Rigidbody2D>(this.gameObject).velocity;
//    }

//    private void OnTriggerEnter2D(Collider2D collider)
//    {
//        if (this.isDead)
//            return;

//        if (collider.CompareTag(Tag.Owl))
//        {
//            Vector2 otherBounce = (collider.gameObject.transform.position - this.gameObject.transform.position).normalized;
//            CacheManager.Instance.Get<Owl>(collider.gameObject).Velocity = otherBounce * 25.0F;
//            collider.gameObject.transform.Translate(otherBounce * 0.1F);
//        }
//    }

//    public void SetBodyAngle(float angle)
//    {
//        this.bodyAngle = angle;
//    }
//    public void SetLeftWingAngle(float angle)
//    {
//        this.leftWingAngle = angle;
//    }
//    public void SetRightWIngAngle(float angle)
//    {
//        this.rightWingAngle = angle;
//    }

//    public void OnDamage(float damage)
//    {
//        if (this.isDead)
//            return;

//        this.heathBar_script.Heath -= damage;
//    }

//    private void OnDie(bool isDie)
//    {
//        this.isDead = isDie;

//        if (this.isDead)
//        {
//            CacheManager.Instance.Get<SpriteRenderer>(this.body_gameObject).color = new Color(1.0F, 1.0F, 1.0F, 0.0F);
//            CacheManager.Instance.Get<SpriteRenderer>(this.leftWing_gameObject).color = new Color(1.0F, 1.0F, 1.0F, 0.0F);
//            CacheManager.Instance.Get<SpriteRenderer>(this.rightWing_gameObject).color = new Color(1.0F, 1.0F, 1.0F, 0.0F);

//            this.gameObject.transform.SetParent(this.backgroundLayer_transform);
//            this.gameObject.transform.localPosition = VectorUtil.GetVector2FromVector3(this.gameObject.transform.localPosition);

//            this.enabled = false;
//        }
//        else
//        {
//            CacheManager.Instance.Get<SpriteRenderer>(this.body_gameObject).color = Color.white;
//            CacheManager.Instance.Get<SpriteRenderer>(this.leftWing_gameObject).color = Color.white;
//            CacheManager.Instance.Get<SpriteRenderer>(this.rightWing_gameObject).color = Color.white;

//            this.gameObject.transform.SetParent(this.backgroundLayer_transform);
//            this.gameObject.transform.localPosition = VectorUtil.GetVector2FromVector3(this.gameObject.transform.localPosition);

//            this.enabled = true;
//        }
//    }

//    private void OnChangedHeath(HeathBar.HeathData heathData)
//    {
//        if (this.isDead)
//            return;

//        int heathDistance = (int)heathData.lastHeath - (int)heathData.heath;
//        CacheManager.Instance.Get<Crash>(this.gameObject).SpawnStarEffect(heathDistance);
//        if (heathData.heath <= 0.0F && this.onDie != null)
//            this.onDie(true);
//    }

//    private void OnBodyRecived(KinectManager.BodyData bodyData)
//    {
//        if (this.isDead)
//            return;

//        if (this.index == bodyData.index)
//        {
//            Dictionary<JointType, Windows.Kinect.Joint> joints = bodyData.body.Joints;
//            if (joints[JointType.HandLeft].TrackingState == TrackingState.Tracked && joints[JointType.HandRight].TrackingState == TrackingState.Tracked)
//            {
//                Vector2 leftHandPosition = VectorUtil.GetVector2FromJoint(joints[JointType.HandLeft]);
//                Vector2 rightHandPosition = VectorUtil.GetVector2FromJoint(joints[JointType.HandRight]);
//                if (joints[JointType.Head].TrackingState == TrackingState.Tracked && joints[JointType.SpineMid].TrackingState == TrackingState.Tracked)
//                {
//                    Vector2 bodyDirection = VectorUtil.GetVector2FromJoint(joints[JointType.Head]) - VectorUtil.GetVector2FromJoint(joints[JointType.SpineMid]);
//                    this.SetBodyAngle(bodyDirection.normalized.x * 150.0F);
//                }
//                if (joints[JointType.SpineShoulder].TrackingState == TrackingState.Tracked)
//                {
//                    Vector2 spineShoulder = VectorUtil.GetVector2FromJoint(joints[JointType.SpineShoulder]);
//                    Vector2 leftWingDirection = leftHandPosition - spineShoulder;
//                    Vector2 rightWingDirection = rightHandPosition - spineShoulder;
//                    this.SetLeftWingAngle(leftWingDirection.normalized.y * 50.0F);
//                    this.SetRightWIngAngle(rightWingDirection.normalized.y * 50.0F);
//                }
//            }
//        }
//    }

//    private void OnTouchBegin(TouchManager.TouchData touchData)
//    {
//        if (this.isDead)
//            return;

//        if (index == 0)
//        {
//            this.SetLeftWingAngle(-10.0F);
//            this.SetRightWIngAngle(-10.0F);
//        }
//    }
//    private void OnTouchEnded(TouchManager.TouchData touchData)
//    {
//        if (this.isDead)
//            return;

//        if (index == 0)
//        {
//            this.SetLeftWingAngle(10.0F);
//            this.SetRightWIngAngle(10.0F);
//        }
//    }

//    private void OnGyroArrived(Vector3 euler)
//    {
//        if (index == 0)
//            this.SetBodyAngle(euler.x * 90.0F);
//    }
//    public bool checkPlayer()
//    {
//        if(this.index==0)
//            return true;
//        else
//            return false;
//    }
//}

///*
//protected void Update()
//{
//    Body body = null;
//    if (body != null)
//    {
//        Dictionary<JointType, Windows.Kinect.Joint> joints = body.Joints;

//        Vector2 leftHandPosition = VectorUtil.GetVector2FromJoint(joints[JointType.HandRight]);
//        Vector2 rightHandPosition = VectorUtil.GetVector2FromJoint(joints[JointType.HandLeft]);
//        {
//            Vector2 bodyDirection = VectorUtil.GetVector2FromJoint(joints[JointType.Head]) - VectorUtil.GetVector2FromJoint(joints[JointType.SpineMid]);
//            this.transform.localRotation = Quaternion.AngleAxis(bodyDirection.normalized.x * -150.0F, Vector3.forward);
//        }
//        {
//            Vector2 leftWingDirection = leftHandPosition - VectorUtil.GetVector2FromJoint(joints[JointType.SpineShoulder]);
//            Vector2 rightWingDirection = rightHandPosition - VectorUtil.GetVector2FromJoint(joints[JointType.SpineShoulder]);
//            this.leftWing.transform.localRotation = Quaternion.AngleAxis(leftWingDirection.normalized.y * -50.0F, Vector3.forward);
//            this.rightWing.transform.localRotation = Quaternion.AngleAxis(rightWingDirection.normalized.y * 50.0F, Vector3.forward);
//        }
//        {
//            Vector2 topDirection = this.topTransform.position - this.gameObject.transform.position;
//            float leftHandPositionDIstance = (leftHandPosition - this.lastLeftHandPosition).magnitude;
//            float rightHandPositionDIstance = (rightHandPosition - this.lastRightHandPosition).magnitude;
//            float middleForce = (leftHandPositionDIstance < rightHandPositionDIstance ? leftHandPositionDIstance : rightHandPositionDIstance) * 600000.0F;
//            this.gameObject.GetComponent<Rigidbody>().AddForce(topDirection * middleForce * Time.deltaTime);
//        }
//        this.lastLeftHandPosition = leftHandPosition;
//        this.lastRightHandPosition = rightHandPosition;
//    }
//}
//*/
