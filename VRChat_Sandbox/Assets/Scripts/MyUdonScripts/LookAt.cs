
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonVersion
{
    public class LookAt : UdonSharpBehaviour
    {
        public Transform target;
        void Update()
        {

            VRCPlayerApi.TrackingData trackData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
            target.position = trackData.position;
            target.rotation = trackData.rotation;
            target.rotation = Quaternion.Euler(0, target.rotation.y, 0);
            //target.rotation= Networking.LocalPlayer.GetBoneRotation(HumanBodyBones.LastBone);
            //target.position = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.LastBone);


            transform.LookAt(target, Vector3.up);
        }

    }
}

