using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace InsightAR.Internal
{
    /// <summary>
    /// re
    /// </summary>
    public enum InsightARPersonPose
    {
        UNKNOWN = -1,
        NORMAL =0,
        PUSH_HANDS_LEFT =1, //推手左
        PUSH_HANDS_RIGHT =2,  //推手右
        SHAPED_HEART =3,    // 比心
        SHAPED_HEART_2 =4,  //比心
        RAISE_LEFT_HAND_UP =5, // 抬手左
        RAISE_RIGHT_HAND_UP =6,  // 抬手右
        BOW_LEFT =7,    // 左鞠躬
        BOW_RIGHT =8,   //右鞠躬
    };

    /// <summary>
    /// 人体描述点
    /// </summary>
    public enum InsightARPersonJoint
    {
        JOINT_HEAD = 0, //头
        JOINT_NECK = 1, //脖子
        JOINT_LEFT_SHOULDER = 2, //左肩
        JOINT_LEFT_ELBOW = 3, //左肘
        JOINT_LEFT_WRIST =4, //左手腕
        JOINT_RIGHT_SHOULDER = 5, //右肩
        JOINT_RIGHT_ELBOW = 6, //右肘
        JOINT_RIGHT_WRIST = 7, //右手腕
        JOINT_LEFT_HIP = 8, // 左髋
        JOINT_LEFT_KNEE=9,  // 左膝
        JOINT_LEFT_ANKLE= 10, //左踝
        JOINT_RIGHT_HIP = 11, //右髋
        JOINT_RIGHT_KNEE = 12, //右膝
        JOINT_RIGHT_ANKLE = 13, //右踝
        JOINT_LEFT_EYE = 14, //左眼
        JOINT_RIGHT_EYE = 15, //右眼
        JOINT_LEFT_EAR = 16, //左耳
        JOINT_RIGHT_EAR = 17, //右耳
        JOINT_COUNT = 18
    }      
}
