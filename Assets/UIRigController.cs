using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum TrackingMode { Tethered, Stationary }

public enum TrackingFrequency { Constant, DistanceBased }

[System.Serializable]
public class UIRigAnchors
{
    public Transform positionAnchor;
    public Transform depthAnchor;
    public Transform heightAnchor;
}

[System.Serializable]
public class RigView
{
    public Transform _canvas;

    public UIRigAnchors anchors;
}

[System.Serializable]
public class RigModel
{
    public TrackingMode trackingMode = TrackingMode.Tethered;
    public TrackingFrequency trackingFrequency = TrackingFrequency.Constant;

    [Range(0f, 10f)]
    public float positionFollowThreshold = 0.1f;

    [Range(0f, 10f)]
    public float rotationFollowThreshold = 0.1f;

    [Range(0f, 10f)]
    public float heightFollowThrehold = 0.1f;

    [Range(0f, 10f)]
    public float FollowSpeed = 0.1f;
    [Range(0f, 10f)]
    public float RotationSpeed = 0.1f;

    public PositionModel positionModel;
    public HeightModel heightModel;
    public DepthModel depthModel;
    public RotationModel rotationModel;
}

public class UIRigController : MonoBehaviour
{
    public Status status = Status.Unitialized;

    [SerializeField]
    private RigModel model;

    [SerializeField]
    private RigView view;

    

    private void OnValidate()
    {
        if (status.Equals(Status.Initialized))
            return;

        if (view.anchors.depthAnchor == null || view.anchors.heightAnchor == null || view.anchors.positionAnchor == null)
        {
            Debug.Log("UI Rig Controller missing Anchors and or Canvas ref" );

            GameObject depthAnchor = new GameObject("Depth Anchor");
            GameObject heightAnchor = new GameObject("Heigth Anchor");
            GameObject positionAnchor = new GameObject("Position Anchor");

            positionAnchor.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            depthAnchor.transform.SetParent(positionAnchor.transform);
            heightAnchor.transform.SetParent(depthAnchor.transform);

            view.anchors.depthAnchor = depthAnchor.transform;
            view.anchors.positionAnchor = positionAnchor.transform;
            view.anchors.heightAnchor = heightAnchor.transform;

            positionAnchor.transform.SetParent(transform);
        }
        else
        {
            model.positionModel = GetComponent<PositionModel>();
            model.depthModel = GetComponent<DepthModel>();
            model.heightModel = GetComponent<HeightModel>();
            model.rotationModel = GetComponent<RotationModel>();
        }

        //Set Canvas
        if(view._canvas)
        {
            view._canvas.SetParent(view.anchors.heightAnchor);
            view._canvas.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

    }

    private void Awake()
    {
        status = Status.Initialized;

        if(model.trackingMode.Equals(TrackingMode.Stationary))
        {
            SetPosition();
            SetDepth();
            SetHeight();
        }

    }

    private void Update()
    {
        switch (model.trackingMode)
        {
            case TrackingMode.Tethered:

                switch (model.trackingFrequency)
                {
                    case TrackingFrequency.Constant:

                        SetPosition();
                        SetRotation();
                        SetDepth();
                        SetHeight();

                        break;
                    case TrackingFrequency.DistanceBased:

                        SetPositionDistanceBased();
                        SetRotationDistanceBased();
                        SetDepthDistanceBased();
                        SetHeightDistanceBased();

                        break;
                    default:
                        break;
                }


                break;
            case TrackingMode.Stationary:

                

                break;
            default:
                break;
        }


    }

    private void SetHeight()
    {
        view.anchors.heightAnchor.DOLocalMoveY(model.heightModel.TrackedHeadPosition, model.FollowSpeed);
    }

    public void SetPosition()
    {
        var targetPosition = model.positionModel.TrackedPosition;

        view.anchors.positionAnchor.DOMove(targetPosition, model.positionFollowThreshold);
    }

    public void SetDepth()
    {
        view.anchors.depthAnchor.localPosition = new Vector3(0,0, model.depthModel.TrackedDepth);
    }

    public void SetRotation()
    {
        var targetRotation = model.rotationModel.TrackedRotation;

        view.anchors.positionAnchor.DORotate(new Vector3(0, targetRotation.y, 0), model.FollowSpeed);
    }

    //Uses the movement threshold to update positioning

    private void SetHeightDistanceBased()
    {
        var currentHeight = model.heightModel.TrackedHeadPosition;
        
        var heightDifference = Mathf.Abs(currentHeight - view.anchors.heightAnchor.localPosition.y);
        
        if (heightDifference > model.heightFollowThrehold)
        {
            view.anchors.heightAnchor.DOLocalMoveY(currentHeight, model.FollowSpeed);
        }
    }

    public void SetPositionDistanceBased()
    {
        var currentPosition = model.positionModel.TrackedPosition;
        
        var distance = Vector3.Distance(currentPosition, view.anchors.positionAnchor.position);
        
        if (distance > model.positionFollowThreshold)
        {
            view.anchors.positionAnchor.DOMove(currentPosition, model.FollowSpeed);
        }
    }

    public void SetDepthDistanceBased()
    {
        var currentDepth = model.depthModel.TrackedDepth;
        
        var depthDifference = Mathf.Abs(currentDepth - view.anchors.depthAnchor.localPosition.z);
        
        if (depthDifference > model.positionFollowThreshold)
        {
            view.anchors.depthAnchor.localPosition = new Vector3(0, 0, currentDepth);
        }
    }

    public void SetRotationDistanceBased()
    {
        var currentRotation = model.rotationModel.TrackedRotation;

        var angle = Quaternion.Angle(Quaternion.Euler(view.anchors.positionAnchor.eulerAngles), Quaternion.Euler(currentRotation));
        
        if (angle > model.rotationFollowThreshold)
        {
            view.anchors.positionAnchor.DORotate(new Vector3(0, currentRotation.y, 0), model.FollowSpeed);
        }
    }






}
