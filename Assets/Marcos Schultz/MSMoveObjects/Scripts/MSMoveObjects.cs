using System.Collections;
using UnityEngine;
using System;
using VRChemiLab.Sava.Script;

[Serializable]
public class MoveDetectionMS
{
    [Header("Move and Rotate Objects")]
    [Tooltip("The tag of objects that can be moved and rotated with the mouse.")]
    public string _tagObjects = "Respawn";
    [Tooltip("The name of objects that can be moved and rotated with the mouse.")]
    public string _nameObjects = "Cube";
    [Tooltip("The 'physic material' of objects that can be moved and rotated with the mouse.")]
    public PhysicMaterial _physicMaterialObjects;
    //
    [Header("Move but not Rotate Objects")]
    [Tooltip("The tag of objects that can be moved with the mouse.")]
    public string tagObjects = "Finish";
    [Tooltip("The name of objects that can be moved with the mouse.")]
    public string nameObjects = "Cube2";
    [Tooltip("The 'physic material' of objects that can be moved with the mouse.")]
    public PhysicMaterial physicMaterialObjects;
}
[Serializable]
public class MoveSettingsMS
{
    [Tooltip("In this variable, it is possible to define whether the objects being moved by the player will be interpolated or not. Interpolation allows smooth movement of objects and is recommended for this type of application.")]
    public RigidbodyInterpolation interpolate = RigidbodyInterpolation.Interpolate;
    [Tooltip("In this variable, it is possible to define the collision detection that the objects being moved by the player will use.")]
    public CollisionDetectionMode collisionDetection = CollisionDetectionMode.ContinuousDynamic;
    //
    public enum MoveOrRotateType { centerOfMass, pivotPoint };
    [Space(15)]
    [Tooltip("Here you can determine whether the object will be moved or rotated relative to its point of origin, or relative to its center of mass.")]
    public MoveOrRotateType MoveInRelationTo = MoveOrRotateType.centerOfMass;
    //
    public enum TypeMove { velocity, addForce };
    [Tooltip("Here it is possible to decide the type of movement that the script will make to the rigid body of the objects that can be moved, so that the system adapts better to several situations.")]
    public TypeMove TypeOfMovement = TypeMove.velocity;
    [Tooltip("If this variable is true, the mass of the object being loaded will affect the speed at which it can be rotated.")]
    public bool massAffectsRotationalSpeed = true;
    [Range(1.0f, 6.0f)]
    [Tooltip("The closest you can bring your player's camera object.")]
    public float minDistance = 1.8f;
    [Range(3.0f, 10.0f)]
    [Tooltip("As far as you can bring your player's camera object.")]
    public float maxDistance = 6;
    [Range(1.0f, 20.0f)]
    [Tooltip("The speed at which the object can be moved.")]
    public float speedOfMovement = 5;
    [Range(5.0f, 100.0f)]
    [Tooltip("The speed at which the object can be rotated.")]
    public float speedOfRotation = 25;
    [Range(1.0f, 30.0f)]
    [Tooltip("The speed at which the object can be approached or disapproved.")]
    public float speedScrool = 10;
    [Tooltip("The force with which the object can be thrown.")]
    public float throwingForce = 800;
    [Tooltip("The force with which the object can be moved. This variable only takes effect if the selected motion type is 'AddForce'.")]
    public float moveForce = 350;
}
[Serializable]
public class MoveControlsMS
{
    [Tooltip("The key that must be pressed to move the objects.")]
    public KeyCode KeyToMove = KeyCode.Mouse0;
    [Tooltip("The key that must be pressed to throwing the objects.")]
    public KeyCode KeyToThrowing = KeyCode.Mouse1;
    [Tooltip("The key that must be pressed to rotate the objects.")]
    public KeyCode keyToRotate = KeyCode.R;
    [Tooltip("Here it is possible to define the axis of the mouse scroll wheel. It is used to move objects closer or further to the player.")]
    public string mouseScroolAxis = "Mouse ScrollWheel";
    [Tooltip("Here it is possible to define the X axis of the mouse. Used to rotate objects horizontally.")]
    public string mouseX = "Mouse X";
    [Tooltip("Here it is possible to define the Y axis of the mouse. Used to rotate objects vertically.")]
    public string mouseY = "Mouse Y";
    [Space(15)]
    [Tooltip("If this variable is checked, the mouse is automatically hidden.")]
    public bool hideTheMouse = false;
    [Tooltip("If this variable is true, the code will automatically adjust the player layer to 'IgnoreRaycast'")]
    public bool setLayerInPlayer = true;
    [Tooltip("Sets whether the cursor is free, locked and invisible or only limited.")]
    public CursorLockMode _cursorLockMode = CursorLockMode.None;
}

