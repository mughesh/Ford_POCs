using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Autohand
{
    [RequireComponent(typeof(ConfigurableJoint))]
    public class PhysicsGadgetConfigurableLimitReader : MonoBehaviour
    {
        public enum ReadMode
        {
            Position,
            Rotation
        }

        [Header("Mode Settings")]
        public ReadMode readMode = ReadMode.Position;
        public bool invertValue = false;

        [Tooltip("Minimum abs value required to return nonzero output (dead zone)")]
        [Range(0f, 1f)]
        public float playRange = 0.025f;

        protected ConfigurableJoint joint;
        protected Vector3 limitAxis;
        protected Vector3 startLocalPosition;
        protected Quaternion startLocalRotation;

        int freeAxisIndex = -1;
        float value;

        protected virtual void Start()
        {
            joint = GetComponent<ConfigurableJoint>();

            // Determine which axis is free
            limitAxis = new Vector3(
                joint.xMotion == ConfigurableJointMotion.Locked ? 0 : 1,
                joint.yMotion == ConfigurableJointMotion.Locked ? 0 : 1,
                joint.zMotion == ConfigurableJointMotion.Locked ? 0 : 1
            );

            if (joint.angularXMotion != ConfigurableJointMotion.Locked)
                freeAxisIndex = 0;
            else if (joint.angularYMotion != ConfigurableJointMotion.Locked)
                freeAxisIndex = 1;
            else if (joint.angularZMotion != ConfigurableJointMotion.Locked)
                freeAxisIndex = 2;

            startLocalPosition = transform.localPosition;
            startLocalRotation = transform.localRotation;
        }

        /// <summary>
        /// Returns a -1 -> 1 value based on the current joint position or rotation.
        /// </summary>
        public float GetValue()
        {
            if (readMode == ReadMode.Position)
                return GetLinearValue();
            else
                return GetAngularValue();
        }

        float GetLinearValue()
        {
            Vector3 currPos = Vector3.Scale(transform.localPosition, limitAxis);
            Vector3 startPos = Vector3.Scale(startLocalPosition, limitAxis);

            bool positive = !(startPos.x < currPos.x || startPos.y < currPos.y || startPos.z < currPos.z);

            if (invertValue)
                positive = !positive;

            float dist = Vector3.Distance(startPos, currPos);
            value = dist / joint.linearLimit.limit;

            if (!positive)
                value *= -1f;

            if (float.IsNaN(value))
                value = 0;

            if (Mathf.Abs(value) < playRange)
                value = 0;

            return Mathf.Clamp(value, -1f, 1f);
        }

        float GetAngularValue()
        {
            if (freeAxisIndex == -1)
                return 0f;

            Quaternion localRot = transform.localRotation;
            Quaternion relativeRot = Quaternion.Inverse(startLocalRotation) * localRot;
            Vector3 angles = relativeRot.eulerAngles;
            angles.x = NormalizeAngle(angles.x);
            angles.y = NormalizeAngle(angles.y);
            angles.z = NormalizeAngle(angles.z);

            float angle = 0f;
            float low = 0f, high = 0f;

            switch (freeAxisIndex)
            {
                case 0:
                    angle = angles.x;
                    low = joint.lowAngularXLimit.limit;
                    high = joint.highAngularXLimit.limit;
                    break;
                case 1:
                    angle = angles.y;
                    low = -joint.angularYLimit.limit;
                    high = joint.angularYLimit.limit;
                    break;
                case 2:
                    angle = angles.z;
                    low = -joint.angularZLimit.limit;
                    high = joint.angularZLimit.limit;
                    break;
            }

            float normalized = Mathf.InverseLerp(low, high, angle) * 2f - 1f;

            if (invertValue)
                normalized *= -1f;

            if (Mathf.Abs(normalized) < playRange)
                normalized = 0f;

            return Mathf.Clamp(normalized, -1f, 1f);
        }

        float NormalizeAngle(float angle)
        {
            if (angle > 180f)
                angle -= 360f;
            return angle;
        }

        public ConfigurableJoint GetJoint() => joint;
    }
}
