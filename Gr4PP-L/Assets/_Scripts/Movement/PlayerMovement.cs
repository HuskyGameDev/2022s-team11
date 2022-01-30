using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _Scripts.Movement {

    public class PlayerMovement : MonoBehaviour
    {
        [Header("Components")]
        private Rigidbody2D rb;

        [Header("Layer Masks")]
        [SerializeField] private LayerMask groundLayer;

        [Header("Movement")]
        [SerializeField] private float moveSpeed; //max horizontal speed
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        private float givenAccel;
        private float givenDecel;
        [SerializeField] private float velPower;
        [Space(10)]
        [SerializeField] private float frictionAmount;
        private float horizontalInput;

        [Header("Jump")]
        [SerializeField] private float jumpForce;
        [Range(0, 1)]
        [SerializeField] private float jumpCutMultiplier;
        [Space(10)]
        [SerializeField] private float jumpCoyoteTime;
        private float lastGroundedTime;
        private float lastJumpTime;
        [SerializeField] private float jumpWallJumpSpacing;
        private float lastGroundJump;
        [Space(10)]
        [SerializeField] private float fallGravityMultiplier;
        [SerializeField] private float terminalVelocity;
        [Space(10)]
        private bool isJumping;
        private bool jumpInputReleased;

        [Header("Wall Jump")]
        [SerializeField] private float wallJumpForce;
        [Space(10)]
        [SerializeField] private float wallJumpCoyoteTime;
        private float lastWallTime;
        [SerializeField] private float wallSlideSpeed;
        [SerializeField] private float wallJumpPreservationTime;
        private float lastWallJump;
        private bool leftWall;
        private bool rightWall;
        [Space(10)]

        [Header("Ground Collision")]
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Vector2 groundCheckSize;
        [SerializeField] private Vector2 wallCheckSize;
        [SerializeField] private Vector2 wallCheckOffset;


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();


            givenAccel = acceleration;
            givenDecel = deceleration;
        }

        private void Update()
        {
            #region Inputs
            horizontalInput = GetInput().x;

            if (Input.GetButtonDown("Jump"))
            {
                //lastJumpTime = jumpBufferTime;
            }

            if (Input.GetButtonUp("Jump"))
            {
                OnJumpUp();
            }

            if (Input.GetButtonDown("Grapple"))
            {
                rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            }
            #endregion

            #region Checks
            if(Physics2D.OverlapBox(groundCheckPoint.position - new Vector3(0, 1, 0), groundCheckSize, 0, groundLayer))
            {
                lastGroundedTime = jumpCoyoteTime;
            }

            if (Physics2D.OverlapBox(groundCheckPoint.position + new Vector3(-wallCheckOffset.x, wallCheckOffset.y, 0), wallCheckSize, 0, groundLayer))
            {
                lastWallTime = wallJumpCoyoteTime;
                leftWall = true;
                if(isJumping && lastWallJump < 0)
                {
                    isJumping = false;
                }
            } else if (Physics2D.OverlapBox(groundCheckPoint.position + new Vector3(wallCheckOffset.x, wallCheckOffset.y, 0), wallCheckSize, 0, groundLayer))
            {
                lastWallTime = wallJumpCoyoteTime;
                rightWall = true;
                if (isJumping && lastWallJump < 0)
                {
                    isJumping = false;
                }
            }

            if(rb.velocity.y < 0)
            {
                isJumping = false;
            }
            #endregion

            #region Jump
            if(lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping && !(lastWallTime > 0))
            {
                //Jump();
            } else if(lastWallTime > 0 && lastJumpTime > 0 && !isJumping)
            {
                WallJump();
            }
            #endregion

            #region Timer
            lastGroundedTime -= Time.deltaTime;
            lastJumpTime -= Time.deltaTime;
            lastWallTime -= Time.deltaTime;
            lastWallJump -= Time.deltaTime;
            lastGroundJump -= Time.deltaTime;
            #endregion
        }

        private void FixedUpdate()
        {
            HorizontalMovement();

            #region Terminal Velocity
                if(rb.velocity.y < -terminalVelocity)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -terminalVelocity);
                }
            #endregion

            #region Wall Interaction
                if (lastWallTime < 0)
                {
                    leftWall = false;
                    rightWall = false;
                }

                if(lastWallJump > 0)
                {
                    acceleration = 0;
                    deceleration = 0;
                } else
                {
                    acceleration = givenAccel;
                    deceleration = givenDecel;
                }

                //wall friction
                if (lastWallTime > 0 && rb.velocity.y < -wallSlideSpeed)
                {
                    if ((leftWall && horizontalInput < -0.01f) || (rightWall && horizontalInput > 0.01f))
                    {
                        rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
                    }
                }
            #endregion
        }

        private Vector2 GetInput()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        private void HorizontalMovement()
        {
            //
        }

        private void WallJump()
        {
            if (leftWall)
            {
                if (rb.velocity.y < 0 || lastGroundJump > 0)
                {
                    rb.velocity = new Vector2(0, 0);
                }
                rb.velocity = new Vector2(0, rb.velocity.y);
                rb.AddForce(new Vector2(1, 1) * wallJumpForce, ForceMode2D.Impulse);
            } else if(rightWall)
            {
                if (rb.velocity.y < 0 || lastGroundJump > 0)
                {
                    rb.velocity = new Vector2(0, 0);
                }
                rb.velocity = new Vector2(0, rb.velocity.y);
                rb.AddForce(new Vector2(-1, 1) * wallJumpForce, ForceMode2D.Impulse);
            }
            
            isJumping = true;
            lastWallJump = wallJumpPreservationTime;
        }

        private void OnJumpUp()
        {
            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
            }

            jumpInputReleased = true;
            lastJumpTime = 0;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckPoint.position - new Vector3(0,1,0), groundCheckSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(groundCheckPoint.position + new Vector3(wallCheckOffset.x, wallCheckOffset.y, 0), wallCheckSize);
            Gizmos.DrawWireCube(groundCheckPoint.position + new Vector3(-wallCheckOffset.x, wallCheckOffset.y, 0), wallCheckSize);
        }


    }
}