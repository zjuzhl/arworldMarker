using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorUtility
{
    /// <summary>
    /// Determines if has state the specified animator layIndex state.
    /// </summary>
    /// <returns><c>true</c> if has state the specified animator layIndex state; otherwise, <c>false</c>.</returns>
    /// <param name="animator">Animator.</param>
    /// <param name="layIndex">Lay index.</param>
    /// <param name="state">State.</param>
    public static bool  HasState(this Animator animator, int layerIndex, string name)
    {
        return animator.HasState(layerIndex, Animator.StringToHash(name));
    }

    /// <summary>
    /// Determines if has clip the specified animator name.
    /// </summary>
    /// <returns><c>true</c> if has clip the specified animator name; otherwise, <c>false</c>.</returns>
    /// <param name="animator">Animator.</param>
    /// <param name="name">Name.</param>
    public static bool HasClip(this Animator animator, string name)
    {
        RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
        AnimationClip[] animationClips = runtimeAnimatorController.animationClips;  

        for (int i = 0; i < animationClips.Length; i++)
        {
            if (animationClips[i].name == name)
            {
                return true;
            }  
        }
        return false;
    }
}