public class MSMoveObjects : MonoBehaviour
{

    public enum RaycastType { CameraForword, ScreenToWorldPoint };
    [Tooltip("")]
    public RaycastType _raycastType = RaycastType.CameraForword;

    public enum TypeSelect { Tag, Name, PhysicsMaterial };
    [Tooltip("The way the code will detect the objects, can be by tag, by name or by 'physics material'.")]
    public TypeSelect detectionMode = TypeSelect.Tag;

    [Tooltip("Here you can configure how the code will detect objects that can be moved.")]
    public MoveDetectionMS configureObjectDetectioon;

    [Tooltip("Here you can configure how code will move objects and how physics will be processed.")]
    public MoveSettingsMS configureTheMovementOfObjects;

    [Tooltip("Here you can configure the controls the player will use to move objects.")]
    public MoveControlsMS configureControls;

    [Header("Textures")]
    [Tooltip("A 'closed hand' texture, representing that the object is being moved.")]
    public Sprite closedHandTexture;
    [Tooltip("A 'open hand' texture, representing that the object can be moved.")]
    public Sprite openHandTexture;
    [Range(0.1f, 5.0f)]
    [Tooltip("This variable will set the size with which the textures will appear on the screen.")]
    public float texturesScale = 1.0f;

    [Space(15)]
    [Tooltip("A texture that represents a point in the center of the screen.")]
    public Sprite pointInTheCenterOfTheScreen;
    [Range(0.1f, 3.0f)]
    [Tooltip("The scale of the texture that represents a point in the center of the screen.")]
    public float pointScale = 1;

    public GameObject tempObject;
    RigidbodyInterpolation tempInterpolate;
    CollisionDetectionMode tempCollisionDetectionMode;
    bool rbTempIsKinematic = false;
    bool rbTempUseGravity = true;

    bool blockRotation;
    bool canMove;
    bool blockMovement;
    bool isMoving;
    float distance;
    float rotXTemp;
    float rotYTemp;
    float tempDistance;
    float massFactor;
    RaycastHit tempHit;
    RaycastHit tempHitDown;
    Rigidbody rbTemp;
    Vector3 rayEndPoint;
    Vector3 tempDirection;
    Vector3 tempSpeed;
    Vector3 direcAddForceMode;
    Vector3 pointClosestToTheCollider;
    public static bool rotatingObject;
    Camera mainCamera;
    GameObject objClosedHand;
    GameObject objOpenHand;
    GameObject objPointOnScreen;

    private void OnValidate()
    {
        if (configureTheMovementOfObjects != null)
        {
            if (configureTheMovementOfObjects.minDistance > configureTheMovementOfObjects.maxDistance)
            {
                configureTheMovementOfObjects.maxDistance = configureTheMovementOfObjects.minDistance;
            }
        }
        if (configureObjectDetectioon != null)
        {
            if (String.IsNullOrEmpty(configureObjectDetectioon.tagObjects))
            {
                configureObjectDetectioon.tagObjects = "Finish";
            }
            if (String.IsNullOrEmpty(configureObjectDetectioon._tagObjects))
            {
                configureObjectDetectioon._tagObjects = "Respawn";
            }
        }
        if (configureControls != null)
        {
            if (String.IsNullOrEmpty(configureControls.mouseScroolAxis))
            {
                configureControls.mouseScroolAxis = "Mouse ScrollWheel";
            }
            if (String.IsNullOrEmpty(configureControls.mouseX))
            {
                configureControls.mouseX = "Mouse X";
            }
            if (String.IsNullOrEmpty(configureControls.mouseY))
            {
                configureControls.mouseY = "Mouse Y";
            }
        }
    }

