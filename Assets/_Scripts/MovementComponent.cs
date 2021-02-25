using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

namespace Character
{

    public class MovementComponent : MonoBehaviour
    {
        public GameObject RedUI;
        public GameObject GreenUI;
        public GameObject BlueUI;
        public GameObject YellowUI;
        private TextMeshProUGUI blueText;
        private TextMeshProUGUI greenText;
        private TextMeshProUGUI yellowText;
        private TextMeshProUGUI redText;


        [SerializeField] private float WalkSpeed;
        [SerializeField] private float JumpForce;

        private Vector2 InputVector = Vector2.zero;
        private Vector3 MoveDirection = Vector3.zero;

        //Comp
        private Animator PlayerAnimator;
        private PlayerController PlayerController;
        private Rigidbody PlayerRigidbody;

        //NPS Controllers.
        public Animator Vampire;
        public Animator OtherGuy;
        public Animator Vampire2;
        public Animator OtherGuy2;



        //Objects
        public GameObject Blue;
        public GameObject Green;
        public GameObject Yellow;
        public GameObject Red;


        public int blueCounter;
        public int greenCounter;
        public int yellowCounter;
        public int redCounter;




        public bool onSelect = false;
        public bool onUnSelect = false;

        //Reference 
        private Transform PlayerTransform;


        //Animator Hashes
        private readonly int MovementXHash = Animator.StringToHash("MovementX");
        private readonly int MovementZHash = Animator.StringToHash("MovementZ");
        private readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
        private readonly int IsCollectingHash = Animator.StringToHash("IsCollecting");

        //NPC
        private readonly int VampaireDanceHash = Animator.StringToHash("dance");
        private readonly int OtherGuyDanceHash = Animator.StringToHash("dance");


        private void Awake()
        {
            PlayerController = GetComponent<PlayerController>();
            PlayerAnimator = GetComponent<Animator>();
            PlayerRigidbody = GetComponent<Rigidbody>();

            PlayerTransform = transform;
        }

        private void Update()
        {
            if (PlayerController.IsJumping) return;

            if (!(InputVector.magnitude > 0)) MoveDirection = Vector3.zero;

            MoveDirection = PlayerTransform.forward * InputVector.y + PlayerTransform.right * InputVector.x;

            float currentSpeed = WalkSpeed;

            Vector3 movementDirection = MoveDirection * (currentSpeed * Time.deltaTime);

            PlayerTransform.position += movementDirection;
        }

        public void OnMovement(InputValue value)
        {
            if (PlayerController.IsJumping) return;
            InputVector = value.Get<Vector2>();

            PlayerAnimator.SetFloat(MovementXHash, InputVector.x);
            PlayerAnimator.SetFloat(MovementZHash, InputVector.y);
        }



        public void OnJump(InputValue button)
        {
            PlayerController.IsJumping = true;
            //Debug.Log("isJumping");
            PlayerAnimator.SetBool(IsJumpingHash, true);

            PlayerRigidbody.AddForce((transform.up + MoveDirection) * JumpForce, ForceMode.Impulse);
        }

        public void OnSelect(InputValue button)
        {
            onSelect = true;
            PlayerController.IsCollecting = true;
            PlayerAnimator.SetBool(IsCollectingHash,true);
            StartCoroutine(wait());

        }
        public void OnUnSelect(InputValue button)
        {
            onUnSelect = true;
            PlayerController.IsCollecting = true;
            PlayerAnimator.SetBool(IsCollectingHash, true);
            StartCoroutine(wait());

        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Ground") && !PlayerController.IsJumping) return;

            PlayerController.IsJumping = false;
            PlayerAnimator.SetBool(IsJumpingHash, false);
        }
       