    private void Awake()
    {
        distance = (configureTheMovementOfObjects.minDistance + configureTheMovementOfObjects.maxDistance) / 2;

        mainCamera = Camera.main;
        if (!mainCamera)
        {
            mainCamera = GetComponent<Camera>();
            if (!mainCamera)
            {
                mainCamera = GetComponentInChildren<Camera>();
                if (!mainCamera)
                {
                    Debug.LogError("The camera containing the code must have the 'MainCamera' tag so that you can rotate the objects.");
                    return;
                }
            }
        }

        if (configureControls.hideTheMouse)
        {
            Cursor.visible = false;
        }
        Cursor.lockState = configureControls._cursorLockMode;
        if (configureControls.setLayerInPlayer)
        {
            GameObject refTemp = transform.root.gameObject;
            refTemp.layer = 2;
            foreach (Transform trans in refTemp.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = 2;
            }
        }
        //
        float tempDistance = 0.3f;
        float tempScale = texturesScale * 0.05f;
        float tempScale2 = pointScale * 0.01f;
        float tempfloatNear = mainCamera.nearClipPlane;
        if (tempfloatNear >= tempDistance)
        {
            tempDistance = tempfloatNear + 0.02f;
        }
        if (closedHandTexture)
        {
            objClosedHand = new GameObject("objHandTextureClosed");
            objClosedHand.transform.parent = this.transform;
            objClosedHand.AddComponent<SpriteRenderer>().sprite = closedHandTexture;
            objClosedHand.transform.localPosition = new Vector3(0.0f, 0.0f, tempDistance);
            objClosedHand.transform.localScale = new Vector3(tempScale, tempScale, tempScale);
            objClosedHand.transform.localRotation = Quaternion.identity;
            objClosedHand.SetActive(false);
        }
        if (openHandTexture)
        {
            objOpenHand = new GameObject("objHandTextureOpen");
            objOpenHand.transform.parent = this.transform;
            objOpenHand.AddComponent<SpriteRenderer>().sprite = openHandTexture;
            objOpenHand.transform.localPosition = new Vector3(0.0f, 0.0f, tempDistance);
            objOpenHand.transform.localScale = new Vector3(tempScale, tempScale, tempScale);
            objOpenHand.transform.localRotation = Quaternion.identity;
            objOpenHand.SetActive(false);
        }
        if (pointInTheCenterOfTheScreen)
        {
            objPointOnScreen = new GameObject("objHandTextureClosed");
            objPointOnScreen.transform.parent = this.transform;
            objPointOnScreen.AddComponent<SpriteRenderer>().sprite = pointInTheCenterOfTheScreen;
            objPointOnScreen.transform.localPosition = new Vector3(0.0f, 0.0f, tempDistance);
            objPointOnScreen.transform.localScale = new Vector3(tempScale2, tempScale2, tempScale2);
            objPointOnScreen.transform.localRotation = Quaternion.identity;
            objPointOnScreen.SetActive(false);
        }
        tempInterpolate = RigidbodyInterpolation.None;
        tempCollisionDetectionMode = CollisionDetectionMode.Discrete;
        rbTempIsKinematic = false;
        rbTempUseGravity = true;
    }

    private void RaycastVector3Down()
    {//raycast to check if the player is stepping on top of the object he is moving. If so, the code will block the movement of objects.
        if (tempObject)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out tempHitDown, 5))
            {
                Debug.DrawLine(transform.position, tempHitDown.point, Color.red);
                DebugDrawPoint(tempHitDown.point, Color.green);
                //
                switch (detectionMode)
                {
                    case TypeSelect.Tag:
                        if (tempHitDown.transform.CompareTag(configureObjectDetectioon._tagObjects) && tempObject.transform.gameObject == tempHitDown.transform.gameObject)
                        {
                            blockMovement = true;
                        }
                        else if (tempHitDown.transform.CompareTag(configureObjectDetectioon.tagObjects) && tempObject.transform.gameObject == tempHitDown.transform.gameObject)
                        {
                            blockMovement = true;
                        }
                        else
                        {
                            blockMovement = false;
                        }
                        break;
                    case TypeSelect.Name:
                        if (tempHitDown.transform.name == configureObjectDetectioon._nameObjects && tempObject.transform.gameObject == tempHitDown.transform.gameObject)
                        {
                            blockMovement = true;
                        }
                        else if (tempHitDown.transform.name == configureObjectDetectioon.nameObjects && tempObject.transform.gameObject == tempHitDown.transform.gameObject)
                        {
                            blockMovement = true;
                        }
                        else
                        {
                            blockMovement = false;
                        }
                        break;
                    case TypeSelect.PhysicsMaterial:
                        if (configureObjectDetectioon._physicMaterialObjects)
                        {
                            if (tempHitDown.transform.GetComponent<Collider>().sharedMaterial == configureObjectDetectioon._physicMaterialObjects && tempObject.transform.gameObject == tempHitDown.transform.gameObject)
                            {
                                blockMovement = true;
                            }
                            else if (tempHitDown.transform.GetComponent<Collider>().sharedMaterial == configureObjectDetectioon.physicMaterialObjects && tempObject.transform.gameObject == tempHitDown.transform.gameObject)
                            {
                                blockMovement = true;
                            }
                            else
                            {
                                blockMovement = false;
                            }
                        }
                        else
                        {
                            blockMovement = false;
                        }
                        break;
                }
            }
            else
            {
                blockMovement = false;
            }
        }
        else
        {
            blockMovement = false;
        }
    }

    private void Update()
    {
        RaycastVector3Down();

        //raycast camera
        rayEndPoint = transform.position + transform.forward * distance;
        bool _collision = false;
        if (_raycastType == RaycastType.CameraForword)
        {
            _collision = Physics.Raycast(transform.position, transform.forward, out tempHit, (configureTheMovementOfObjects.maxDistance + 1));
        }
        else
        {
            _collision = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out tempHit, (configureTheMovementOfObjects.maxDistance + 1));
            //
            Vector3 direction = tempHit.point - transform.position;
            direction.Normalize();
            rayEndPoint = transform.position + direction * distance;
        }

        if (_collision)
        {
            Debug.DrawLine(transform.position, tempHit.point, Color.green);
            DebugDrawPoint(tempHit.point, Color.red);
            //
            switch (detectionMode)
            {
                case TypeSelect.Tag:
                    if (Vector3.Distance(transform.position, tempHit.point) <= configureTheMovementOfObjects.maxDistance && tempHit.transform.CompareTag(configureObjectDetectioon._tagObjects))
                    {
                        canMove = true;
                        if (!isMoving)
                        {
                            blockRotation = false;
                        }
                        break;
                    }
                    if (Vector3.Distance(transform.position, tempHit.point) <= configureTheMovementOfObjects.maxDistance && tempHit.transform.CompareTag(configureObjectDetectioon.tagObjects))
                    {
                        canMove = true;
                        if (!isMoving)
                        {
                            blockRotation = true;
                        }
                        break;
                    }
                    canMove = false;
                    if (!isMoving)
                    {
                        blockRotation = false;
                    }
                    break;
                case TypeSelect.Name:
                    if (Vector3.Distance(transform.position, tempHit.point) <= configureTheMovementOfObjects.maxDistance && tempHit.transform.name == configureObjectDetectioon._nameObjects)
                    {
                        canMove = true;
                        if (!isMoving)
                        {
                            blockRotation = false;
                        }
                        break;
                    }
                    if (Vector3.Distance(transform.position, tempHit.point) <= configureTheMovementOfObjects.maxDistance && tempHit.transform.name == configureObjectDetectioon.nameObjects)
                    {
                        canMove = true;
                        if (!isMoving)
                        {
                            blockRotation = true;
                        }
                        break;
                    }
                    canMove = false;
                    if (!isMoving)
                    {
                        blockRotation = false;
                    }
                    break;
                case TypeSelect.PhysicsMaterial:
                    if (configureObjectDetectioon._physicMaterialObjects)
                    {
                        if (Vector3.Distance(transform.position, tempHit.point) <= configureTheMovementOfObjects.maxDistance && tempHit.transform.GetComponent<Collider>().sharedMaterial == configureObjectDetectioon._physicMaterialObjects)
                        {
                            canMove = true;
                            if (!isMoving)
                            {
                                blockRotation = false;
                            }
                            break;
                        }
                        if (Vector3.Distance(transform.position, tempHit.point) <= configureTheMovementOfObjects.maxDistance && tempHit.transform.GetComponent<Collider>().sharedMaterial == configureObjectDetectioon.physicMaterialObjects)
                        {
                            canMove = true;
                            if (!isMoving)
                            {
                                blockRotation = true;
                            }
                            break;
                        }
                        canMove = false;
                        if (!isMoving)
                        {
                            blockRotation = false;
                        }
                    }
                    else
                    {
                        canMove = false;
                    }
                    break;
            }
            //
            if (Input.GetKeyDown(configureControls.KeyToMove) && canMove)
            {
                if (tempHit.rigidbody)
                {

                    //get
                    rbTempIsKinematic = tempHit.rigidbody.isKinematic;
                    rbTempUseGravity = tempHit.rigidbody.useGravity;
                    tempInterpolate = tempHit.rigidbody.interpolation;
                    tempCollisionDetectionMode = tempHit.rigidbody.collisionDetectionMode;
                    //set
                    tempHit.rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                    if (!rbTempIsKinematic)
                    {
                        tempHit.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    }
                    //

                    distance = Vector3.Distance(transform.position, tempHit.transform.position); // or tempHit.point
                    tempObject = tempHit.transform.gameObject;
                    PlayerPickedUpTheObject(); // Debug function
                    isMoving = true;
                }
                else
                {
                    Debug.LogWarning("The target object does not have the 'Rigidbody' component. This way, you can not interact with it.");
                }
            }
        }
        else
        {
            canMove = false;
            if (_raycastType == RaycastType.ScreenToWorldPoint)
            {
                rayEndPoint = transform.position + mainCamera.ScreenPointToRay(Input.mousePosition).direction * distance;
            }
        }
        distance += (Input.GetAxis(configureControls.mouseScroolAxis) * configureTheMovementOfObjects.speedScrool);
        distance = Mathf.Clamp(distance, configureTheMovementOfObjects.minDistance, configureTheMovementOfObjects.maxDistance);
        if (tempObject)
        {
            rbTemp = tempObject.GetComponent<Rigidbody>();
        }

        if (blockMovement && tempObject)
        {
            ResetRigidbodyParameters();
            tempObject = null;
            rbTemp = null;
            isMoving = false;
            PlayerDroppedTheObject(); // Debug function
        }
        if (Input.GetKeyUp(configureControls.KeyToMove) && tempObject)
        {
            ResetRigidbodyParameters();
            tempObject = null;
            rbTemp = null;
            isMoving = false;
            PlayerDroppedTheObject(); // Debug function
        }
        if (Input.GetKeyDown(configureControls.KeyToThrowing) && tempObject)
        {
            ResetRigidbodyParameters();
            tempDirection = rayEndPoint - transform.position;
            tempDirection.Normalize();
            rbTemp.AddForce(tempDirection * Math.Abs(configureTheMovementOfObjects.throwingForce) * 4);
            tempObject = null;
            rbTemp = null;
            isMoving = false;
            PlayerDroppedTheObject(); // Debug function
        }
        if (tempObject)
        {
            Collider[] colliderList = tempObject.GetComponentsInChildren<Collider>();
            bool theObjectIsFarAway = true;
            for (int x = 0; x < colliderList.Length; x++)
            {
                pointClosestToTheCollider = colliderList[x].ClosestPoint(transform.position);// or ClosestPointOnBounds
                DebugDrawPoint(pointClosestToTheCollider, Color.black);
                if (Vector3.Distance(transform.position, pointClosestToTheCollider) < configureTheMovementOfObjects.maxDistance)
                {
                    theObjectIsFarAway = false;
                    break;
                }
            }
            if (theObjectIsFarAway)
            {
                ResetRigidbodyParameters();
                tempObject = null;
                rbTemp = null;
                isMoving = false;
                PlayerDroppedTheObject(); // Debug function
            }
        }


        //Rotation =======================================================================
        if (tempObject && mainCamera && !blockRotation)
        {
            if (Input.GetKey(configureControls.keyToRotate))
            {
                //
                if (configureTheMovementOfObjects.massAffectsRotationalSpeed)
                {
                    massFactor = Mathf.Clamp((1.0f / rbTemp.mass), 0.01f, 0.3f);
                }
                else
                {
                    massFactor = 0.2f;
                }
                rotatingObject = true;

                //Rotation
                rotXTemp = Input.GetAxis(configureControls.mouseX) * configureTheMovementOfObjects.speedOfRotation * massFactor;
                rotYTemp = Input.GetAxis(configureControls.mouseY) * configureTheMovementOfObjects.speedOfRotation * massFactor;
                if (_raycastType == RaycastType.ScreenToWorldPoint)
                {
                    rbTemp.isKinematic = true;
                    tempObject.transform.Rotate(mainCamera.transform.up, -rotXTemp, Space.World);
                    tempObject.transform.Rotate(mainCamera.transform.right, rotYTemp, Space.World);
                }
                else
                {
                    switch (configureTheMovementOfObjects.MoveInRelationTo)
                    {
                        case MoveSettingsMS.MoveOrRotateType.centerOfMass:
                            rbTemp.angularVelocity = (-mainCamera.transform.up * rotXTemp * 2.5f);
                            rbTemp.angularVelocity += (mainCamera.transform.right * rotYTemp * 2.5f);
                            break;
                        case MoveSettingsMS.MoveOrRotateType.pivotPoint:
                            tempObject.transform.Rotate(mainCamera.transform.up, -rotXTemp, Space.World);
                            tempObject.transform.Rotate(mainCamera.transform.right, rotYTemp, Space.World);
                            break;
                    }
                }
            }
            else
            {
                rotatingObject = false;
                if (_raycastType == RaycastType.ScreenToWorldPoint)
                {
                    rbTemp.isKinematic = rbTempIsKinematic;
                }
            }
            if (Input.GetKeyUp(configureControls.keyToRotate))
            {
                rotatingObject = false;
                if (_raycastType == RaycastType.ScreenToWorldPoint)
                {
                    rbTemp.isKinematic = rbTempIsKinematic;
                }
            }
        }
        else
        {
            rotatingObject = false;
        }


        EnableSpriteElements();
    }

    private void ResetRigidbodyParameters()
    {
        if (tempObject)
        {
            if (rbTemp)
            {
                rbTemp.useGravity = rbTempUseGravity;
                rbTemp.isKinematic = rbTempIsKinematic;
                rbTemp.interpolation = tempInterpolate;
                rbTemp.collisionDetectionMode = tempCollisionDetectionMode;
                //
                tempInterpolate = RigidbodyInterpolation.None;
                tempCollisionDetectionMode = CollisionDetectionMode.Discrete;
                rbTempUseGravity = true;
                rbTempIsKinematic = false;
            }
        }
    }

    private void EnableSpriteElements()
    {
        if (_raycastType == RaycastType.CameraForword)
        {
            if (objOpenHand)
            {
                objOpenHand.SetActive(canMove && !isMoving);

                // if (tempHit.collider != null && tempHit.collider.name == "waterbeaker")
                // {
                //     UIManager.Instance.ShowDetail(true, "H<size=30>2</size>O");
                // }
                // else if (tempHit.collider != null && tempHit.collider.name.Contains("Na"))
                // {
                //     UIManager.Instance.ShowDetail(true, "Na");
                // }

            }
            if (objClosedHand)
            {
                objClosedHand.SetActive(isMoving);
            }
            if (objPointOnScreen)
            {
                objPointOnScreen.SetActive(!canMove && !isMoving);
            }

            Debug.Log("True");
        }
        else
        {

            // UIManager.Instance.ShowDetail(false, "Na");
            if (objClosedHand)
            {
                objClosedHand.SetActive(false);
            }
            if (objOpenHand)
            {
                objOpenHand.SetActive(false);

            }
            if (objPointOnScreen)
            {
                objPointOnScreen.SetActive(false);
            }

            Debug.Log("False");
        }
    }

    // Here the object is moved
    private void FixedUpdate()
    {
        if (tempObject)
        {
            if (!Input.GetKey(configureControls.keyToRotate))
            {
                rbTemp.angularVelocity = Vector3.Lerp(rbTemp.angularVelocity, Vector3.zero, 0.3f);
            }
            massFactor = 1 / rbTemp.mass;

            //Pivot or centerOfMass ========================================================================================
            Vector3 rayEndPointRef = rayEndPoint; //MoveSettingsMS.MoveOrRotateType.pivotPoint
            if (configureTheMovementOfObjects.MoveInRelationTo == MoveSettingsMS.MoveOrRotateType.centerOfMass)
            {
                rayEndPointRef = rayEndPoint + (rbTemp.transform.position - rbTemp.worldCenterOfMass);
            }

            //Direction vector =============================================================================================
            tempSpeed = (rayEndPointRef - rbTemp.transform.position);
            tempSpeed.Normalize();
            tempDistance = Vector3.Distance(rayEndPointRef, rbTemp.transform.position);
            tempDistance = Mathf.Clamp(tempDistance, 0, 1);

            //Apply forces =================================================================================================
            switch (configureTheMovementOfObjects.TypeOfMovement)
            {
                case MoveSettingsMS.TypeMove.velocity:
                    rbTemp.velocity = Vector3.Lerp(rbTemp.velocity, tempSpeed * configureTheMovementOfObjects.speedOfMovement * 6.0f * tempDistance * massFactor, 0.3f);
                    break;
                case MoveSettingsMS.TypeMove.addForce:
                    direcAddForceMode = tempSpeed * configureTheMovementOfObjects.speedOfMovement * Mathf.Abs(configureTheMovementOfObjects.moveForce) * tempDistance;
                    rbTemp.velocity = Vector3.zero;
                    rbTemp.AddForce(direcAddForceMode, ForceMode.Force);
                    break;
            }

            //move drawers and others ======================================================================================
            tempObject.SendMessage("MoveObjectInFixedUpdate", tempSpeed, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void DebugDrawPoint(Vector3 position, Color _color)
    {
        Debug.DrawLine(position, position + Vector3.up * 0.15f, _color);
        Debug.DrawLine(position, position - Vector3.up * 0.15f, _color);
        Debug.DrawLine(position, position + Vector3.right * 0.15f, _color);
        Debug.DrawLine(position, position - Vector3.right * 0.15f, _color);
        Debug.DrawLine(position, position + Vector3.forward * 0.15f, _color);
        Debug.DrawLine(position, position - Vector3.forward * 0.15f, _color);
    }





    //Additional and debug functions =======================
    private void PlayerPickedUpTheObject()
    {
        //Debug.Log("Picked up the object");
    }
    private void PlayerDroppedTheObject()
    {
        //Debug.Log("Dropped the object");
    }

    public void DropObject()
    {
        ResetRigidbodyParameters();
        if (tempObject)
        {
            if (rbTemp)
            {
                tempObject = null;
                rbTemp = null;
                isMoving = false;
                PlayerDroppedTheObject(); // Debug function
                //
                rbTempIsKinematic = false;
            }
        }
    }
    //=======================================================
}