        void OnTriggerEnter(Collider other)
        {
            Vampire.SetBool(VampaireDanceHash, true);
            OtherGuy.SetBool(OtherGuyDanceHash, true);
            Vampire2.SetBool(VampaireDanceHash, true);
            OtherGuy2.SetBool(OtherGuyDanceHash, true);
            // Debug.Log(other.tag);
            if (other.tag == "Blue" && onSelect)
            {
               
                StartCoroutine(blue());
            }
            //if (other.tag == "Blue" && onUnSelect)
            //{

            //    StartCoroutine(unblue());
            //}
            if (other.tag == "Green" && onSelect)
            {
                StartCoroutine(green());
            }
            //if (other.tag == "Green" && onUnSelect)
            //{

            //    StartCoroutine(ungreen());
            //}
            if (other.tag == "Yellow" && onSelect)
            {
                StartCoroutine(yellow());
            }
            //if (other.tag == "Yellow" && onUnSelect)
            //{
            //    StartCoroutine(unyellow());
            //}
            if (other.tag == "Red" && onSelect)
            {
                StartCoroutine(red());
            }
            //if (other.tag == "Red" && onUnSelect)
            //{
            //    StartCoroutine(unred());
            //}
            if (other.tag == "Final" && onSelect)
            {
                SceneManager.LoadScene("Restart");
            }

        }
        IEnumerator wait()
        {
           
            yield return new WaitForSeconds(1.0f);
            PlayerController.IsCollecting = false;
            PlayerAnimator.SetBool(IsCollectingHash, false);
            onSelect = false;

        }
        IEnumerator blue()
        {
            blueCounter++;
            
            blueText = BlueUI.GetComponent<TextMeshProUGUI>();
            blueText.text = blueCounter.ToString();
            Blue.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            Vampire.SetBool(VampaireDanceHash, false);
            OtherGuy.SetBool(OtherGuyDanceHash, false);
            Vampire2.SetBool(VampaireDanceHash, false);
            OtherGuy2.SetBool(OtherGuyDanceHash, false);
            Blue.gameObject.SetActive(false);
            Debug.Log(blueCounter);
        }
        IEnumerator unblue()
        {
            if (blueCounter>0)
            {
                blueCounter--;
                blueText = BlueUI.GetComponent<TextMeshProUGUI>();
                blueText.text = blueCounter.ToString();
                Blue.gameObject.SetActive(true);
                yield return new WaitForSeconds(2f);
                Blue.gameObject.SetActive(false);
                Debug.Log(blueCounter);
            }
           
        }
        IEnumerator red()
        {
            redCounter++;
            redText = RedUI.GetComponent<TextMeshProUGUI>();
            redText.text = redCounter.ToString();
            Red.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            Vampire.SetBool(VampaireDanceHash, false);
            OtherGuy.SetBool(OtherGuyDanceHash, false);
            Vampire2.SetBool(VampaireDanceHash, false);
            OtherGuy2.SetBool(OtherGuyDanceHash, false);
            Red.gameObject.SetActive(false);
        }
        IEnumerator unred()
        {
            if (redCounter > 0)
            {
                redCounter--;
                redText = RedUI.GetComponent<TextMeshProUGUI>();
                redText.text = redCounter.ToString();
                Red.gameObject.SetActive(true);
                yield return new WaitForSeconds(2f);

                Red.gameObject.SetActive(false);
            }
            
        }
        IEnumerator green()
        {
            greenCounter++;
            greenText = GreenUI.GetComponent<TextMeshProUGUI>();
            greenText.text = greenCounter.ToString();
            Green.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            Vampire.SetBool(VampaireDanceHash, false);
            OtherGuy.SetBool(OtherGuyDanceHash, false);
            Vampire2.SetBool(VampaireDanceHash, false);
            OtherGuy2.SetBool(OtherGuyDanceHash, false);
            Green.gameObject.SetActive(false);
        }
        IEnumerator ungreen()
        {
            if (greenCounter>0)
            {
            greenCounter--;
            greenText = GreenUI.GetComponent<TextMeshProUGUI>();
            greenText.text = greenCounter.ToString();
            Green.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            Green.gameObject.SetActive(false);
            }
           
        }
        IEnumerator yellow()
        {
            yellowCounter++;
            yellowText = YellowUI.GetComponent<TextMeshProUGUI>();
            yellowText.text = yellowCounter.ToString();
            Yellow.gameObject.SetActive(true);
            yield return new WaitForSeconds(6.0f);
            Vampire.SetBool(VampaireDanceHash, false);
            OtherGuy.SetBool(OtherGuyDanceHash, false);
            Vampire2.SetBool(VampaireDanceHash, false);
            OtherGuy2.SetBool(OtherGuyDanceHash, false);
            Yellow.gameObject.SetActive(false);
        }
        IEnumerator unyellow()
        {
            if (yellowCounter>0)
            {
                yellowCounter--;
                yellowText = YellowUI.GetComponent<TextMeshProUGUI>();
                yellowText.text = yellowCounter.ToString();
                Yellow.gameObject.SetActive(true);
                yield return new WaitForSeconds(2f);
                Yellow.gameObject.SetActive(false);
            }
            
        }
    }
}
